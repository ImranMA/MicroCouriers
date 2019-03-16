using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracking.Application.DTO;
using Tracking.Application.Interface;

namespace Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private ITrackingService _trackingService;


        public TrackingController(ITrackingService trackingservice)
        {
            _trackingService = trackingservice;
        }

        // GET: api/Tracking
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Tracking/5
        [HttpGet("{id}")]
        public async Task<TrackingDTO> Get(string id)
        {
            return await _trackingService.FindByIdAsync(id) ;
        }

        // POST: api/Tracking
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Tracking/5
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
