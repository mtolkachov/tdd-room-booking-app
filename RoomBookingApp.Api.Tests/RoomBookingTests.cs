using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Api.Tests
{
    public class RoomBookingTests
    {
        private readonly Mock<IRoomBookingRequestProcessor> _roomBookingRequestProcessor;
        private readonly RoomBookingController _controller;
        private readonly RoomBookingRequest _request;
        private readonly RoomBookingResult _result;

        public RoomBookingTests()
        {
            _roomBookingRequestProcessor = new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_roomBookingRequestProcessor.Object);
            _request = new RoomBookingRequest();
            _result = new RoomBookingResult();

            _roomBookingRequestProcessor.Setup(x => x.BookRoom(_request)).Returns(_result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Success)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failture)]
        public async Task Should_Call_Booking_Method_When_Valid(int exprectedmMethodCalls, bool isModelValid, Type expectedActionResultType, BookingResultFlag bookingResultFlag)
        {
            // assert
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "Error");
            }

            _result.Flag = bookingResultFlag;

            // act
            var result = await _controller.BookRoomAsync(_request);

            // assert

            result.ShouldBeOfType(expectedActionResultType);
            _roomBookingRequestProcessor.Verify(i => i.BookRoom(_request), Times.Exactly(exprectedmMethodCalls));
        }
    }
}
