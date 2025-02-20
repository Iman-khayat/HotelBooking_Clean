using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Xunit;

namespace HotelBooking.UnitTests.Fakes
{
    public class FakeBookingRepository : IRepository<Booking>
    {
        private DateTime fullyOccupiedStartDate;
        private DateTime fullyOccupiedEndDate;

        public FakeBookingRepository(DateTime start, DateTime end)
        {
            fullyOccupiedStartDate = start;
            fullyOccupiedEndDate = end;
        }
        [Fact]
        public void Add_AddsBookingAndSetsCallFlag()
        {
            // Arrange
            FakeBookingRepository repo = new FakeBookingRepository(DateTime.Today, DateTime.Today.AddDays(1));
            Booking booking = new Booking();

            // Act
            repo.Add(booking);

            // Assert
            Assert.True(repo.addWasCalled);
        }

        // This field is exposed so that a unit test can validate that the
        // Add method was invoked.
        public bool addWasCalled = false;

        public void Add(Booking entity)
        {
            addWasCalled = true;
        }

        // This field is exposed so that a unit test can validate that the
        // Edit method was invoked.
        public bool editWasCalled = false;

        public void Edit(Booking entity)
        {
            editWasCalled = true;
        }
        [Fact]
        public void Edit_EditsBookingAndSetsCallFlag()
        {
            // Arrange
            FakeBookingRepository repo = new FakeBookingRepository(DateTime.Today, DateTime.Today.AddDays(1));
            Booking booking = new Booking();

            // Act
            repo.Edit(booking);

            // Assert
            Assert.True(repo.editWasCalled);
        }

        public Booking Get(int id)
        {
            return new Booking { Id = 1, StartDate = fullyOccupiedStartDate, EndDate = fullyOccupiedEndDate, IsActive = true, CustomerId = 1, RoomId = 1 };
        }

        [Fact]
        public void Get_ReturnsBookingWithExpectedValues()
        {
            // Arrange
            DateTime expectedStartDate = DateTime.Today.AddDays(5);
            DateTime expectedEndDate = DateTime.Today.AddDays(10);
            FakeBookingRepository repo = new FakeBookingRepository(expectedStartDate, expectedEndDate);

            // Act
            Booking booking = repo.Get(1);

            // Assert
            Assert.Equal(expectedStartDate, booking.StartDate);
            Assert.Equal(expectedEndDate, booking.EndDate);
            Assert.True(booking.IsActive);
            Assert.Equal(1, booking.CustomerId);
            Assert.Equal(1, booking.RoomId);
        }
        [Fact]
        public void GetAll_ReturnsMultipleBookingsIncludingSpecificIds()
        {
            // Arrange
            FakeBookingRepository repo = new FakeBookingRepository(DateTime.Today, DateTime.Today.AddDays(1));

            // Act
            IEnumerable<Booking> bookings = repo.GetAll();

            // Assert
            Assert.Equal(3, bookings.Count());
            Assert.Contains(new Booking { Id = 1 }, bookings);
            Assert.Contains(new Booking { Id = 2 }, bookings);
        }

        public IEnumerable<Booking> GetAll()
        {
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Today.AddDays(1), EndDate=DateTime.Today.AddDays(1), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=1, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 },
            };
            return bookings;
        }

        // This field is exposed so that a unit test can validate that the
        // Remove method was invoked.
        public bool removeWasCalled = false;

        public void Remove(int id)
        {
            removeWasCalled = true;
        }
        [Fact]
        public void Remove_RemovesBookingAndSetsCallFlag()
        {
            // Arrange
            FakeBookingRepository repo = new FakeBookingRepository(DateTime.Today, DateTime.Today.AddDays(1));

            // Act
            repo.Remove(1);

            // Assert
            Assert.True(repo.removeWasCalled);
        }
    }
}
