
using Microcharts;
using Microcharts.Maui;
using PsicoSync.Model;
using PsicoSync.Servicios;
using SkiaSharp;

namespace PsicoSync.Views;

public partial class ResumenPage : ContentPage
{
	ServicioCita servicioCita = new ServicioCita();
	List<objCita> Citas;
	public ResumenPage()
	{
		InitializeComponent();

		Citas = new();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		CargarCitas();
		
	}

    private async void CargarCitas()
    {
		Citas.Clear();
		var citasAgendadas = await servicioCita.GetItemsAsync();
		var citasFinalizadas = await servicioCita.GetItemsAsync(true);
		Citas.AddRange(citasAgendadas);
		Citas.AddRange(citasFinalizadas);

        CargarPreciosTotales();
		CargarTipoCitasCharts();
    }

    private void CargarTipoCitasCharts()
    {
        var tipoCitaCounts = Citas
            .Where(c => c.TipoCita != null) // Ensure TipoCita is not null
            .GroupBy(c => c.TipoCita.Nombre)
            .Select(group => new
            {
                Nombre = group.Key,
                Count = group.Count()
            })
            .ToList();

        ChartEntry[] entryTipoCitasNumero = tipoCitaCounts.Select(x => new ChartEntry(x.Count)
        {
            Label = x.Nombre,
            ValueLabel = x.Count.ToString(),
            Color = SKColor.Parse("#FF1493")
        }).ToArray();

        chartTipoCitasNumero.Chart = new DonutChart() { Entries = entryTipoCitasNumero, LabelTextSize = 30 };

        var tipoCitaIngresos = Citas
            .Where(c => c.TipoCita != null) // Ensure TipoCita is not null
            .GroupBy(c => c.TipoCita.Nombre)
            .Select(group => new
            {
                Nombre = group.Key,
                Ingresos = group.Sum(x => x.TipoCita.Precio)
            })
            .ToList();

        ChartEntry[] entryTipoCitaIngresos = tipoCitaIngresos.Select(x => new ChartEntry((float)x.Ingresos)
        {
            Label = x.Nombre,
            ValueLabel = x.Ingresos.ToString("C"), // Format as currency
            Color = SKColor.Parse("#00BFFF")
        }).ToArray();

        chartTipoCitasIngresos.Chart = new BarChart() { Entries = entryTipoCitaIngresos, LabelTextSize = 30};
    }

    private void CargarPreciosTotales()
    {
        int numeroCitasFinalizadas = Citas.Count(x => "Finalizada".Equals(x.Estado));
		int numeroCitasAgendadas = Citas.Count(x => "Agendada".Equals(x.Estado));

		decimal totalFinalizadas = Citas.Where(x => "Finalizada".Equals(x.Estado)).Sum(x => x.TipoCita.Precio);
		decimal totalAgendadas = Citas.Where(x => "Agendada".Equals(x.Estado)).Sum(x => x.TipoCita.Precio);
		decimal total = totalFinalizadas + totalAgendadas;

        ChartEntry[] entryPreciosTotales = new[]
		{
            new ChartEntry(numeroCitasFinalizadas)
            {
                Label = "Finalizadas",
                ValueLabel = totalFinalizadas.ToString(),
                Color = SKColor.Parse("#FF1493"),
				
            },
			new ChartEntry(numeroCitasAgendadas)
			{
                Label = "Agendadas",
                ValueLabel = totalAgendadas.ToString(),
                Color = SKColor.Parse("#00BFFF")
            }
        };

		preciosTotalesChart.Chart = new DonutChart() { Entries = entryPreciosTotales, LabelTextSize=30 };
		labelIngresos.Text = $"Ingresos totales: ${total}";
    }
}