using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers.Interfaces;
using BLL.Helpers.Mapping.Interfaces;
using BLL.Models;
using DAL.DBModels;
using DAL.Repositories.Interfaces;
using Stripe;

namespace BLL.Services.Interfaces
{
    public abstract class IPaymentExecute
    {
        protected readonly IPaymentRepository _paymentRepository;
        protected readonly IMappingProvider _mappingProvider;
        protected readonly IRetryHelper _retryHelper;

        protected IPaymentExecute(IPaymentRepository paymentRepository, IMappingProvider mappingProvider, IRetryHelper retryHelper)
        {
            _paymentRepository = paymentRepository;
            _mappingProvider = mappingProvider;
            _retryHelper = retryHelper;
        }

        protected async Task<UserDTO> CreateUser(PaymentModel payment)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Source = payment.CardToken,
                Email = payment.Email,
            };
            var customerService = new CustomerService();
            Customer customer = customerService.Create(customerOptions);

            var user = new UserDTO()
            {
                Email = payment.Email,
                ExternalId = customer.Id,
            };
            if (payment.SaveCard) return await _paymentRepository.CreateUser(user);
            else return user;
        }
        public abstract Task<IEnumerable<TransactionDTO>> Execute(PaymentModel payment);
    }
}