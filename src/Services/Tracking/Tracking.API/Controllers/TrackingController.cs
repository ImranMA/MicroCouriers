using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
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
        private TelemetryClient telemetry;

        public TrackingController(ITrackingService trackingservice, TelemetryClient telemetry)
        {
            _trackingService = trackingservice;
            this.telemetry = telemetry;
        }

        // GET: api/Tracking
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Tracking/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var resultSet = await _trackingService.FindByIdAsync(id);

                if (resultSet == null)
                {
                    return NotFound();
                }

                return  Ok(resultSet);
            }
            catch(Exception ex)
            {
                telemetry.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Sorry Some problem Occured In Tracking Service");
            }           
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
