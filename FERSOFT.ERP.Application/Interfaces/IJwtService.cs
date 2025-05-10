using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FERSOFT.ERP.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userName, IList<string> roles);
    }
}
