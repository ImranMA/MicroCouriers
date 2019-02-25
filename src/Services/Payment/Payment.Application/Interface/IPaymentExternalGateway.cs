
using Payment.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface IPaymentExternalGateway
    {
        Task PaymentProcess(PaymentDTO payment);       
    }
}
