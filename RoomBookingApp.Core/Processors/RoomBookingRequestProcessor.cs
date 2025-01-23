using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain.BaseModels;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor : IRoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _bookingService;

        public RoomBookingRequestProcessor(IRoomBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            if (bookingRequest is null) throw new ArgumentNullException(nameof(bookingRequest));

            var availableRooms = _bookingService.GetAvailableRooms(bookingRequest.Date);
            var result = CreateRoomBookingObject<RoomBookingResult>(bookingRequest);

            if (availableRooms.Any())
            {
                var room = availableRooms.First();
                var roomBooking = CreateRoomBookingObject<RoomBooking>(bookingRequest);
                roomBooking.RoomId = room.Id;

                _bookingService.Save(roomBooking);

                result.Flag = BookingResultFlag.Success;
                result.RoomBookingId = roomBooking.Id;
            }
            else
            {
                result.Flag = BookingResultFlag.Failture;
            }

            return result;
        }

        private TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest)
            where TRoomBooking : RoomBookingBase, new()
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Email = bookingRequest.Email,
                Date = bookingRequest.Date
            };
        }
    }
}