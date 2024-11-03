
using Microsoft.Maui;
using ProyectoFinalM.DataAccess;
using ProyectoFinalM.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Web;

namespace ProyectoFinalM.Views;


public partial class PaymentPage : ContentPage, IQueryAttributable
{
    private PaymentDetails paymentDetails;
    private const decimal TAX_RATE = 0.19M;

    public PaymentPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("paymentDetails", out var detailsObj))
        {
            var encodedDetails = detailsObj.ToString();
            var decodedDetails = HttpUtility.UrlDecode(encodedDetails);
            paymentDetails = JsonSerializer.Deserialize<PaymentDetails>(decodedDetails);

        }

        LoadMovieDetails();
        UpdatePriceDetails();
    }

    private void LoadMovieDetails()
    {
        try
        {
            var dbContext = new CinemaDbContext();
            var movie = dbContext.Movies.FirstOrDefault(m => m.Id == paymentDetails.MovieId);
            if (movie != null)
            {
                MovieTitleLabel.Text = $"Pel�cula: {movie.Titulo}";
                ShowTimeLabel.Text = $"Funci�n: {DateTime.Now:dd/MM/yyyy HH:mm}";
                SelectedSeatsLabel.Text = $"Asientos: {string.Join(", ", paymentDetails.SelectedSeats)}";
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Error al cargar detalles de la pel�cula: " + ex.Message, "OK");
        }
    }

    private void UpdatePriceDetails()
    {
        var subtotal = paymentDetails.TotalAmount;
        var tax = subtotal * TAX_RATE;
        var total = subtotal + tax;

        var culture = new CultureInfo("es-CO");

        SubtotalLabel.Text = $"{subtotal.ToString("C", culture):F2}";
        totalLabel.Text = $"{total.ToString("C", culture):F2}";
        IvaLabel.Text = $"{tax.ToString("C", culture):F2}";
    }


    private async void OnConfirmPaymentClicked(object sender, EventArgs e)
    {

        bool confirm = await DisplayAlert("Confirmar",
            "�Est� seguro de realizar el pago?",
            "S�", "No");

        if (confirm)
        {
            try
            {
                var dbContext = new CinemaDbContext();

                var subtotal = paymentDetails.TotalAmount;
                var tax = subtotal * TAX_RATE;
                var total = subtotal + tax;

                var sale = new Sale(
                paymentDetails.MovieId,
                subtotal,
                tax,
                total,
                DateTime.Now
            );

                dbContext.Sales.Add(sale);
                await dbContext.SaveChangesAsync();

                // Crear tickets asociados a la venta
                foreach (var seat in paymentDetails.SelectedSeats)
                {
                    var ticket = new Ticket(
                        paymentDetails.MovieId,
                        seat,
                        DateTime.Now
                    )
                    {
                        SaleId = sale.Id // Asociamos el ticket con la venta
                    };
                    dbContext.Tickets.Add(ticket);
                }

                await dbContext.SaveChangesAsync();
                await DisplayAlert("�xito", "�Pago realizado con �xito!", "OK");
                foreach (var sal in dbContext.Sales)
                {
                    Debug.WriteLine($"Venta ID: {sal.Id}, Pel�cula: {sal.MovieId}, Total: {sal.Total}");
                    Debug.WriteLine($"Tickets asociados: {sal.Tickets.Count}");
                }
                await Shell.Current.GoToAsync("//main/"+nameof(LoginPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al procesar el pago: " + ex.Message, "OK");
            }
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        bool cancel = await DisplayAlert("Cancelar",
            "�Est� seguro de cancelar la compra?",
            "S�", "No");

        if (cancel)
        {
            await Shell.Current.GoToAsync("//main/"+nameof(LoginPage));
        }
    }
}