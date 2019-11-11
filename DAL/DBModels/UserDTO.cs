using System;
using System.Collections.Generic;

namespace DAL.DBModels
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public string UserToken { get; set; }
    }
}
