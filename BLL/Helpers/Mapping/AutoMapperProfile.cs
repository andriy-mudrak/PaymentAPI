using AutoMapper;
using BLL.Models;
using PaymentAPI.DBModels;

namespace BLL.Helpers.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionDTO, TransactionModel>();
        }
    }
}