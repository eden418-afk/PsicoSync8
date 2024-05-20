using PsicoSync.Model;

namespace PsicoSync.Views;

public partial class CitaPage : ContentPage
{
	public objCita Cita { get; set; }
	public CitaPage(objCita cita)
	{
		InitializeComponent();

		Cita = cita;
		BindingContext = this;
	}
}