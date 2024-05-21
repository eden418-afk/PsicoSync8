namespace PsicoSync.Views;

public partial class CatalogosPage : ContentPage
{
	public CatalogosPage()
	{
		InitializeComponent();
	}

    private async void OnTipoCitasTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TipoCitasPage), true);
    }

    private async void OnClientesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ClientesPage), true);
    }

    private void OnHistorialCitasTapped(object sender, TappedEventArgs e)
    {

    }
}