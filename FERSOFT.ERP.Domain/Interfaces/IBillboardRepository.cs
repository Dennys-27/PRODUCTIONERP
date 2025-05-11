using FERSOFT.ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Domain.Interfaces
{
    public interface IBillboardRepository : IRepositoryGeneric<BillboardEntity> 
    {
        Task<BillboardEntity?> GetBillboardWithDetailsAsync(int billboardId);
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
