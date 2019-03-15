using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroCourier.Web.DTO;
using MicroCourier.Web.RESTClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace MicroCourier.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentAPI _paymentAPI;

        public PaymentController(IPaymentAPI paymentAPI)
        {
            _paymentAPI = paymentAPI;
        }


        // GET: api/Payment
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/payment
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] PaymentDTO payment)
        {
            try
            {
                var res = await _paymentAPI.CreatedPayment(payment);
                return Ok(res);
            }
            catch (BrokenCircuitException)
            {
                // Catches error when payment.api is in circuit-opened mode               
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Payment Service Is Not Available. Please Try again");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message.ToString());
            }

        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
