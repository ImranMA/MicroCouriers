using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.DTO;
using Payment.Application.Interface;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private TelemetryClient telemetry;
        private readonly IPaymentService _paymentService;


        public PaymentController(IPaymentService paymentService, TelemetryClient telemetry)
        {
            _paymentService = paymentService;
            this.telemetry = telemetry;
        }

        // GET: api/Payment
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Payment/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Payment
        //[Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentDTO paymentDTO)
        {
            try
            {
                var paymentId = await _paymentService.AddAsync(paymentDTO);
                return Ok(paymentId);
            }
            catch (Exception ex)
            {
                telemetry.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry We are Unable to Process Payment.");
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
