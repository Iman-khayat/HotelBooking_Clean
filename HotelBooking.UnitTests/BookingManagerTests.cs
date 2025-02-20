using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using System.Linq;
using Moq;
using System.Collections.Generic;


namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        IRepository<Booking> bookingRepository;




        public BookingManagerTests()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);

        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_ReturnsAvailableRoom()
        {
            // This test was added to satisfy the following test design
            // principle: "Tests should have strong assertions".

            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);

            var bookingForReturnedRoomId = bookingRepository.GetAll().Where(
                b => b.RoomId == roomId
                && b.StartDate <= date
                && b.EndDate >= date
                && b.IsActive);
            // Assert
            Assert.Empty(bookingForReturnedRoomId);
        } 
        
        [Theory]
        [InlineData(1, true)]  // Room available
        [InlineData(-1, false)] // Room not available
        public void CreateBooking_DataDriven_ReturnsExpectedResult(int roomId, bool expectedResult)
        {
            // Arrange
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockRoomRepository = new Mock<IRepository<Room>>();


        [Fact]
        public void CreateBooking_AvailableRoom_BookingCreated()
        {
            // Arrange
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockRoomRepository = new Mock<IRepository<Room>>();


            mockRoomRepository.Setup(r => r.GetAll()).Returns(GetRooms(2)); // Simulate 2 available rooms

            BookingManager manager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
            Booking booking = new Booking { StartDate = DateTime.Today.AddDays(2), EndDate = DateTime.Today.AddDays(5) };

            // Act
            bool bookingCreated = manager.CreateBooking(booking);

            // Assert
            Assert.True(bookingCreated);
            mockBookingRepository.Verify(r => r.Add(booking), Times.Once); // Verify booking is added
        }
        [Fact]
        public void CreateBooking_AvailableRoom_ReturnsTrueAndSetsBookingProperties()
        {
            // Arrange
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            int availableRoomId = 10;
            mockRoomRepository.Setup(r => r.GetAll()).Returns(GetRooms(2)); // 2 rooms
            mockBookingRepository.Setup(r => r.GetAll()).Returns(new List<Booking>()); // No existing bookings
            Booking booking = new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(5) };

            BookingManager manager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);

            // Act
            bool bookingCreated = manager.CreateBooking(booking);

            // Assert
            Assert.True(bookingCreated);
            Assert.Equal(availableRoomId, booking.RoomId);
            Assert.True(booking.IsActive);
            mockBookingRepository.Verify(r => r.Add(booking), Times.Once);
        }
        [Fact]
        public void CreateBooking_NoAvailableRoom_ReturnsFalse()
        {
            // Arrange
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockRoomRepository = new Mock<IRepository<Room>>();
            mockRoomRepository.Setup(r => r.GetAll()).Returns(GetRooms(1)); // 1 room
            mockBookingRepository.Setup(r => r.GetAll()).Returns(GetBookings(new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(5) })); // Existing booking for the same period

            Booking booking = new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(5) };

            BookingManager manager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);

            // Act
            bool bookingCreated = manager.CreateBooking(booking);

            // Assert
            Assert.False(bookingCreated);
            mockBookingRepository.Verify(r => r.Add(booking), Times.Never);
        }

        public IEnumerable<Room> GetRooms(int noOfRooms)
        {
            List<Room> rooms = new List<Room>();
            for (int i = 0; i < noOfRooms; i++)
            {
                rooms.Add(new Room { Id = i + 1 }); // Assign unique IDs
            }
            return rooms;
        }

        private List<Booking> GetBookings(Booking booking)
        {
            return new List<Booking>() { booking };
        }

    }
}
