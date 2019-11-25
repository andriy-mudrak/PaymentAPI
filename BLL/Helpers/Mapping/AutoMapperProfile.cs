using AutoMapper;
using BLL.Models;
using DAL.DBModels;

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