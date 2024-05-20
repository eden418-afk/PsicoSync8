using PsicoSync.Model;
using PsicoSync.Servicios;

namespace PsicoSync.Views;

public partial class NuevoTipoCitaPage : ContentPage
{
    ServicioTipoCita servicioTipoCita;
	public NuevoTipoCitaPage(ServicioTipoCita servicioTipoCita)
	{
		InitializeComponent();

        this.servicioTipoCita = servicioTipoCita;
	}

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var newTipoCita = new objTipoCita
        {
            Nombre = nombreEntry.Text,
            DuracionMinutos = int.Parse(duracionEntry.Text),
            Precio = decimal.Parse(precioEntry.Text)
        };

        await servicioTipoCita.SaveItemAsync(newTipoCita);

        await DisplayAlert("Éxito", "Tipo de Cita guardado correctamente.", "OK");

        await Navigation.PopAsync();
    }
}