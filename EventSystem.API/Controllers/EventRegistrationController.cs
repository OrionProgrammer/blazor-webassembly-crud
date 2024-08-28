using EventSystem.Domain;
using EventSystem.Helpers;
using EventSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventSystem.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EventRegistrationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventRegistrationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/v1/EventRegistration/{eventId}/{userId}
        //[Authorize(Roles = "Admin, User")]
        [HttpGet("{eventId:long}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> GetEventRegistration([FromRoute] long eventId, [FromRoute] string userId)
        {
            if (eventId == 0 || string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { status = "404", message = "EventId or UserId is Invalid!" });
            }

            var isRegistered = await _unitOfWork.EventRegistrationRepository.IsUserRegisteredForEvent(eventId, userId);
            return Ok(isRegistered);
        }

        // POST: api/v1/EventRegistration
        //[Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EventRegistration>> PostEventRegistration([FromBody] EventRegistrationModel eventRegistrationModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "400", message = "Model State is Invalid" });

            //create a unique reference number
            string uniqueString = Guid.NewGuid().ToString().Split('-')[0];
            var refNumber = $"EV-{eventRegistrationModel.EventId}-{uniqueString}";

            //map model to data entity
            EventRegistration eventRegistration = new()
            {
                EventId = eventRegistrationModel.EventId,
                UserId = eventRegistrationModel.UserId,
                ReferenceNumber = refNumber
            };

            await _unitOfWork.EventRegistrationRepository.InsertAsync(eventRegistration);

            // update the attendance count for the event.
            Event @event = await _unitOfWork.EventRepository.GetByIdLongAsync(eventRegistrationModel.EventId);
            @event.AttendanceCount++;
            await _unitOfWork.EventRepository.UpdateLongAsync(@event, @event.Id);

            await _unitOfWork.Complete();

            return Ok(eventRegistration);
        }
    }
}
