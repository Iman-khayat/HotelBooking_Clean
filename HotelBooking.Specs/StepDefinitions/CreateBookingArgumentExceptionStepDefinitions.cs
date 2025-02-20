using HotelBooking.Core;
using Moq;


namespace HotelBooking.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreateBookingArgumentExceptionStepDefinitions
    {
        private Mock<IRepository<Booking>> mockBookingRepository;
        private Mock<IRepository<Room>> mockRoomRepository;

        private CreateBookingArgumentExceptionStepDefinitions()
        {
            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
        }

        // Function to setup a single room in mockRoomRepository
        private IEnumerable<Room> GetSingleRoom()
        {
            List<Room> rooms = new List<Room>();
            rooms.Add(new Room { Id = 1 });
            return rooms;
        }

        // Function to setup an existing booking in mockBookingRepository
        private static List<Booking> GetExistingBooking()
        {
            List<Booking> bookings = new List<Booking>();
            bookings.Add(new Booking { StartDate = DateTime.Today.AddDays(4), EndDate = DateTime.Today.AddDays(18), IsActive = true, RoomId = 1 });
            return bookings;
        }

        // create booking object
        private Booking booking = new Booking();

        // create action object for when ArgumentException is thrown
        Action act;

        // create currentDate for calculating dates using StartDateOffset and EndDateOffset
        readonly DateTime currentDate = DateTime.Today;
        
        [Given(@"John Smith have selected (.*)")]
        public void GivenJohnSmithHaveSelected(int StartDateOffset)
        {
            DateTime StartDate = currentDate.AddDays(StartDateOffset);
            booking.StartDate = StartDate;
        }

        [Given(@"EndDateOffset (.*)")]
        public void GivenEndDateOffset(int EndDateOffset)
        {
            DateTime EndDate = currentDate.AddDays(EndDateOffset);
            booking.EndDate = EndDate;
        }

        [When(@"John Smith press create")]
        public void WhenJohnSmithPressCreate()
        {
            mockRoomRepository.Setup(r => r.GetAll()).Returns(GetSingleRoom());
            mockBookingRepository.Setup(r => r.GetAll()).Returns(GetExistingBooking());

            BookingManager bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
            
            act = () => bookingManager.CreateBooking(booking);
        }

        [Then(@"an ArgumentException is thrown")]
        public void ThenAnArgumentExceptionIsThrown()
        {
            act.Should().Throw<ArgumentException>();
        }
    }
}
