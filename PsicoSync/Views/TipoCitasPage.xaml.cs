using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;

namespace PsicoSync.Views;

public partial class TipoCitasPage : ContentPage
{
    ServicioTipoCita servicioTipoCitas = new();
    ObservableCollection<objTipoCita> TipoCitas { get; set; }

    ColorHandler Colores = new();
    public TipoCitasPage()
	{
		InitializeComponent();
        BindingContext = this;

        TipoCitas = new ObservableCollection<objTipoCita>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarTipoCitas();
    }

    private async void CargarTipoCitas()
    {
        TipoCitas.Clear();
        var tipoCitas = await servicioTipoCitas.GetItemsAsync();
        foreach (var tipoCita in tipoCitas)
        {
            TipoCitas.Add(tipoCita);
        }
        ListViewTipoCitas.ItemsSource = TipoCitas;
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
}