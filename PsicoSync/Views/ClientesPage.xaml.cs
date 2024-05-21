using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace PsicoSync.Views;

public partial class ClientesPage : ContentPage
{
    ServicioCliente servicioClientes = new();
    public ObservableCollection<objCliente> Clientes { get; set; }
    public ObservableCollection<objCliente> ClientesFiltrados { get; set; }
    ColorHandler Colores = new();

    string filtroTipo = "Todo";
    public ClientesPage()
	{
		InitializeComponent();

        Clientes = new ObservableCollection<objCliente>();
        ClientesFiltrados = new ObservableCollection<objCliente>();

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarClientes();
    }

    private async void CargarClientes()
    {
        Clientes.Clear();
        ClientesFiltrados.Clear();
        var clientes = await servicioClientes.GetItemsAsync();
        foreach (var cliente in clientes)
        {
            Clientes.Add(cliente);
            ClientesFiltrados.Add(cliente);
        }
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
        filtroTipo = "Todo";
        FiltrarClientes();

        btnTodo.BackgroundColor = Colores.Primary;
        btnTodo.TextColor = Colores.White;
        btnPacientes.BackgroundColor = Colores.White;
        btnPacientes.TextColor = Colores.Black;
        btnPsicologos.BackgroundColor = Colores.White;
        btnPsicologos.TextColor = Colores.Black;
    }

    private void btnPacientes_Clicked(object sender, EventArgs e)
    {
        filtroTipo = "Paciente";
        FiltrarClientes();

        btnPacientes.BackgroundColor = Colores.Primary;
        btnPacientes.TextColor = Colores.White;
        btnTodo.BackgroundColor = Colores.White;
        btnTodo.TextColor = Colores.Black;
        btnPsicologos.BackgroundColor = Colores.White;
        btnPsicologos.TextColor = Colores.Black;
    }

    private void btnPsicologos_Clicked(object sender, EventArgs e)
    {
        filtroTipo = "Psicologo";
        FiltrarClientes();

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

    private void FiltrarClientes()
    {
        var filteredClientes = Clientes.Where(c =>
            (string.IsNullOrEmpty(SearchText) || c.Nombre.ToLower().Contains(SearchText.ToLower())) &&
            (filtroTipo == "Todo" || c.Tipo == filtroTipo)
        ).ToList();

        ClientesFiltrados.Clear();
        foreach (var cliente in filteredClientes)
        {
            ClientesFiltrados.Add(cliente);
        }
    }

    string searchText = "";
    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged();
            FiltrarClientes();
        }
    }
}