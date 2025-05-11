using FERSOFT.ERP.Application.DTOs.Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Interfaces.Cinema
{
    public interface IBillboardService
    {
        // Crea una nueva cartelera
        Task<BillboardDto> CreateBillboardAsync(BillboardDto billboardDto);

        // Devuelve todas las carteleras
        Task<IEnumerable<BillboardDto>> GetAllBillboardsAsync();

        // Devuelve una cartelera por su ID
        Task<BillboardDto> GetBillboardByIdAsync(int id);

        // Actualiza los datos de una cartelera
        Task UpdateBillboardAsync(BillboardDto billboardDto);

        // Elimina una cartelera
        Task DeleteBillboardAsync(int id);

        // Cancela una cartelera (lógica de negocio específica)
        Task CancelBillboardAsync(int billboardId);

    }
}
