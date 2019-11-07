using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Helpers;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using PaymentAPI.DBModels;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task Pay(string type, PaymentModel payment)
        {
            await PaymentServiceConstants.PAYMENT_OPERATIONS[type].Execute(_paymentRepository, payment);
        }
        
        public async Task<IEnumerable<TransactionModel>> GetTransactions(string type = null, int id = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            var filterType = RequestTypeValidator.GetTransactions_TypeChecker(type);
            var model = await _paymentRepository.GetTransactions(filterType, id, startDate, endDate);
            return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(model);
        }

        private void SaveCard(string email, string cardToken, int userId) 
        {
            var isUserExist = _paymentRepository.GetUser(userId);

            if (isUserExist.UserId == 0)
            {
                _paymentRepository.CreateUser(new UserDTO
                {
                    UserId = userId,
                    UserCardToken = cardToken,
                });
            }
            else
            {
                _paymentRepository.UpdateUser(new UserDTO
                {
                    UserId = userId,
                    UserCardToken = cardToken,
                });
            }
        }
    }
}