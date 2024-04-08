using Tennishallen.Data.Models;

namespace Tennishallen.Models
{
    public class InvoiceViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
		public string InvoiceId { get; set; }
		public double TotalCost { get; set; }
        public List<Reservation> CourtReservations { get; set; }
        public List<Reservation> Lessons { get; set; }
    }
}
