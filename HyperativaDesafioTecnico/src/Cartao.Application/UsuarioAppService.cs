using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Application
{
    public class UsuarioAppService : AppServiceBase<Usuario>, IUsuarioAppService
    {

        private readonly IUsuarioService _usuarioService;

        public UsuarioAppService(IUsuarioService usuarioService)
            : base(usuarioService)
        {
            _usuarioService = usuarioService;
        }



    }
}
