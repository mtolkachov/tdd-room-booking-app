﻿namespace RoomBookingApp.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<RoomBooking> Bookings { get; set; }
    }
}