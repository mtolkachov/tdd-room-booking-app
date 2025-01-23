using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingRequestProcessor _roomBookingRequestProcessor;

        public RoomBookingController(
            IRoomBookingRequestProcessor roomBookingRequestProcessor)
        {
            _roomBookingRequestProcessor = roomBookingRequestProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoomAsync(RoomBookingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = _roomBookingRequestProcessor.BookRoom(request);
            if (result.Flag == Core.BookingResultFlag.Success) return Ok(result);

            ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No rooms available for given date.");

            return BadRequest(ModelState);
        }
    }
}
