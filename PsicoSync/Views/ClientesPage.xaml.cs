using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;

namespace PsicoSync.Views;

public partial class ClientesPage : ContentPage
{
    ServicioCliente servicioClientes = new();
    ObservableCollection<objCliente> Clientes { get; set; }
    ColorHandler Colores = new();
    public ClientesPage()
	{
		InitializeComponent();
        BindingContext = this;

        Clientes = new ObservableCollection<objCliente>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarClientes();
    }

    private async void CargarClientes()
    {
        Clientes.Clear();
        var clientes = await servicioClientes.GetItemsAsync();
        foreach (var cliente in clientes)
        {
            Clientes.Add(cliente);
        }
        ListViewClientes.ItemsSource = Clientes;
    }

    private void ListViewClientes_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        objCliente cliente = e.Item as objCliente;

        var navigationParameter = new ShellNavigationQueryParameters
            {
                { "Cliente", cliente }
            };

        Shell.Current.GoToAsync(nameof(NuevoClientePage), true, navigationParameter);
    }

    private void btnTodo_Clicked(object sender, EventArgs e)
    {
        btnTodo.BackgroundColor = Colores.Primary;
        btnTodo.TextColor = Colores.White;
        btnPacientes.BackgroundColor = Colores.White;
        btnPacientes.TextColor = Colores.Black;
        btnPsicologos.BackgroundColor = Colores.White;
        btnPsicologos.TextColor = Colores.Black;
    }

    private void btnPacientes_Clicked(object sender, EventArgs e)
    {
        btnPacientes.BackgroundColor = Colores.Primary;
        btnPacientes.TextColor = Colores.White;
        btnTodo.BackgroundColor = Colores.White;
        btnTodo.TextColor = Colores.Black;
        btnPsicologos.BackgroundColor = Colores.White;
        btnPsicologos.TextColor = Colores.Black;
    }

    private void btnPsicologos_Clicked(object sender, EventArgs e)
    {
        btnPsicologos.BackgroundColor = Colores.Primary;
        btnPsicologos.TextColor = Colores.White;
        btnPacientes.BackgroundColor = Colores.White;
        btnPacientes.TextColor = Colores.Black;
        btnTodo.BackgroundColor = Colores.White;
        btnTodo.TextColor = Colores.Black;
    }

    private void btnAgregarCliente_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(NuevoClientePage), true);
    }

    private async void ListViewClientes_Refreshing(object sender, EventArgs e)
    {
        CargarClientes();
        ListViewClientes.EndRefresh();
    }
}