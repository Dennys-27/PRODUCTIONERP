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
        // Crea una nueva cartelera con los datos enviados
        Task<BillboardDto> CreateBillboardAsync(BillboardDto billboardDto);

        // Devuelve todas las carteleras registradas
        Task<IEnumerable<BillboardDto>> GetAllBillboardsAsync();

        // Cancela una cartelera usando su ID
        Task CancelBillboardAsync(int billboardId);

    }
}
