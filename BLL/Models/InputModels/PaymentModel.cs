
namespace BLL.Models
{
    public class PaymentModel
    {
        public string CardToken { get; set; }
        public string Currency { get; set; }
        public long Amount { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string  Email { get; set; }
        public bool SaveCard { get; set; }
        public int VendorId { get; set; }
        public string Type { get; set; }
    }
}