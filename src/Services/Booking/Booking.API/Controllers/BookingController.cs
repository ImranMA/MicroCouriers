using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Application.Booking.Commands.CreateBooking;
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
          
        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<string>> Get(string bookingId)
        {
            return Ok(await Mediator.Send(new GetBookingQuery { BookingId = bookingId }));           
        }

        // GET: api/Booking/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateBookingCommand command)
        {   
            var bookingOrderId = await Mediator.Send(command);
            return Ok(bookingOrderId);
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
