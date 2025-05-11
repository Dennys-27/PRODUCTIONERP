using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Domain.Interfaces;
using FERSOFT.ERP.Infrastructure.Data;
using FERSOFT.ERP.Infrastructure.Repositorios.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Infrastructure.Repositorios.Cinema
{
    public class BillboardRepository : RepositoryGeneric<BillboardEntity>, IBillboardRepository
    {
        private readonly ApplicationDbContext _context;
        public BillboardRepository(ApplicationDbContext context)  : base(context) 
        {
            _context = context;
        }
        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await operation();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<BillboardEntity> GetBillboardWithDetailsAsync(int billboardId)
        {
            return await _context.Billboards
                .Include(b => b.Room)
                .ThenInclude(r => r.Seats)
                .Include(b => b.Bookings)
                .ThenInclude(bk => bk.Client)
                .FirstOrDefaultAsync(b => b.Id == billboardId);
        }
    }
}
