using PsicoSync.Model;
using PsicoSync.Servicios;

namespace PsicoSync.Views;

public partial class NuevoClientePage : ContentPage
{
    ServicioCliente servicioCliente;
	public NuevoClientePage(ServicioCliente servicioCliente)
	{
		InitializeComponent();

        this.servicioCliente = servicioCliente;
	}

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var newCliente = new objCliente
        {
            Nombre = nombreEntry.Text,
            Edad = int.Parse(edadEntry.Text),
            Ocupacion = ocupacionEntry.Text,
            FechaNacimiento = fechaNacimientoPicker.Date,
            AntecedentesMedicos = antecedentesMedicosEditor.Text,
            AntecedentesFamiliares = antecedentesFamiliaresEditor.Text,
            Sexo = sexoPicker.SelectedItem.ToString(),
            Telefono = telefonoEntry.Text,
            Correo = correoEntry.Text
        };

        servicioCliente.SaveItemAsync(newCliente);

        // Display success message
        await DisplayAlert("Éxito", "Cliente guardado correctamente.", "OK");

        // Navigate back
        await Navigation.PopAsync();
    }
}