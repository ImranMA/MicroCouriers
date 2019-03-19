using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Application.Booking.Commands.CreateBooking;
using Booking.Application.Booking.Commands.UpdateBooking;
using Booking.Application.Booking.Queries.GetBooking;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {

        // GET: api/Booking/5
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string Id)
        {
            try
            {
                var resultSet = await Mediator.Send(new GetBookingQuery() { BookingId = Id });
                return Ok(resultSet);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Some problem Occured");
            }
                       
        }


        // POST: api/booking
        //Return Booking Reference
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            try
            {
                var bookingOrderId = await Mediator.Send(command);
                return Ok(bookingOrderId);
            }
            catch(Exception){
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
