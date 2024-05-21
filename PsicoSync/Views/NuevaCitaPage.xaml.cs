using Microsoft.Maui.Handlers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;

namespace PsicoSync.Views;

[QueryProperty(nameof(Cita), "Cita")]
public partial class NuevaCitaPage : ContentPage
{
    ServicioCliente servicioCliente;
    ServicioTipoCita servicioTipoCita;
    ServicioCita servicioCita = new ();

    objCita cita;
    public objCita Cita
    {
        get => cita;
        set
        {
            cita = value;
            OnPropertyChanged();
            if (Cita.ID != 0)
            {
                PintarCita();
                btnCancelar.IsVisible = true;
                layoutObservaciones.IsVisible = true;
                btnFinalizar.IsVisible = true;
            }
        }
    }
	public NuevaCitaPage(ServicioCliente servicioCliente, ServicioTipoCita servicioTipoCita)
	{
		InitializeComponent();

        this.servicioCliente = servicioCliente;
        this.servicioTipoCita = servicioTipoCita;

        Clientes = [];
        TipoCitas = [];

        CargarClientes();
        CargarTipoCitas();

        BindingContext = this;
    }

    private void PintarCita()
    {
        tipoEventoPicker.SelectedItem = Cita?.Tipo;
        modalidadPicker.SelectedItem = Cita?.Modalidad;
        TipoCita = TipoCitas.Where(x => x.ID == Cita?.TipoCitaID).FirstOrDefault();
        fechaPicker.Date = Cita?.Fecha ?? DateTime.Now;
        horaPicker.Time = Cita?.Fecha.TimeOfDay ?? DateTime.Now.TimeOfDay;
        Cliente = Clientes.Where(x => x.ID == Cita?.ClienteID).FirstOrDefault();
        descripcionEntry.Text = Cita?.Descripcion;
    }

    private async void CargarTipoCitas()
    {
        var tipoCitas = await servicioTipoCita.GetItemsAsync();
        TipoCitas.Clear();
        foreach (var tipoCita in tipoCitas)
        {
            TipoCitas.Add(tipoCita);
        }

        if (Cita != null)
        {
            TipoCita = TipoCitas.Where(x => x.ID == Cita.TipoCitaID).FirstOrDefault();
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

        if (Cita != null)
        {
            Cliente = Clientes.Where(x => x.ID == Cita.ClienteID).FirstOrDefault();
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

        GuardarAtributosCita();

        Cita.Estado = "Agendada";

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

    private void GuardarAtributosCita()
    {
        DateTime fecha = fechaPicker.Date + horaPicker.Time;

        if (Cita == null)
            Cita = new();

        Cita.Tipo = tipoEventoPicker.SelectedItem.ToString();
        Cita.Modalidad = modalidadPicker.SelectedItem.ToString();
        Cita.TipoCita = TipoCita;
        Cita.TipoCitaID = TipoCita.ID;
        Cita.Fecha = fecha;
        Cita.Cliente = Cliente;
        Cita.ClienteID = Cliente.ID;
        Cita.Descripcion = descripcionEntry.Text;
        Cita.Observaciones = observacionesEntry.Text;
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

    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        await servicioCita.DeleteItemAsync(cita);

        if (cita.Tipo == "Cita")
        {
            await DisplayAlert("Éxito", "Cita cancelada exitosamente.", "OK");
        }
        else if (cita.Tipo == "Renta")
        {
            await DisplayAlert("Éxito", "Renta cancelada exitosamente.", "OK");
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void btnFinalizar_Clicked(object sender, EventArgs e)
    {
        if (!await CamposSonValidos())
            return;

        GuardarAtributosCita();

        Cita.Estado = "Finalizada";

        await servicioCita.SaveItemAsync(Cita);

        if (Cita.Tipo == "Cita")
        {
            await DisplayAlert("Éxito", "Cita finalizada exitosamente.", "OK");
        }
        else if (Cita.Tipo == "Renta")
        {
            await DisplayAlert("Éxito", "Renta finalizada exitosamente.", "OK");
        }

        await Shell.Current.GoToAsync("..");
    }
}