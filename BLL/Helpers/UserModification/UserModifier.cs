using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers.Queries.Interfaces;
using BLL.Helpers.UserUpdating.Interfaces;
using BLL.Models;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Stripe;

namespace BLL.Helpers.UserUpdating
{
    public class UserModifier: IUserModifier
    {
        private readonly ITransactionRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserQueryCreator _userQueryCreator;

        public UserModifier(ITransactionRepository paymentRepository, IUserRepository userRepository, IUserQueryCreator userQueryCreator)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _userQueryCreator = userQueryCreator;
        }

        public async Task<UserDTO> GetOrCreateUser(PaymentModel payment)
        {
            return  (await _userRepository.GetUser(await _userQueryCreator.GetUser(payment.Email))).LastOrDefault() ?? await CreateUser(payment);
        }

        private async Task<UserDTO> CreateUser(PaymentModel payment)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Source = payment.CardToken,
                Email = payment.Email,
            };
            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions);

            var user = new UserDTO()
            {
                Email = payment.Email,
                ExternalId = customer.Id,
            };
            if (payment.SaveCard) return await _userRepository.CreateUser(user);
            else return user;
        }
    }
}