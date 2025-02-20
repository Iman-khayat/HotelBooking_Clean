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
<<<<<<< HEAD
            
=======

>>>>>>> 8bc300c804747871c1cffc00ab7acbfc8f71e9d6
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);

            var bookingForReturnedRoomId = bookingRepository.GetAll().Where(
                b => b.RoomId == roomId
                && b.StartDate <= date
                && b.EndDate >= date
                && b.IsActive);
<<<<<<< HEAD
            
=======

>>>>>>> 8bc300c804747871c1cffc00ab7acbfc8f71e9d6
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

<<<<<<< HEAD
            // Setup RoomRepository mock based on roomId
            var rooms = new List<Room>();
            if (roomId > 0)
            {
                rooms.Add(new Room { Id = roomId }); // Simulate room availability
                mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);
            }
            else
            {
                mockRoomRepository.Setup(r => r.GetAll()).Returns(new List<Room>()); // No rooms
            }

            var bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
            var booking = new Booking { StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(2) };

            // Act
            bool result = bookingManager.CreateBooking(booking);

            // Assert
            Assert.Equal(expectedResult, result);

            if (expectedResult)
            {
                Assert.Equal(roomId, booking.RoomId);
                Assert.True(booking.IsActive);
                mockBookingRepository.Verify(repo => repo.Add(booking), Times.Once);
            }
            else
            {
                Assert.Equal(0, booking.RoomId); // Or default value
                Assert.False(booking.IsActive);
                mockBookingRepository.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Never);
            }
        }
        
        [Theory]
        [InlineData(1, 0)] // No bookings
        [InlineData(1, 1)] // One booking, room available
        [InlineData(1, 2)] // Two bookings, one room
        public void FindAvailableRoom_DataDriven_ReturnsExpectedRoomId(int totalRooms, int activeBookings)
=======

        [Fact]
        public void CreateBooking_AvailableRoom_BookingCreated()
>>>>>>> 8bc300c804747871c1cffc00ab7acbfc8f71e9d6
        {
            // Arrange
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockRoomRepository = new Mock<IRepository<Room>>();

<<<<<<< HEAD
            var rooms = Enumerable.Range(1, totalRooms).Select(i => new Room { Id = i }).ToList();
            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);

            var bookings = new List<Booking>();
            if (activeBookings > 0)
            {
                for (int i = 0; i < activeBookings; i++)
                {
                    bookings.Add(new Booking { RoomId = i + 1, StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(2), IsActive = true });
                }
            }
            mockBookingRepository.Setup(repo => repo.GetAll()).Returns(bookings);

            var bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(2);

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            if (totalRooms == 0 || activeBookings >= totalRooms) {
                Assert.Equal(-1, roomId); // No room available
            } else {
                Assert.Equal(1, roomId); // Room 1 should be available if we have at least one room and it is not fully booked
            }
        }
    [Theory]
    [InlineData(1, 1, 1, "2024-01-01", "2024-01-03", "2024-01-02")] // One room, one booking, one occupied date
    [InlineData(2, 1, 1, "2024-01-01", "2024-01-03", "")]        // Two rooms, one booking, no occupied dates
    [InlineData(1, 2, 1, "2024-01-01", "2024-01-03", "2024-01-02")] // One room, two bookings, one occupied date
    [InlineData(1, 1, 0, "2024-01-01", "2024-01-03", "")]        // One room, one booking, no rooms, no occupied dates
    public void GetFullyOccupiedDates_DataDriven_ReturnsExpectedDates(
        int totalRooms, int totalBookings, int activeBookings, string startDateStr, string endDateStr, string expectedOccupiedDatesStr)
    {
        // Arrange
        var mockBookingRepository = new Mock<IRepository<Booking>>();
        var mockRoomRepository = new Mock<IRepository<Room>>();

        var rooms = Enumerable.Range(1, totalRooms).Select(i => new Room { Id = i }).ToList();
        mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);

        var bookings = new List<Booking>();
        if (totalBookings > 0)
        {
            for (int i = 0; i < totalBookings; i++)
            {
                bookings.Add(new Booking { RoomId = i + 1, StartDate = DateTime.Parse("2024-01-02"), EndDate = DateTime.Parse("2024-01-02"), IsActive = (i < activeBookings) }); // All bookings on the same day for simplicity
            }
        }
        mockBookingRepository.Setup(repo => repo.GetAll()).Returns(bookings);

        var bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
        var startDate = DateTime.Parse(startDateStr);
        var endDate = DateTime.Parse(endDateStr);

        // Act
        var occupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

        // Assert
        var expectedOccupiedDates = string.IsNullOrEmpty(expectedOccupiedDatesStr)
            ? new List<DateTime>()
            : expectedOccupiedDatesStr.Split(',').Select(DateTime.Parse).ToList();

        Assert.Equal(expectedOccupiedDates.Count, occupiedDates.Count);
        foreach (var date in expectedOccupiedDates)
        {
            Assert.Contains(date, occupiedDates);
        }
    }
=======

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

>>>>>>> 8bc300c804747871c1cffc00ab7acbfc8f71e9d6
    }
}
