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

            context.Add(new RoomBooking { RoomId = 1, Date = date, Email = "test@test.com", FullName = "User One" });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1), Email = "test1@test.com", FullName = "User Two" });

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

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2021, 06, 09), Email = "test@test.com", FullName = "User One" };

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
