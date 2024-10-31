using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Moq;
using Reqnroll;
using Xunit;

namespace HotelBooking.Specs.StepDefinitions
{
    [Binding]
    public class BookingManagerSteps
    {
        private BookingManager bookingManager;
        private bool bookingResult;
        private Exception thrownException;
        private Mock<IRepository<Room>> mockRoomRepo;
        private Mock<IRepository<Booking>> mockBookingRepo;
        
        private DateTime occupiedStartDate;
        private DateTime occupiedEndDate;
        
        private DateTime startDate;
        private DateTime endDate;

        [Given(@"the hotel has (.*) rooms available")]
        public void GivenTheHotelHasRoomsAvailable(int rooms)
        {
            // Initialize mock objects
            mockRoomRepo = new Mock<IRepository<Room>>();
            mockBookingRepo = new Mock<IRepository<Booking>>();

            // Mock room repository to return a list of rooms
            mockRoomRepo.Setup(repo => repo.GetAll()).Returns(
                Enumerable.Range(1, rooms).Select(id => new Room { Id = id }).ToList()
            );

            // Mock booking repository to return an empty list of bookings initially
            mockBookingRepo.Setup(repo => repo.GetAll()).Returns(new List<Booking>());

            // Create the BookingManager with mocked dependencies
            bookingManager = new BookingManager(mockBookingRepo.Object, mockRoomRepo.Object);
        }

        [Given(@"there is an occupied range of dates in the booking system from ""(.*)"" to ""(.*)""")]
        public void GivenThereIsAnOccupiedRangeOfDatesInTheBookingSystemFromTo(string startDate, string endDate)
        {
            var today = DateTime.Today;
            occupiedStartDate = ParseRelativeDate(startDate, today);
            occupiedEndDate = ParseRelativeDate(endDate, today);

            var occupiedBooking = new Booking
            {
                StartDate = occupiedStartDate,
                EndDate = occupiedEndDate,
                IsActive = true,
                RoomId = 1 // Assuming only one room is occupied for simplicity
            };

            // Set up mock to return the occupied booking
            mockBookingRepo.Setup(repo => repo.GetAll()).Returns(new List<Booking> { occupiedBooking });
        }
        
        [Given(@"the booking start date is ""(.*)""")]
        public void GivenTheBookingStartDateIs(string today)
        {
            startDate = ParseRelativeDate(today, DateTime.Today);
        }
        
        [Given(@"the booking end date is ""(.*)""")]
        public void GivenTheBookingEndDateIs(string tomorrow)
        {
            endDate = ParseRelativeDate(tomorrow, DateTime.Today);
        }

        // Adjusting the When step to match the scenario definition
        [When(@"I try to create a booking")]
        public void WhenITryToCreateABookingWithStartDateAndEndDateBeingTheSame()
        {
            try
            {
                bookingResult = bookingManager.CreateBooking(new Booking
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    IsActive = true
                });
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
        }

        [Then(@"the booking should be created successfully")]
        public void ThenTheBookingShouldBeCreatedSuccessfully()
        {
            Assert.True(bookingResult);
        }

        [Then(@"the booking should fail to be created")]
        public void ThenTheBookingShouldFailToBeCreated()
        {
            Assert.False(bookingResult);
        }
        
        // Helper method to parse relative dates
        private DateTime ParseRelativeDate(string relativeDate, DateTime referenceDate)
        {
            return relativeDate switch
            {
                "yesterday" => referenceDate.AddDays(-1),
                "today" => referenceDate,
                "tomorrow" => referenceDate.AddDays(1),
                "the end of time" => DateTime.MaxValue,
                // Days from now
                var s when s.EndsWith("days from now") => referenceDate.AddDays(int.Parse(s.Split(' ')[0])),
                // Day(s) after the start date
                var s when s.EndsWith("days after the start date") => startDate.AddDays(int.Parse(s.Split(' ')[0])),
                var s when s.EndsWith("day after the start date") => startDate.AddDays(int.Parse(s.Split(' ')[0])), 
                // Day(s) before the occupied range
                var s when s.EndsWith("days before the occupied range") => occupiedStartDate.AddDays(-int.Parse(s.Split(' ')[0])),
                var s when s.EndsWith("day before the occupied range") => occupiedStartDate.AddDays(-int.Parse(s.Split(' ')[0])),
                // Day(s) after the occupied range
                var s when s.EndsWith("days after the occupied range") => occupiedEndDate.AddDays(int.Parse(s.Split(' ')[0])),
                var s when s.EndsWith("day after the occupied range") => occupiedEndDate.AddDays(int.Parse(s.Split(' ')[0])),
                // Day(s) before the end of time
                var s when s.EndsWith("days before the end of time") => DateTime.MaxValue.AddDays(-int.Parse(s.Split(' ')[0])),
                var s when s.EndsWith("day before the end of time") => DateTime.MaxValue.AddDays(-int.Parse(s.Split(' ')[0])),
                // Day(s) after the end of time
                var s when s.EndsWith("days after the end of time") => DateTime.MaxValue.AddDays(int.Parse(s.Split(' ')[0])),
                _ => throw new ArgumentException("Unknown date format")
            };
        }
    }
}
