
using Microcharts;
using Microcharts.Maui;
using PsicoSync.Model;
using PsicoSync.Servicios;
using SkiaSharp;

namespace PsicoSync.Views;

public partial class ResumenPage : ContentPage
{
	ServicioCita servicioCita = new ServicioCita();
	List<objCita> CitasFinalizadas;
	public ResumenPage()
	{
		InitializeComponent();

        CitasFinalizadas = new();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		CargarCitas();
	}

    private async void CargarCitas()
    {
		CitasFinalizadas = await servicioCita.GetItemsAsync(true);

        //CargarPreciosTotales();
		CargarTipoCitasCharts();
        CargarPsicologosCharts();
        CargarCitasRentasCharts();
    }

    private void CargarCitasRentasCharts()
    {
        var tipoCitaRenta = CitasFinalizadas
            .Where(c => c.Tipo != null)
            .GroupBy(c => c.Tipo)
            .Select(group => new
            {
                Nombre = group.Key,
                Count = group.Count()
            })
            .ToList();

        decimal ingresosCitas = CitasFinalizadas.Where(c => "Cita".Equals(c.Tipo)).Sum(c => c.TipoCita.Precio);
        decimal ingresosRentas = CitasFinalizadas.Where(c => "Renta".Equals(c.Tipo)).Sum(c => c.TipoCita.Precio);

        labelCitasIngresosTotal.Text = $"Citas ingresos: ${ingresosCitas}";
        labelRentasIngresosTotal.Text = $"Rentas ingresos: ${ingresosRentas}";

        ChartEntry[] entryCitaRenta = new[]
        {
            new ChartEntry((float?)ingresosCitas)
            {
                Label = "Citas",
                ValueLabel = ingresosCitas.ToString(),
                Color = SKColor.Parse(colors[colors.Count() - 2])
            },
            new ChartEntry((float?)ingresosRentas)
            {
                Label = "Rentas",
                ValueLabel = ingresosRentas.ToString(),
                Color = SKColor.Parse(colors[colors.Count() - 3])
            }
        };

        chartCitasRentasIngresos.Chart = new DonutChart() { Entries = entryCitaRenta, LabelTextSize = 30 };
    }

    private void CargarPsicologosCharts()
    {
        var psicologoIngresos = CitasFinalizadas
            .Where(c => c.Cliente?.Tipo == "Psicologo")
            .GroupBy(c => c.Cliente.Nombre)
            .Select(group => new
            {
                Nombre = group.Key,
                Ingresos = group.Sum(x => x.TipoCita.Precio)
            })
            .ToList();

        layoutPsicologosIngresos.Children.Clear();
        foreach (var psicologo in psicologoIngresos)
        {
            layoutPsicologosIngresos.Children.Add(new Label
            {
                Text = $"{psicologo.Nombre}: {psicologo.Ingresos.ToString("C")}",
                Margin = new Thickness(0, 2, 0, 4)
            });
        }

        ChartEntry[] entryPsicologosIngresos = psicologoIngresos.Select((x, index) => new ChartEntry((float)x.Ingresos)
        {
            Label = x.Nombre,
            ValueLabel = x.Ingresos.ToString("C"), // Format as currency
            Color = SKColor.Parse(colors[index % colors.Count])
        }).ToArray();

        chartPsicologosIngresos.Chart = new BarChart() { Entries = entryPsicologosIngresos, LabelTextSize = 30 };
    }

    private void CargarTipoCitasCharts()
    {
        var tipoCitaCounts = CitasFinalizadas
            .Where(c => c.TipoCita != null) // Ensure TipoCita is not null
            .GroupBy(c => c.TipoCita.Nombre)
            .Select(group => new
            {
                Nombre = group.Key,
                Count = group.Count()
            })
            .ToList();

        layoutTiposCitasCantidad.Children.Clear();
        foreach (var tipoCita in tipoCitaCounts)
        {
            layoutTiposCitasCantidad.Children.Add(new Label
            {
                Text = $"{tipoCita.Nombre}: {tipoCita.Count}",
                Margin = new Thickness(0, 2, 0, 4)
            });
        }

        ChartEntry[] entryTipoCitasNumero = tipoCitaCounts.Select((x, index) => new ChartEntry(x.Count)
        {
            Label = x.Nombre,
            ValueLabel = x.Count.ToString(),
            Color = SKColor.Parse(colors[index % colors.Count()])
        }).ToArray();

        chartTipoCitasNumero.Chart = new DonutChart() { Entries = entryTipoCitasNumero, LabelTextSize = 30 };

        var tipoCitaIngresos = CitasFinalizadas
            .Where(c => c.TipoCita != null) // Ensure TipoCita is not null
            .GroupBy(c => c.TipoCita.Nombre)
            .Select(group => new
            {
                Nombre = group.Key,
                Ingresos = group.Sum(x => x.TipoCita.Precio)
            })
            .ToList();

        layoutTiposCitasIngresos.Children.Clear();
        foreach (var tipoCita in tipoCitaIngresos)
        {
            layoutTiposCitasIngresos.Children.Add(new Label
            {
                Text = $"{tipoCita.Nombre}: {tipoCita.Ingresos.ToString("C")}",
                Margin = new Thickness(0, 2, 0, 4)
            });
        }

        ChartEntry[] entryTipoCitaIngresos = tipoCitaIngresos.Select((x, index) => new ChartEntry((float)x.Ingresos)
        {
            Label = x.Nombre,
            ValueLabel = x.Ingresos.ToString("C"), // Format as currency
            Color = SKColor.Parse(colors[index % colors.Count()])
        }).ToArray();

        chartTipoCitasIngresos.Chart = new BarChart() { Entries = entryTipoCitaIngresos, LabelTextSize = 30};
    }

  //  private void CargarPreciosTotales()
  //  {
  //      int numeroCitasFinalizadas = Citas.Count(x => "Finalizada".Equals(x.Estado));
		//int numeroCitasAgendadas = Citas.Count(x => "Agendada".Equals(x.Estado));

		//decimal totalFinalizadas = Citas.Where(x => "Finalizada".Equals(x.Estado)).Sum(x => x.TipoCita.Precio);
		//decimal totalAgendadas = Citas.Where(x => "Agendada".Equals(x.Estado)).Sum(x => x.TipoCita.Precio);
		//decimal total = totalFinalizadas + totalAgendadas;

  //      labelCitasAgendadasTotal.Text = $"Citas agendadas: ${totalAgendadas}";
  //      labelCitasFinalizadasTotal.Text = $"Citas finalizadas: ${totalFinalizadas}";

  //      ChartEntry[] entryPreciosTotales = new[]
		//{
  //          new ChartEntry(numeroCitasFinalizadas)
  //          {
  //              Label = "Finalizadas",
  //              ValueLabel = totalFinalizadas.ToString(),
  //              Color = SKColor.Parse("#FF1493"),
				
  //          },
		//	new ChartEntry(numeroCitasAgendadas)
		//	{
  //              Label = "Agendadas",
  //              ValueLabel = totalAgendadas.ToString(),
  //              Color = SKColor.Parse("#00BFFF")
  //          }
  //      };

		//preciosTotalesChart.Chart = new DonutChart() { Entries = entryPreciosTotales, LabelTextSize=30 };
		//labelIngresos.Text = $"Ingresos totales: ${total}";
  //  }

    List<string> colors =
    [
        "#FF1493", // DeepPink
        "#00BFFF", // DeepSkyBlue
        "#32CD32", // LimeGreen
        "#FFD700", // Gold
        "#8A2BE2", // BlueViolet
        "#FF4500", // OrangeRed
        "#2E8B57", // SeaGreen
        "#00CED1", // DarkTurquoise
        "#9400D3", // DarkViolet
        "#FF6347"  // Tomato
    ];
}