using System.Threading.Tasks;
using BLL.Models;
using DAL.DBModels;

namespace BLL.Helpers.UserUpdating.Interfaces
{
    public interface IUserModifier
    {
        Task<UserDTO> GetOrCreateUser(PaymentModel payment);
    }
}