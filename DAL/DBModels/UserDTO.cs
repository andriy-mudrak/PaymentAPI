namespace PaymentAPI.DBModels
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public string UserCardToken { get; set; }
    }
}
