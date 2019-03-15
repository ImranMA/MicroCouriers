using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroCourier.Web.Commands;
using MicroCourier.Web.DTO;
using MicroCourier.Web.RESTClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace MicroCourier.Web.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        private readonly IBookingAPI _bookingAPI;

        public BookingController(IBookingAPI bookingAPi)
        {
            _bookingAPI = bookingAPi;
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var res = await _bookingAPI.GetBookingById(id);
                return Ok(res);
            }
            catch (BrokenCircuitException)
            {
                // Catches error when Basket.api is in circuit-opened mode                 
                HandleBrokenCircuitException();
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry The Service Is Not Available");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message.ToString());
            }
        }

        private void HandleBrokenCircuitException()
        {
           // ViewBag.BasketInoperativeMsg = "Booking Service is inoperative, please try later on. (Business Msg Due to Circuit-Breaker)";
        }

        // POST: api/booking
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateBookingCommand command)
        {

            try
            {
                var res = await _bookingAPI.CreatedBooking(command);
                return Ok(res);
            }
            catch (BrokenCircuitException)
            {
                // Catches error when Basket.api is in circuit-opened mode                 
                HandleBrokenCircuitException();
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry The Service Is Not Available");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message.ToString());
            }         

        }

        // PUT: api/Booking/5
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
