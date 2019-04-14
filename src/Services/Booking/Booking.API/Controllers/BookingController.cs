using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Application.Booking.Commands.CreateBooking;
using Booking.Application.Booking.Commands.UpdateBooking;
using Booking.Application.Booking.Queries.GetBooking;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {


        private TelemetryClient telemetry;

        public BookingController(TelemetryClient telemetry)
        {
            this.telemetry = telemetry;
        }


        // GET: api/Booking/5
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            try
            {
                var resultSet = await Mediator.Send(new GetBookingQuery() { BookingId = Id });

                if (resultSet == null)
                {
                    return NotFound();
                }

                return Ok(resultSet);

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

                var bookingOrderId = await Mediator.Send(command);
                
                //We can replace this with CreatedAtAction as well
                return StatusCode(StatusCodes.Status201Created, bookingOrderId);
            }
            catch(Exception ex){
                telemetry.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry We are Unable to Create Booking.");
            }            
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task Put(string bookingOrderId, [FromBody] UpdateBooking command)
        {
            await Mediator.Send(command);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
