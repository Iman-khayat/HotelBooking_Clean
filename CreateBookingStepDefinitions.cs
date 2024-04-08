using HotelBooking.Core;
using Moq;


namespace HotelBooking.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreateBookingStepDefinitions
    {
        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;

        private CreateBookingStepDefinitions()
        {
            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
        }

        // Function to setup #rooms in mockRoomRepository
        private IEnumerable<Room> GetSingleRoom()
        {
            List<Room> rooms = new List<Room>();
            rooms.Add(new Room { Id = 1 });
            return rooms;
        }

        // Function to setup existing booking in mockBookingRepository
        private static List<Booking> GetExistingBooking()
        {
            List<Booking> bookings = new List<Booking>();
            bookings.Add(new Booking { StartDate = DateTime.Today.AddDays(4), EndDate = DateTime.Today.AddDays(18), IsActive = true, RoomId = 1 });
            return bookings;
        }

        // create booking object
        private Booking booking = new Booking();

        // create boolean
        private bool bool_bookingCreated;


        [Given("John Smith are at the create booking page having selected (.*) (.*)")]
        public void GivenStartDateAndEndDate(int StartDateOffset, int EndDateOffset)
        {
            DateTime currentDate = DateTime.Today;
            DateTime StartDate = currentDate.AddDays(StartDateOffset);
            DateTime EndDate = currentDate.AddDays(EndDateOffset);

            // add dates to booking object
            booking.StartDate = StartDate;
            booking.EndDate = EndDate;
        }

        [When("John Smith press the Create button")]
        public void WhenPressCreateBooking()
        {
            mockRoomRepository.Setup(r => r.GetAll()).Returns(GetSingleRoom());
            mockBookingRepository.Setup(r => r.GetAll()).Returns(GetExistingBooking());

            BookingManager bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);

            // set value of bool_bookingCreated
            bool_bookingCreated = bookingManager.CreateBooking(booking);
        }

        [Then("his booking is (.*)")]
        public void ThenTheBookingIs(bool Created)
        {
            Assert.True(bool_bookingCreated == Created);
        }
    }
}
