using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroCourier.Web.DTO;
using MicroCourier.Web.RESTClients;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace MicroCourier.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private TelemetryClient telemetry;
        private readonly IPaymentAPI _paymentAPI;

        public PaymentController(IPaymentAPI paymentAPI, TelemetryClient telemetry)
        {
            _paymentAPI = paymentAPI;
            this.telemetry = telemetry;
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
        //[Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDTO payment)
        {
            try
            {
                var res = await _paymentAPI.CreatedPayment(payment);
                return Ok(res);
            }
            catch (BrokenCircuitException ex)
            {
                telemetry.TrackException(ex);
                // Catches error when payment.api is in circuit-opened mode               
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Payment Service Is Not Available. Please Try again later.");
            }
            catch (Exception ex)
            {
                telemetry.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Some problem Occured");
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
