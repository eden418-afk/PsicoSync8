using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace PsicoSync.Views;

public partial class TipoCitasPage : ContentPage
{
    ServicioTipoCita servicioTipoCitas = new();
    public ObservableCollection<objTipoCita> TipoCitas { get; set; }
    public ObservableCollection<objTipoCita> TipoCitasFiltradas { get; set; }

    ColorHandler Colores = new();
    public TipoCitasPage()
	{
		InitializeComponent();

        TipoCitas = new ObservableCollection<objTipoCita>();
        TipoCitasFiltradas = new ObservableCollection<objTipoCita>();

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarTipoCitas();
    }

    private async void CargarTipoCitas()
    {
        TipoCitas.Clear();
        TipoCitasFiltradas.Clear();
        var tipoCitas = await servicioTipoCitas.GetItemsAsync();
        foreach (var tipoCita in tipoCitas)
        {
            TipoCitas.Add(tipoCita);
            TipoCitasFiltradas.Add(tipoCita);
        }
    }

    private void ListViewTipoCitas_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        objTipoCita tipoCita = e.Item as objTipoCita;

        var navigationParameter = new ShellNavigationQueryParameters
            {
                { "TipoCita", tipoCita }
            };

        Shell.Current.GoToAsync(nameof(NuevoTipoCitaPage), true, navigationParameter);
    }

    private void btnAgregarTipoCita_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(NuevoTipoCitaPage), true);
    }

    private void ListViewTipoCitas_Refreshing(object sender, EventArgs e)
    {
        CargarTipoCitas();
        ListViewTipoCitas.EndRefresh();
    }

    private void FiltrarTipoCitas()
    {
        var tipoCitasFiltradas = TipoCitas.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchTextLower = RemoveDiacritics(SearchText.ToLower());
            tipoCitasFiltradas = tipoCitasFiltradas.Where(tc =>
                    RemoveDiacritics(tc.Nombre).ToLower().Contains(searchTextLower) ||
                    tc.DuracionMinutos.ToString().Contains(searchTextLower) ||
                    tc.Precio.ToString().Contains(searchTextLower));
        }

        TipoCitasFiltradas.Clear();
        foreach (var tipoCita in tipoCitasFiltradas)
        {
            TipoCitasFiltradas.Add(tipoCita);
        }
    }

    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

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


    string searchText = "";
    public string SearchText
    {
        get => searchText;
        set
        {
            if (searchText != value)
            {
                searchText = value;
                OnPropertyChanged();
                FiltrarTipoCitas();
            }
        }
    }
}