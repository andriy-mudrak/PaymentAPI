using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private readonly IPaymentProvider _paymentProvider;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IPaymentProvider paymentProvider)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _paymentProvider = paymentProvider;
        }

        public async Task<IEnumerable<TransactionModel>> Pay(PaymentServiceConstants.PaymentType type, PaymentModel payment)
        {
            var response = await _paymentProvider.GetPaymentOperation(type).Execute(payment);
            return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(response);
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactions(int orderId, int userId, int vendorId, DateTime? startDate, DateTime? endDate)
        {
            //var filterType = RequestTypeValidator.TypeChecker(type);
            var model = await _paymentRepository.GetTransactions(orderId, userId, vendorId, startDate, endDate);
            return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(model);
        }

        //public async Task<IEnumerable<TransactionModel>> GetTransactions(string type = null, int id = 0, DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    var filterType = RequestTypeValidator.TypeChecker(type);
        //    var model = await _paymentRepository.GetTransactions(filterType, id, startDate, endDate);
        //    return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(model);
        //}
        //private Expression CreateExpression()
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