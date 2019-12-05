namespace DAL.DBModels
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public string Email { get; set; }
        public string UserToken { get; set; }
    }
}
