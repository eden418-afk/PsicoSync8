namespace PsicoSync.Views;

public partial class ConfigPage : ContentPage
{
	public ConfigPage()
	{
		InitializeComponent();
	}

    private async void CerrarSesion_Clicked(object sender, EventArgs e)
    {
		Application.Current.MainPage = new LoginPage();
    }
}