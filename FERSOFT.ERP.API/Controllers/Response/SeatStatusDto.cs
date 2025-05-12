namespace FERSOFT.ERP.API.Controllers.Response
{
    public class SeatStatusDto
    {
        public string RoomName { get; set; }
        public int AvailableSeats { get; set; }
        public int OccupiedSeats { get; set; }
    }
}
