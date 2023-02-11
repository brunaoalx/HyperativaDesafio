using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Interfaces.Services
{
    internal interface IServiceBase<Tentity> where Tentity : class
    {
        void Add(Tentity entity, string query);
        void Dispose();
    }
}
