using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // arrange
            var date = new DateTime(2021, 06, 09);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("AvailableRoomTest").Options;

            using var context = new RoomBookingAppDbContext(dbOptions);

            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { RoomId = 1, Date = date });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // act

            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // assert

            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            // arrange
            var date = new DateTime(2021, 06, 09);

            var options = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("ShouldSaveTest").Options;

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2021, 06, 09) };

            using var context = new RoomBookingAppDbContext(options);
            var roomBookingService = new RoomBookingService(context);

            // act

            roomBookingService.Save(roomBooking);
            var bookings = context.RoomBookings.ToList();

            // assert
            var booking = Assert.Single(bookings); 

            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
        }
    }
}
