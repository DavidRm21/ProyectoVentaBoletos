using ProyectoFinalM.DataAccess;

namespace ProyectoFinalM.Models
{
    public class SaleDetails
    {
        public Movie Movie { get; set; }
        public DateTime PurchaseTime { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string TicketsInfo { get; set; }
    }
}
