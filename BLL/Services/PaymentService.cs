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
using DAL.DBModels;

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
            var model = await _paymentRepository.GetTransactions(orderId, userId, vendorId, startDate, endDate);
            return _mapper.Map<IEnumerable<TransactionDTO>, IEnumerable<TransactionModel>>(model);
        }
    }
}