using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalM.DataAccess;
using ProyectoFinalM.Models;
using ProyectoFinalM.Views;

namespace ProyectoFinalM.ViewModel
{
    public class SalesReportViewModel : BindableObject
    {
        private ObservableCollection<SaleDetails> _dailySales;
        private string _currentDate;
        private string _totalSalesInfo;

        public ObservableCollection<SaleDetails> DailySales
        {
            get => _dailySales;
            set
            {
                _dailySales = value;
                OnPropertyChanged();
            }
        }

        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }

        public string TotalSalesInfo
        {
            get => _totalSalesInfo;
            set
            {
                _totalSalesInfo = value;
                OnPropertyChanged();
            }
        }

        public ICommand ResetDatabaseCommand { get; }
        public ICommand LoadSalesCommand { get; }

        public SalesReportViewModel()
        {
            ResetDatabaseCommand = new Command(async () => await ResetDatabase());
            LoadSalesCommand = new Command(async () => await LoadSales());
            LoadSalesCommand.Execute(null);
        }

        private async Task LoadSales()
        {
            try
            {
                using var context = new CinemaDbContext();
                var today = DateTime.Today;
                var sales = await context.Sales
                    .Include(s => s.Tickets)
                    .Where(s => s.PurchaseDate.Date == today)
                    .ToListAsync();

                var movies = await context.Movies.ToDictionaryAsync(m => m.Id);

                DailySales = new ObservableCollection<SaleDetails>(
                    sales.Select(s => new SaleDetails
                    {
                        Movie = movies[s.MovieId],
                        PurchaseTime = s.PurchaseDate,
                        Subtotal = s.Subtotal,
                        Tax = s.Tax,
                        Total = s.Total,
                        TicketsInfo = $"Asientos: {string.Join(", ", s.Tickets.Select(t => t.SeatNumber))}"
                    }));

                CurrentDate = $"Ventas del {today:dd/MM/yyyy}";
                TotalSalesInfo = $"Total ventas: ${sales.Sum(s => s.Total):N2}";
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Error al cargar las ventas: {ex.Message}", "OK");
            }
        }

        private async Task ResetDatabase()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar Reinicio",
                "¿Está seguro de que desea reiniciar la base de datos? Esta acción eliminará todas las ventas.",
                "Sí", "No");

            if (confirm)
            {
                try
                {
                    using var context = new CinemaDbContext();
                    context.Sales.RemoveRange(context.Sales);
                    context.Tickets.RemoveRange(context.Tickets);
                    await context.SaveChangesAsync();
                    LoadSales(); // Recargar la vista
                    await Application.Current.MainPage.DisplayAlert("Éxito",
                        "Base de datos reiniciada correctamente", "OK");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        $"Error al reiniciar la base de datos: {ex.Message}", "OK");
                }
            }
        }

       
    }
}
