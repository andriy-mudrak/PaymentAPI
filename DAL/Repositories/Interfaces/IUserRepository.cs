using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DBModels;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> CreateUser(UserDTO user);
        Task<UserDTO> UpdateUser(UserDTO user);
        Task<IEnumerable<UserDTO>> GetUser(Expression<Func<UserDTO, bool>> predicate);
    }
}