using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using Shouldly;

namespace RoomBookingApp.Core.Tests
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;
        public RoomBookingRequestProcessorTest()
        {
            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@test.com",
                Date = new DateTime(2021, 1, 10)
            };
            _availableRooms = [new Room() { Id = 1}];

            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(q => q.GetAvailableRooms(_request.Date)).Returns(_availableRooms);

            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
          
        }

        [Fact]
        public void Should_Return_Room_Booking_Response_With_Request_Values()
        {
            // arrange
            // act

            RoomBookingResult result = _processor.BookRoom(_request);

            // assert
            result.ShouldNotBeNull();
            result.FullName.ShouldBe(_request.FullName);
            result.Email.ShouldBe(_request.Email);
            result.Date.ShouldBe(_request.Date);
        }

        [Fact]
        public void Shoud_Throw_Exception_For_Null_Request()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Shoud_Save_Booking_Request()
        {
            RoomBooking bookingToSave = null;
            _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    bookingToSave = booking;
                });

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once);

            bookingToSave.ShouldNotBeNull();
            bookingToSave.FullName.ShouldBe(_request.FullName);
            bookingToSave.Email.ShouldBe(_request.Email);
            bookingToSave.Date.ShouldBe(_request.Date);
            bookingToSave.RoomId.ShouldBe(_availableRooms.First().Id);
        }


        [Fact]
        public void Shoud_Not_Save_Booking_Request_If_None_Available()
        {
            _availableRooms.Clear();

            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failture, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Shoud_Return_SuccessOrFailture_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
                _availableRooms.Clear();

            var result = _processor.BookRoom(_request);

            bookingSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Shoud_Return_BookingId_In_Result(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            else
            {
                _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
                    .Callback<RoomBooking>(booking =>
                    {
                        booking.Id = roomBookingId;
                    });
            }

            var result = _processor.BookRoom(_request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }

    }
}
