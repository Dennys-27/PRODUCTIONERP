using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Interfaces
{
    public interface IBookingRepository : IRepositoryGeneric<BookingEntity>
    {
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
