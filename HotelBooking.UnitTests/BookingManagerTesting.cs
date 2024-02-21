using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using System.Linq;

namespace HotelBookingUnitTests.UnitTests
{
    public class BookingManagerTesting
    {
        private IBookingManager bookingManager;
        IRepository<Booking> bookingRepository;
        public BookingManagerTesting()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }
        [Fact]
        public void CreateBooking_AvailableRoom_ReturnsTrueAndSetsBooking()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            var room = new Room();
            var booking = new Booking
            {
                StartDate = startDate,
                EndDate = endDate,
                
            };

 
            // Act
            var success = bookingManager.CreateBooking(booking);
 
            // Assert
            Assert.True(success);
            Assert.Equal(room.Id, booking.RoomId);
            Assert.True(booking.IsActive);
         }

        [Fact]
        public void CreateBooking_NoAvailableRoom_ReturnsFalse()
        {
            // Arrange
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            var booking = new Booking
            {
                StartDate = startDate,
                EndDate = endDate
            };

 
            // Act
            var success = bookingManager.CreateBooking(booking);

            // Assert
            Assert.False(success);
            Assert.False(booking.IsActive);
         }

        [Fact]
        public void CreateBooking_InvalidStartDate_ThrowsException()
        {
            // Arrange
            var booking = new Booking();
 
            // Act & Assert (using Assert.Throws for expected exception)
            Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(booking));
            Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(new Booking { StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now }));
        }


    }

}