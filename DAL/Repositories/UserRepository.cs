using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly PaymentsDbContext _context;

        public UserRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> CreateUser(UserDTO user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            _context.Users.Update(user);  
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<UserDTO>> GetUser(Expression<Func<UserDTO, bool>> predicate)
        {
            return await _context.Users.Where(predicate).ToListAsync();
        }
    }
}