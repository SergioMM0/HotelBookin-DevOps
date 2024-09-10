using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private readonly BookingManager bookingManager;
        private readonly Mock<IRepository<Booking>> mockBookingRepository;
        private readonly Mock<IRepository<Room>> mockRoomRepository;

        public BookingManagerTests()
        {
            // Setup the mocks
            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomRepository = new Mock<IRepository<Room>>();
            bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomRepository.Object);
        }

        #region FindAvailableRoom Tests

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(1);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(startDate, endDate));
            Assert.Equal("The start date cannot be in the past or later than the end date.", ex.Message);
        }

        [Fact]
        public void FindAvailableRoom_StartDateAfterEndDate_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(5);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(startDate, endDate));
            Assert.Equal("The start date cannot be in the past or later than the end date.", ex.Message);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_ReturnsValidRoomId()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(12);

            // Mock room repository
            var rooms = new List<Room> { new() { Id = 1 }, new() { Id = 2 } };
            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);

            // Mock booking repository (no conflicting bookings)
            var bookings = new List<Booking>();
            mockBookingRepository.Setup(b => b.GetAll()).Returns(bookings);

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.NotEqual(-1, roomId);
            Assert.Contains(roomId, rooms.Select(r => r.Id));
        }

        [Fact]
        public void FindAvailableRoom_NoRoomAvailable_ReturnsMinusOne()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(12);

            // Mock room repository
            var rooms = new List<Room> { new Room { Id = 1 }, new Room { Id = 2 } };
            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);

            // Mock booking repository (all rooms are booked)
            var bookings = new List<Booking>
            {
                new () { RoomId = 1, StartDate = startDate, EndDate = endDate, IsActive = true },
                new () { RoomId = 2, StartDate = startDate, EndDate = endDate, IsActive = true }
            };
            mockBookingRepository.Setup(b => b.GetAll()).Returns(bookings);

            // Act
            int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Equal(-1, roomId);
        }

        #endregion

        #region CreateBooking Tests

        [Fact]
        public void CreateBooking_RoomAvailable_BookingCreatedSuccessfully()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(12);

            var booking = new Booking { StartDate = startDate, EndDate = endDate };

            // Mock room and booking repositories
            var rooms = new List<Room> { new () { Id = 1 } };
            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);
            mockBookingRepository.Setup(b => b.GetAll()).Returns(new List<Booking>());

            // Act
            var result = bookingManager.CreateBooking(booking);

            // Assert
            Assert.True(result);
            mockBookingRepository.Verify(b => b.Add(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public void CreateBooking_NoRoomAvailable_BookingNotCreated()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(12);

            var booking = new Booking { StartDate = startDate, EndDate = endDate };

            // Mock room and booking repositories
            var rooms = new List<Room> { new () { Id = 1 } };
            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);

            // All rooms booked
            var bookings = new List<Booking> 
            { 
                new () { RoomId = 1, StartDate = startDate, EndDate = endDate, IsActive = true }
            };
            mockBookingRepository.Setup(b => b.GetAll()).Returns(bookings);

            // Act
            var result = bookingManager.CreateBooking(booking);

            // Assert
            Assert.False(result);
            mockBookingRepository.Verify(b => b.Add(It.IsAny<Booking>()), Times.Never);
        }

        #endregion

        #region GetFullyOccupiedDates Tests

        [Theory]
        [InlineData("2024-09-01", "2024-09-10", 10)]
        [InlineData("2024-09-11", "2024-09-15", 0)]
        public void GetFullyOccupiedDates_MultipleBookings_ReturnsCorrectOccupiedDates(string start, string end, int expectedOccupiedCount)
        {
            // Arrange
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end);

            var bookings = new List<Booking>
            {
                new () { RoomId = 1, StartDate = DateTime.Parse("2024-09-01"), EndDate = DateTime.Parse("2024-09-10"), IsActive = true },
                new () { RoomId = 2, StartDate = DateTime.Parse("2024-09-01"), EndDate = DateTime.Parse("2024-09-10"), IsActive = true }
            };

            // Mock repositories
            mockBookingRepository.Setup(b => b.GetAll()).Returns(bookings);
            mockRoomRepository.Setup(r => r.GetAll()).Returns(new List<Room> { new () { Id = 1 }, new () { Id = 2 } });

            // Act
            var fullyOccupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Equal(expectedOccupiedCount, fullyOccupiedDates.Count);
        }
        
        [Fact]
        public void GetFullyOccupiedDates_StartDateAfterEndDate_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(5);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(startDate, endDate));
            Assert.Equal("The start date cannot be later than the end date.", ex.Message);
        }

        #endregion
    }
}
