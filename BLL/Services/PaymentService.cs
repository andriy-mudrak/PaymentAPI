using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Helpers;
using BLL.Helpers.Queries.Interfaces;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.DBModels;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ITransactionRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IPaymentProvider _paymentProvider;
        private readonly ITransactionQueryCreator _queryCreator;

        public PaymentService(ITransactionRepository paymentRepository, IMapper mapper, IPaymentProvider paymentProvider, ITransactionQueryCreator queryCreator)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _paymentProvider = paymentProvider;
            _queryCreator = queryCreator;
        }

        public async Task<IEnumerable<TransactionModel>> Pay(PaymentServiceConstants.PaymentType type, PaymentModel payment)
        {
            var response = await _paymentProvider.GetPaymentOperation(type).Execute(payment);
            return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(response);
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactions(int orderId, int userId, int vendorId, DateTime? startDate, DateTime? endDate)
        {
            var model = await _paymentRepository.GetTransactions(await _queryCreator.GetAllTransactions(orderId, userId, vendorId, startDate, endDate));
            return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(model);
        }
    }
}