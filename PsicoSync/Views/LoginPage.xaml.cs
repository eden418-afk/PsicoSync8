namespace PsicoSync.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        syncIndicator.IsRunning = true;
        syncIndicator.IsVisible = true;

        bool isAuthenticated = await AuthenticateUser(email, password);

        syncIndicator.IsRunning = false;
        syncIndicator.IsVisible = false;

        if (isAuthenticated)
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Correo o contraseña incorrectos.", "OK");
        }
    }

    private Task<bool> AuthenticateUser(string email, string password)
    {
        return Task.FromResult(email == "psicosync@gmail.com" && password == "password");
    }
}