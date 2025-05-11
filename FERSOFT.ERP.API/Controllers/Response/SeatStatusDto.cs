namespace FERSOFT.ERP.API.Controllers.Response
{
    public class SeatStatusDto
    {
        public string SeatNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string RoomName { get; set; }
    }
}
