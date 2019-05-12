
using Payment.Application.DTO;
using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface IPaymentExternalGateway
    {
        Task PaymentProcess(Payments payment);       
    }
}
