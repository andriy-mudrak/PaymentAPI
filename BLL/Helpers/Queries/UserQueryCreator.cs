using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers.Queries.Interfaces;
using DAL.DBModels;

namespace BLL.Helpers.Queries
{
    public class UserQueryCreator:IUserQueryCreator
    {
        public async Task<Expression<Func<UserDTO, bool>>> GetUser(string email)
        {
            return dto => email != null && dto.Email == email;
        }
    }
}