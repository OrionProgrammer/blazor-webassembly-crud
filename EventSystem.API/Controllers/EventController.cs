using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EventSystem.Domain;
using AutoMapper;
using EventSystem.Helpers;
using EventSystem.Model;

namespace EventSystem.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EventsController : ControllerBase
    {
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/v1/Events
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync();
            return Ok(events);
        }

        // GET: api/v1/Events/{id}
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> GetEvent([FromRoute] long id)
        {
            if(id == 0)
            {
                return BadRequest(new { status = "404", message = "Id cannot be 0" });
            }

            var @event = await _unitOfWork.EventRepository.GetByIdLongAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // POST: api/v1/Events
        //[Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Event>> PostEvent([FromBody] EventModel eventModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "400", message = "Model State is Invalid" });

            //var user = await _userManager.GetUserAsync(User);

            //if (user == null)
            //{
            //    return Unauthorized();
            //}

            //map model to data entity
            Event @event = new ()
            {
                Name = eventModel.Name,
                Description = eventModel.Description,
                Date = eventModel.Date,
                Location = eventModel.Location,
                SeatCount = eventModel.SeatCount,
                AttendanceCount = 0
            };

            await _unitOfWork.EventRepository.InsertAsync(@event);
            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() }, @event);
        }

        // PUT: api/v1/Events/{id}
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEvent([FromRoute] long id, [FromBody] EventModel eventModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = "400", message = "Model State is Invalid" });

            if (id != eventModel.Id)
            {
                return BadRequest("Event ID mismatch.");
            }

            //map model to data entity
            Event @event = new()
            {
                Id = eventModel.Id,
                Name = eventModel.Name,
                Description = eventModel.Description,
                Date = eventModel.Date,
                Location = eventModel.Location,
                SeatCount = eventModel.SeatCount,
                AttendanceCount = 0
            };

            try
            {
                await _unitOfWork.EventRepository.UpdateLongAsync(@event, id);
                await _unitOfWork.Complete();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await EventExistsAsync(id))
                {
                    return NotFound();
                }

                return BadRequest(new { status = "400", message = ex.Message });
            }
        }

        // DELETE: api/v1/Events/{id}
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent([FromRoute] long id)
        {
            if (id == 0)
            {
                return BadRequest(new { status = "404", message = "Id cannot be 0" });
            }

            _unitOfWork.EventRepository.DeleteLong(id);
            await _unitOfWork.Complete();

            return Ok();
        }

        private async Task<bool> EventExistsAsync(long id)
        {
            bool exists = await _unitOfWork.EventRepository.Exists(id);
            await _unitOfWork.Complete();
            return exists;
        }
    }
}