using PsicoSync.Model;
using PsicoSync.Servicios;

namespace PsicoSync.Views;

[QueryProperty(nameof(Cliente), "Cliente")]
public partial class NuevoClientePage : ContentPage
{
    ServicioCliente servicioCliente;

    public NuevoClientePage(ServicioCliente servicioCliente)
	{
		InitializeComponent();

        this.servicioCliente = servicioCliente;
	}

    objCliente cliente;
    public objCliente Cliente
    {
        get => cliente;
        set
        {
            cliente = value;
            OnPropertyChanged();
            if (Cliente.ID != 0)
            {
                PintarCliente();
            }
        }
    }

    private void PintarCliente()
    {
        tipoPicker.SelectedItem = Cliente?.Tipo;
        nombreEntry.Text = Cliente?.Nombre;
        edadEntry.Text = Cliente?.Edad.ToString();
        ocupacionEntry.Text = Cliente?.Ocupacion;
        fechaNacimientoPicker.Date = Cliente?.FechaNacimiento ?? DateTime.Now;
        antecedentesMedicosEditor.Text = Cliente?.AntecedentesMedicos;
        antecedentesFamiliaresEditor.Text = Cliente?.AntecedentesFamiliares;
        sexoPicker.SelectedItem = Cliente?.Sexo;
        telefonoEntry.Text = Cliente?.Telefono;
        correoEntry.Text = Cliente?.Correo;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (!await CamposSonValidos())
            return;

        Cliente ??= new objCliente();

        Cliente.Tipo = tipoPicker.SelectedItem.ToString();
        Cliente.Nombre = nombreEntry.Text;
        Cliente.Edad = int.Parse(edadEntry.Text);
        Cliente.Ocupacion = ocupacionEntry.Text;
        Cliente.FechaNacimiento = fechaNacimientoPicker.Date;
        Cliente.AntecedentesMedicos = antecedentesMedicosEditor.Text;
        Cliente.AntecedentesFamiliares = antecedentesFamiliaresEditor.Text;
        Cliente.Sexo = sexoPicker.SelectedItem?.ToString();
        Cliente.Telefono = telefonoEntry.Text;
        Cliente.Correo = correoEntry.Text;

        await servicioCliente.SaveItemAsync(Cliente);

        await DisplayAlert("Éxito", "Cliente guardado correctamente.", "OK");

        await Navigation.PopAsync();
    }

    private async Task<bool> CamposSonValidos()
    {
        if (string.IsNullOrWhiteSpace(tipoPicker.SelectedItem?.ToString()))
        {
            await DisplayAlert("Error", "Seleccione el tipo de cliente", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(nombreEntry.Text))
        {
            await DisplayAlert("Error", "Introduzca el nombre", "OK");
            return false;
        }

        if (!string.IsNullOrWhiteSpace(edadEntry.Text))
        {
            if (!int.TryParse(edadEntry.Text, out _))
            {
                await DisplayAlert("Error", "La edad debe ser un número", "OK");
                return false;
            }   
        }

        return true;
    }
}