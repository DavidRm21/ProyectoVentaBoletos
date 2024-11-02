using ProyectoFinalM.DataAccess;
using ProyectoFinalM.Models;
using System.ComponentModel;
using System.Text.Json;
using System.Web;

namespace ProyectoFinalM.Views;

public partial class SalaPage : ContentPage ,IQueryAttributable
{
    private const int ROWS = 5;
    private const int COLS = 6;
    private const decimal TICKET_PRICE = 5500.00M;
    private readonly List<string> selectedSeats = new();
    private int movieId;
public SalaPage()
	{
		InitializeComponent();
        InitializeSeats();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        movieId = int.Parse(query["id"].ToString());
        LoadOccupiedSeats();
    }

    private void InitializeSeats()
    {
        // Configurar el grid de asientos
        for (int i = 0; i <= ROWS; i++)
        {
            SeatsGrid.RowDefinitions.Add(new RowDefinition { Height = 40 });
        }
        for (int j = 0; j <= COLS; j++)
        {
            SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
        }

        // Agregar letras de fila
        for (int i = 0; i < ROWS; i++)
        {
            var rowLabel = new Label
            {
                Text = ((char)('A' + i)).ToString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            SeatsGrid.Add(rowLabel, 0, i + 1);
        }

        // Agregar números de columna
        for (int j = 0; j < COLS; j++)
        {
            var colLabel = new Label
            {
                Text = (j + 1).ToString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            SeatsGrid.Add(colLabel, j + 1, 0);
        }

        // Agregar botones de asientos
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLS; j++)
            {
                var seatButton = new Button
                {
                    WidthRequest = 35,
                    HeightRequest = 35,
                    BackgroundColor = Colors.LightGray,
                    ClassId = $"{(char)('A' + i)}{j + 1}" // A1, A2, B1, etc.
                };

                seatButton.Clicked += OnSeatClicked;
                SeatsGrid.Add(seatButton, j + 1, i + 1);
            }
        }
    }

    private async void LoadOccupiedSeats()
    {
        // Aquí deberías cargar los asientos ocupados de tu base de datos
        var dbContext = new CinemaDbContext();
        var occupiedSeats = dbContext.Tickets
            .Where(t => t.MovieId == movieId)
            .Select(t => t.SeatNumber)
            .ToList();

        foreach (var seatNumber in occupiedSeats)
        {
            var button = SeatsGrid.Children.OfType<Button>()
                .FirstOrDefault(b => b.ClassId == seatNumber);
            if (button != null)
            {
                button.BackgroundColor = Colors.Red;
                button.IsEnabled = false;

            }
        }
        
        if((occupiedSeats.Count / 2) >= ROWS * COLS)
        {
            await DisplayAlert("Sala llena", "La sala de cine ha alcanzado su capacidad máxima y no se pueden reservar más asientos. Por favor, selecciona otra función.", "OK");
            await Shell.Current.GoToAsync("//MoviesPage");
        }
    }

    private void OnSeatClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.BackgroundColor == Colors.LightGray)
            {
                // Seleccionar asiento
                button.BackgroundColor = Colors.Green;
                selectedSeats.Add(button.ClassId);
            }
            else if (button.BackgroundColor == Colors.Green)
            {
                // Deseleccionar asiento
                button.BackgroundColor = Colors.LightGray;
                selectedSeats.Remove(button.ClassId);
            }

            UpdateSelectedSeatsInfo();
        }
    }

    private void UpdateSelectedSeatsInfo()
    {
        SelectedSeatsLabel.Text = $"Asientos seleccionados: {string.Join(", ", selectedSeats)}";
        TotalPriceLabel.Text = $"Total a pagar: ${selectedSeats.Count * TICKET_PRICE:F2}";
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        if (!selectedSeats.Any())
        {
            await DisplayAlert("Error", "Por favor seleccione al menos un asiento", "OK");
            return;
        }

        var confirm = await DisplayAlert("Confirmar",
            $"¿Desea comprar {selectedSeats.Count} boletos por ${selectedSeats.Count * TICKET_PRICE:F2}?",
            "Sí", "No");

        if (confirm)
        {
            try
            {
                var dbContext = new CinemaDbContext();
                foreach (var seat in selectedSeats)
                {
                    var ticket = new Ticket(
                        movieId: movieId,
                        seatNumber: seat,
                        purchaseDate: DateTime.Now
                    );
                    dbContext.Tickets.Add(ticket);
                }
                await dbContext.SaveChangesAsync();

                var payment = new PaymentDetails(
                        movieId,
                        selectedSeats.ToList(),
                        selectedSeats.Count * TICKET_PRICE
                    );

                await DisplayAlert("Éxito", "¡Asientos reservados con éxito!", "OK");

                var paymentDetailsJson = JsonSerializer.Serialize(payment);
                var encodedDetails = HttpUtility.UrlEncode(paymentDetailsJson);

                var uri = $"{nameof(PaymentPage)}?paymentDetails={encodedDetails}";
                await Shell.Current.GoToAsync(uri);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error en la reserva: " + ex.Message, "OK");
            }
        }
    }
}