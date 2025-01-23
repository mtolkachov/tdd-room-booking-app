using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private RoomBookingAppDbContext _context;
        public RoomBookingService(RoomBookingAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            return _context.Rooms.Where(r => !r.Bookings.Any(b => b.Date == date));
        }

        public void Save(RoomBooking booking)
        {
            _context.RoomBookings.Add(booking);
            _context.SaveChanges();
        }
    }
}
