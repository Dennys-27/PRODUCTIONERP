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
        // Obtener una cartelera con sus detalles por su ID
        Task<BillboardEntity?> GetBillboardWithDetailsAsync(int billboardId);

        // Ejecutar una operación dentro de una transacción
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
