using System;
using System.Collections.Generic;
using BLL.Models;
using DAL.DBModels;

namespace BLL.Tests.Constants
{
    public class TestConstants
    {
        public static UserDTO user = new UserDTO()
        {
            Email = "test@mail.com",
            ExternalId = "cus_lalalalallalaa",
            UserToken = "tok_okasdlmadsmamasd"
        };

        public static List<UserDTO> users = new List<UserDTO>()
        {
            new UserDTO()
            {
                Email = "admin@mail.com",
                ExternalId = "cus_korkorkrokor",
                UserToken = "tok_mkasdjkjlnkmasd"
            },
            new UserDTO()
            {
                Email = "test@mail.com",
                ExternalId = "cus_lalalalallalaa",
                UserToken = "tok_okasdlmadsmamasd"
            }
        };

        public static PaymentModel payment = new PaymentModel
        {
            Amount = 1000,
            CardToken = "tok_visa",
            Currency = "usd",
            Email = "kram@mail.com",
            OrderId = 1,
            VendorId = 1,
            SaveCard = false,
            Type = "auth",
            UserId = 1,
        };

        public static List<TransactionDTO> transactionsDTO = new List<TransactionDTO>()
        {
            new TransactionDTO()
            {
                ExternalId = "ch_1Fjaaaaa4jzrjuSWkyXJbu3Yv",
                TransactionType = "charge",
                Amount = 1000,
                Status = "succeeded",
                Metadata = "some metadata",
                TransactionTime = DateTime.Now,
                UserId = 1,
                OrderId = 1,
                VendorId = 1,
                Instrument = "card",
                Response = "some response",
            }, new TransactionDTO()
            {
                ExternalId = "ch_1Fjaaaaa4jzrjuSWkyXJbu3Yv",
                TransactionType = "charge retry",
                Amount = 1000,
                Status = "succeeded",
                Metadata = "some metadata retry",
                TransactionTime = DateTime.Now,
                UserId = 1,
                OrderId = 2,
                VendorId = 3,
                Instrument = "card retry",
                Response = "some response retry",
            }
        };

        public static List<TransactionModel> transactions = new List<TransactionModel>()
        {
            new TransactionModel()
            {
                ExternalId = "ch_1Fjaaaaa4jzrjuSWkyXJbu3Yv",
                TransactionType = "charge",
                Amount = 1000,
                Status = "succeeded",
                Metadata = "some metadata",
                TransactionTime = DateTime.Now,
                UserId = 1,
                OrderId = 1,
                VendorId = 1,
                Instrument = "card",
                Response = "some response",
            },
            new TransactionModel()
            {
                ExternalId = "ch_1Fjaaaaa4jzrjuSWkyXJbu3Yv",
                TransactionType = "charge retry",
                Amount = 1000,
                Status = "succeeded",
                Metadata = "some metadata retry",
                TransactionTime = DateTime.Now,
                UserId = 2,
                OrderId = 2,
                VendorId = 3,
                Instrument = "card retry",
                Response = "some response retry",
            }
        };
    }
}