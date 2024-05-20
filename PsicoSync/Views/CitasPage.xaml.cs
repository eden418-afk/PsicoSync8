using Microsoft.Maui.Controls.StyleSheets;
using PsicoSync.Helpers;
using PsicoSync.Model;
using PsicoSync.Servicios;
using System.Collections.ObjectModel;

namespace PsicoSync.Views;

public partial class CitasPage : ContentPage
{
    ServicioCita servicioCitas = new ServicioCita();
    ObservableCollection<objCita> Citas { get; set; }

    ColorHandler Colores = new ColorHandler();
	public CitasPage()
	{
		InitializeComponent();
        BindingContext = this;

        Citas = new ObservableCollection<objCita>();

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
        var citas = await servicioCitas.GetItemsAsync();
        foreach (var cita in citas)
        {
            Citas.Add(cita);
        }
        ListViewCitas.ItemsSource = Citas;
    }

    private void ListViewCitas_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        objCita cita = e.Item as objCita;
        Shell.Current.Navigation.PushAsync(new CitaPage(cita), true);
    }

    private void btnTodo_Clicked(object sender, EventArgs e)
    {
        btnTodo.BackgroundColor = Colores.Primary;
        btnTodo.TextColor = Colores.White;
        btnCitas.BackgroundColor = Colores.White;
        btnCitas.TextColor = Colores.Black;
        btnRentas.BackgroundColor = Colores.White;
        btnRentas.TextColor = Colores.Black;
    }

    private void btnCitas_Clicked(object sender, EventArgs e)
    {
        btnCitas.BackgroundColor = Colores.Primary;
        btnCitas.TextColor = Colores.White;
        btnTodo.BackgroundColor = Colores.White;
        btnTodo.TextColor = Colores.Black;
        btnRentas.BackgroundColor = Colores.White;
        btnRentas.TextColor = Colores.Black;
    }

    private void btnRentas_Clicked(object sender, EventArgs e)
    {
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
}