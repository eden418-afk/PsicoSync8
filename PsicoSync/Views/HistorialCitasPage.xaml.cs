using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace PsicoSync.Views;

public partial class HistorialCitasPage : ContentPage
{
    ServicioCita servicioCitas = new();
    public ObservableCollection<objCita> Citas { get; set; }
    public ObservableCollection<objCita> CitasFiltradas { get; set; }

    ColorHandler Colores = new();

    string filtroTipo = "Todo";
    public HistorialCitasPage()
    {
        InitializeComponent();

        Citas = new ObservableCollection<objCita>();
        CitasFiltradas = new ObservableCollection<objCita>();

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarCitas();
    }

    private async void CargarCitas()
    {
        Citas.Clear();
        CitasFiltradas.Clear();
        var citas = await servicioCitas.GetItemsAsync(true);
        foreach (var cita in citas)
        {
            Citas.Add(cita);
            CitasFiltradas.Add(cita);
        }
        SearchText = "";

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
            var searchTextLower = RemoveDiacritics(SearchText.ToLower());
            citasFiltradas = citasFiltradas.Where(c =>
                (c.Tipo != null && RemoveDiacritics(c.Tipo).ToLower().Contains(searchTextLower)) ||
                (c.Modalidad != null && RemoveDiacritics(c.Modalidad).ToLower().Contains(searchTextLower)) ||
                (c.Cliente != null && c.Cliente.Nombre != null && RemoveDiacritics(c.Cliente.Nombre).ToLower().Contains(searchTextLower)) ||
                (c.TipoCita != null && c.TipoCita.Nombre != null && RemoveDiacritics(c.TipoCita.Nombre).ToLower().Contains(searchTextLower)) ||
                (c.FechaString != null && RemoveDiacritics(c.FechaString).ToLower().Contains(searchTextLower)));
        }

        CitasFiltradas.Clear();
        foreach (var cita in citasFiltradas)
        {
            CitasFiltradas.Add(cita);
        }
    }

    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
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