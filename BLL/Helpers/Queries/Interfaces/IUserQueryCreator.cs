using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DBModels;

namespace BLL.Helpers.Queries.Interfaces
{
    public interface IUserQueryCreator
    {
        Task<Expression<Func<UserDTO, bool>>> GetUser(string email);
    }
}