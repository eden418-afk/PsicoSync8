using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;

namespace PsicoSync.Views;

public partial class CitasPage : ContentPage
{
    ServicioCita servicioCitas = new();
    ObservableCollection<objCita> Citas { get; set; }
    ObservableCollection<objCita> CitasFiltradas { get; set; }

    ColorHandler Colores = new();

    string filtroTipo = "Todo";
    public CitasPage()
	{
		InitializeComponent();
        BindingContext = this;

        Citas = [];
        CitasFiltradas = [];
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarCitas();
    }

    private async void CargarCitas()
    {
        Citas.Clear();
        var citas = await servicioCitas.GetItemsAsync();
        foreach (var cita in citas)
        {
            Citas.Add(cita);
            CitasFiltradas.Add(cita);
        }
        ListViewCitas.ItemsSource = Citas;
    }

    private void FiltrarCitas()
    {
        var citasFiltradas = Citas.AsEnumerable();

        if (filtroTipo == "Citas")
        {
            citasFiltradas = citasFiltradas.Where(c => c.Tipo == "Cita");
        }
        else if (filtroTipo == "Rentas")
        {
            citasFiltradas = citasFiltradas.Where(c => c.Tipo == "Renta");
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchTextLower = SearchText.ToLower();
            citasFiltradas = citasFiltradas.Where(c =>
                (c.Tipo != null && c.Tipo.ToLower().Contains(searchTextLower)) ||
                (c.Modalidad != null && c.Modalidad.ToLower().Contains(searchTextLower)) ||
                (c.Cliente != null && c.Cliente.Nombre != null && c.Cliente.Nombre.ToLower().Contains(searchTextLower)) ||
                (c.TipoCita != null && c.TipoCita.Nombre != null && c.TipoCita.Nombre.ToLower().Contains(searchTextLower)) ||
                (c.FechaString != null && c.FechaString.ToLower().Contains(searchTextLower)));
        }

        CitasFiltradas.Clear();
        foreach (var cita in citasFiltradas)
        {
            CitasFiltradas.Add(cita);
        }
    }

    private void ListViewCitas_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        objCita cita = e.Item as objCita;

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Cita", cita }
        };

        Shell.Current.GoToAsync(nameof(NuevaCitaPage), true, navigationParameter);

    }

    private void btnTodo_Clicked(object sender, EventArgs e)
    {
        filtroTipo = "Todo";
        FiltrarCitas();

        btnTodo.BackgroundColor = Colores.Primary;
        btnTodo.TextColor = Colores.White;
        btnCitas.BackgroundColor = Colores.White;
        btnCitas.TextColor = Colores.Black;
        btnRentas.BackgroundColor = Colores.White;
        btnRentas.TextColor = Colores.Black;
    }

    private void btnCitas_Clicked(object sender, EventArgs e)
    {
        filtroTipo = "Citas";
        FiltrarCitas();

        btnCitas.BackgroundColor = Colores.Primary;
        btnCitas.TextColor = Colores.White;
        btnTodo.BackgroundColor = Colores.White;
        btnTodo.TextColor = Colores.Black;
        btnRentas.BackgroundColor = Colores.White;
        btnRentas.TextColor = Colores.Black;
    }

    private void btnRentas_Clicked(object sender, EventArgs e)
    {
        filtroTipo = "Rentas";
        FiltrarCitas();

        btnRentas.BackgroundColor = Colores.Primary;
        btnRentas.TextColor = Colores.White;

        btnCitas.BackgroundColor = Colores.White;
        btnCitas.TextColor = Colores.Black;
        btnTodo.BackgroundColor = Colores.White;
        btnTodo.TextColor = Colores.Black;
    }

    private void btnAgregarCita_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(NuevaCitaPage), true);
    }

    private void ListViewCitas_Refreshing(object sender, EventArgs e)
    {
        CargarCitas();
        ListViewCitas.EndRefresh();
    }

    private string searchText;
    public string SearchText
    {
        get => searchText;
        set
        {
            if (searchText != value)
            {
                searchText = value;
                OnPropertyChanged();
                FiltrarCitas();
            }
        }
    }
}