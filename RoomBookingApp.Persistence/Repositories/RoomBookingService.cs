using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext _context;
        public RoomBookingService(RoomBookingAppDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<Room> GetAvalibleRooms(DateTime date)
        {
            return _context.Rooms.Where(q => !q.RoomBookings.Any(x => x.Date == date)).ToList();
        }

        public void Save(RoomBooking roomBooking)
        {
            _context.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
