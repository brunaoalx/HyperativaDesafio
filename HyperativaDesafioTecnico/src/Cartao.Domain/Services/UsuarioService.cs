using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Services
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
            :base(usuarioRepository)
        {
            this._usuarioRepository = usuarioRepository;
        }
        public Usuario GetUsuario(string login, string senha)
        {
            return _usuarioRepository.GetUsuario(login,senha);
        }
    }
}
