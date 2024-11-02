using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalM.Models;

public class PaymentDetails
{
    public int MovieId { get; set; }
    public List<string> SelectedSeats { get; set; }
    public decimal TotalAmount { get; set; }

    public PaymentDetails(int movieId, List<string> selectedSeats, decimal totalAmount)
    {
        MovieId = movieId;
        SelectedSeats = selectedSeats;
        TotalAmount = totalAmount;
    }

    public PaymentDetails()
    {
    }
}
