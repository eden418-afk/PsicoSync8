using PsicoSync.Model;
using PsicoSync.Servicios;

namespace PsicoSync.Views;

[QueryProperty(nameof(TipoCita), "TipoCita")]
public partial class NuevoTipoCitaPage : ContentPage
{
    ServicioTipoCita servicioTipoCita;
	public NuevoTipoCitaPage(ServicioTipoCita servicioTipoCita)
	{
		InitializeComponent();

        this.servicioTipoCita = servicioTipoCita;
	}

    private void PintarTipoCita()
    {
        nombreEntry.Text = TipoCita.Nombre;
        duracionEntry.Text = TipoCita.DuracionMinutos.ToString();
        precioEntry.Text = TipoCita.Precio.ToString();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (!await CamposSonValidos())
            return;

        if (TipoCita == null)
        {
            TipoCita = new objTipoCita();
        }

        TipoCita.Nombre = nombreEntry.Text;
        TipoCita.DuracionMinutos = int.Parse(duracionEntry.Text);
        TipoCita.Precio = decimal.Parse(precioEntry.Text);

        await servicioTipoCita.SaveItemAsync(TipoCita);

        await DisplayAlert("Éxito", "Tipo de Cita guardado correctamente.", "OK");

        await Navigation.PopAsync();
    }

    private async Task<bool> CamposSonValidos()
    {
        if (string.IsNullOrWhiteSpace(nombreEntry.Text))
        {
            await DisplayAlert("Error", "El nombre del tipo de cita es requerido.", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(duracionEntry.Text))
        {
            await DisplayAlert("Error", "La duración del tipo de cita es requerida.", "OK");
            return false;
        }

        if (!int.TryParse(duracionEntry.Text, out _))
        {
            await DisplayAlert("Error", "La duración debe ser un número.", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(precioEntry.Text))
        {
            await DisplayAlert("Error", "El precio del tipo de cita es requerido.", "OK");
            return false;
        }

        if (!decimal.TryParse(precioEntry.Text, out _))
        {
            await DisplayAlert("Error", "El precio debe ser un número.", "OK");
            return false;
        }

        return true;
    }

    objTipoCita tipoCita;
    public objTipoCita TipoCita
    {
        get => tipoCita;
        set
        {
            tipoCita = value;
            OnPropertyChanged();
            if (TipoCita.ID != 0)
            {
                PintarTipoCita();
            }
        }
    }
}