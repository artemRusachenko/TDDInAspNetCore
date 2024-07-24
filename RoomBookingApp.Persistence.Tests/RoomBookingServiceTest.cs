using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {

/*        public RoomBookingServiceTest() 
        { 
        
        }*/

        [Fact]
        public void Should_Return_Available_Rooms()
        {
            //Arrange
            var date = new DateTime(2021, 06, 09);

            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            context.Rooms.Add(new Room { Id = 1, Name = "Room 1" });
            context.Rooms.Add(new Room { Id = 2, Name = "Room 2" });
            context.Rooms.Add(new Room { Id = 3, Name = "Room 3" });

            context.RoomBookings.Add(new RoomBooking { RoomId = 1, Date = date });
            context.RoomBookings.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            //Act
            var availableRooms = roomBookingService.GetAvalibleRooms(date);

            //Asert
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, q => q.Id == 2);
            Assert.Contains(availableRooms, q => q.Id == 3);
            Assert.DoesNotContain(availableRooms, q => q.Id == 1);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2021, 06, 09) };

            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);

            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
        }
    }
}
