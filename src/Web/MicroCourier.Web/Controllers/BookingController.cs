using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroCourier.Web.Commands;
using MicroCourier.Web.DTO;
using MicroCourier.Web.RESTClients;
using Microsoft.ApplicationInsights;
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
        private TelemetryClient telemetry;

        public BookingController(IBookingAPI bookingAPi, TelemetryClient telemetry)
        {
            _bookingAPI = bookingAPi;
            this.telemetry = telemetry;
        }

        // GET: api/Booking/5
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var res = await _bookingAPI.GetBookingById(id);

                if (res == null)
                    return NotFound("Sorry Requested ID not Found");

                return Ok(res);
            }
            catch (BrokenCircuitException ex)
            {
                telemetry.TrackException(ex);
                // Catches error when api is in circuit-opened mode                
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Booking Service Is Not Available. Please try again later.");
            }
            catch(Exception ex)
            {
                telemetry.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Some problem Occured");
            }
        }

        // POST: api/booking      
        //Return Booking Reference
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //400
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var bookingId = await _bookingAPI.CreatedBooking(command);
                return StatusCode(StatusCodes.Status201Created, bookingId);
            }
            catch (BrokenCircuitException ex)
            {
                telemetry.TrackException(ex);
                // Catches error when bookingapi is in circuit-opened mode                
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Booking Service Is Not Available. Please try again later.");
            }
            catch (Exception ex)
            {
                telemetry.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Some problem Occured In booking Service");
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
