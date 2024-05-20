using Microsoft.Maui.Handlers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;

namespace PsicoSync.Views;

public partial class NuevaCitaPage : ContentPage
{
    ServicioCliente servicioCliente;
    ServicioTipoCita servicioTipoCita;
    ServicioCita servicioCita = new ServicioCita();
	public NuevaCitaPage(ServicioCliente servicioCliente, ServicioTipoCita servicioTipoCita)
	{
		InitializeComponent();

        this.servicioCliente = servicioCliente;
        this.servicioTipoCita = servicioTipoCita;

        Clientes = new ObservableCollection<objCliente>();
        TipoCitas = new ObservableCollection<objTipoCita>();

        CargarClientes();
        CargarTipoCitas();

        BindingContext = this;
    }

    private async void CargarTipoCitas()
    {
        var tipoCitas = await servicioTipoCita.GetItemsAsync();
        TipoCitas.Clear();
        foreach (var tipoCita in tipoCitas)
        {
            TipoCitas.Add(tipoCita);
        }
    }

    private async void CargarClientes()
    {
        var clientes = await servicioCliente.GetItemsAsync();
        Clientes.Clear();
        foreach (var cliente in clientes)
        {
            Clientes.Add(cliente);
        }
    }

    private void OnEventTypeChanged(object sender, EventArgs e)
    {
        //var selectedType = eventTypePicker.SelectedItem.ToString();
        //if (selectedType == "Cita")
        //{
        //    appointmentStack.IsVisible = true;
        //    rentalStack.IsVisible = false;
        //}
        //else if (selectedType == "Renta")
        //{
        //    appointmentStack.IsVisible = false;
        //    rentalStack.IsVisible = true;
        //}
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (!await CamposSonValidos())
            return;

        DateTime fecha = fechaPicker.Date + horaPicker.Time;

        objCita cita = new objCita
        {
            Tipo = tipoEventoPicker.SelectedItem.ToString(),
            Modalidad = modalidadPicker.SelectedItem.ToString(),
            TipoCita = TipoCita,
            TipoCitaID = TipoCita.ID,
            Fecha = fecha,
            Cliente = Cliente,
            ClienteID = Cliente.ID,
            Descripcion = descripcionEntry.Text
        };

        await servicioCita.SaveItemAsync(cita);

        if (cita.Tipo == "Cita")
        {
            await DisplayAlert("Éxito", "Cita creada exitosamente.", "OK");
        }
        else if (cita.Tipo == "Renta")
        {
            await DisplayAlert("Éxito", "Renta creada exitosamente.", "OK");
        }

        await Shell.Current.GoToAsync("..");
    }

    private async Task<bool> CamposSonValidos()
    {
        // Cita o renta
        if (tipoEventoPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Por favor selecciona si es una Cita o una Renta.", "Ok");
            return false;
        }

        if (modalidadPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Por favor selecciona la modalidad.", "Ok");
            return false;
        }

        if (TipoCita == null)
        {
            await DisplayAlert("Error", "Por favor selecciona el tipo de cita.", "Ok");
            return false;
        }

        // fecha y hora

        // Cliente
        if (Cliente == null)
        {
            await DisplayAlert("Error", "Por favor selecciona el cliente.", "Ok");
            return false;
        }

        // Descripcion
        //if (string.IsNullOrEmpty(descripcionEntry.Text))
        //{
        //    await DisplayAlert("Error", "Por favor introduce la descripción.", "Ok");
        //    return false;
        //}

        return true;
    }

    private void OnModalidadChanged(object sender, EventArgs e)
    {

    }

    private async void btnNuevoCliente_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NuevoClientePage), true);
    }

    private async void btnNuevoTipoCita_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NuevoTipoCitaPage), true);
    }

    #region Clientes

    public ObservableCollection<objCliente> Clientes { get; set; }

    objCliente cliente;
    public objCliente Cliente
    {
        get => cliente;
        set
        {
            if (cliente != value)
            {
                cliente = value;
                OnPropertyChanged();
                layoutInfoCliente.IsVisible = cliente != null;
            }
        }
    }

    #endregion

    #region TipoCita

    public ObservableCollection<objTipoCita> TipoCitas { get; set; }
    objTipoCita tipoCita;
    public objTipoCita TipoCita
    {
        get => tipoCita;
        set
        {
            if (tipoCita != value)
            {
                tipoCita = value;
                OnPropertyChanged();
                layoutInfoTipoCita.IsVisible = tipoCita != null;
            }
        }
    }

    #endregion
}