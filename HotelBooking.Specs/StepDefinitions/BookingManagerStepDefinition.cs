using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Moq;
using Reqnroll;
using Xunit;

namespace HotelBooking.Specs.StepDefinitions;

[Binding]
public class BookingManagerSteps
{
    private BookingManager bookingManager;
    private bool bookingResult;
    private Exception thrownException;
    private Mock<IRepository<Room>> mockRoomRepo;
    private Mock<IRepository<Booking>> mockBookingRepo;

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

    [When(@"I try to create a booking from ""(.*)"" to ""(.*)""")]
    public void WhenITryToCreateABookingFromTo(string startDate, string endDate)
    {
        try
        {
            bookingResult = bookingManager.CreateBooking(new Booking
            {
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
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

    [Then(@"the booking should fail with an invalid start date error")]
    public void ThenTheBookingShouldFailWithAnInvalidStartDateError()
    {
        Assert.IsType<ArgumentException>(thrownException);
    }

    [Then(@"the booking should fail with an invalid date range error")]
    public void ThenTheBookingShouldFailWithAnInvalidDateRangeError()
    {
        Assert.IsType<ArgumentException>(thrownException);
    }
}

