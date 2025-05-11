using FERSOFT.ERP.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class BookingEntity : BaseEntity
{
    [Required]
    public DateTime Date { get; set; }  // Esta es la propiedad correcta para las fechas

    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; }

    public int SeatId { get; set; }
    public SeatEntity Seat { get; set; }

    public int BillboardId { get; set; }
    public BillboardEntity Billboard { get; set; }

    public int MovieId { get; set; }
    public MovieEntity Movie { get; set; }

    public int ClientId { get; set; }
    public CustomerEntity Client { get; set; }
}
