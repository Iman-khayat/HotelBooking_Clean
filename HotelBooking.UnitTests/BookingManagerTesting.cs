using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerUnitTesting
    {
        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;

        public BookingManagerUnitTesting()
        {
            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
        }



        private static List<Booking> GetBookings(Booking booking)
        {
            return new List<Booking>() { new Booking {
        StartDate = booking.StartDate,
        EndDate = booking.EndDate,
        IsActive = true // Assuming conflicting booking is active 
    } };
        }


        public static IEnumerable<object[]> CreateBookingTestData => new List<object[]>
{
    // Test case 1: Booking created successfully (available room)
    new object[] { DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 2, true }, // Start, End, Rooms, Expected Result

    // Test case 2: Booking creation fails (no available room)
    new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2, false },

    // Test case 3: Booking creation fails (invalid booking data - StartDate after EndDate)
    new object[] { DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), 2, "ArgumentException" }, // Throws ArgumentException
};


        [Theory]
        [MemberData(nameof(CreateBookingTestData))]
        public void CreateBooking_TestDataDriven(DateTime startDate, DateTime endDate, int noOfRooms, object expectedResult)
        {
            // Arrange
            mockRoomRepository.Setup(r => r.GetAll()).Returns(GetRooms(noOfRooms));
            Booking booking = new Booking { StartDate = startDate, EndDate = endDate };

            BookingManager manager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);

            // Act
            if (expectedResult is bool) // Successful booking
            {
                bool bookingCreated = manager.CreateBooking(booking);
                Assert.Equal(expectedResult, bookingCreated);
            }
            else // Expecting ArgumentException
            {
                Assert.Throws<ArgumentException>(() => manager.CreateBooking(booking));
            }
        }

        private IEnumerable<Room> GetRooms(int noOfRooms)
        {
            List<Room> rooms = new List<Room>();
            for (int i = 0; i < noOfRooms; i++)
            {
                rooms.Add(new Room { Id = i + 1 }); // Assign unique IDs
            }
            return rooms;
        }
    }



}
