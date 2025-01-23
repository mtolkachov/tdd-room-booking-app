using RoomBookingApp.Core.Domain;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Core.DataServices
{
    public interface IRoomBookingService
    {
        void Save(RoomBooking booking);
        IEnumerable<Room> GetAvailableRooms(DateTime date);
    }
}
