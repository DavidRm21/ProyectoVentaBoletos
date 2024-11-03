using ProyectoFinalM.ViewModel;

namespace ProyectoFinalM.Views;

public partial class VentasPage : ContentPage
{
    private SalesReportViewModel _viewModel;
    public VentasPage()
	{
        InitializeComponent();
        _viewModel = new SalesReportViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadSalesCommand.Execute(null);
    }
}