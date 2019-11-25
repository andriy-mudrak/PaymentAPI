using System;

namespace BLL.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        public string ExternalId { get; set; }
        public string TransactionType { get; set; }
        public long Amount { get; set; }
        public string Status { get; set; }
        public string Metadata { get; set; }
        public DateTime TransactionTime { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int VendorId { get; set; }
        public string Response { get; set; }
        public string Instrument { get; set; }
    }
}