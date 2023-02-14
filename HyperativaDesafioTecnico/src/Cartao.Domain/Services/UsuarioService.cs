using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;

namespace HyperativaDesafio.Domain.Services
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
            : base(usuarioRepository)
        {
            this._usuarioRepository = usuarioRepository;
        }
        public Usuario GetUsuario(string login, string senha)
        {
            return _usuarioRepository.GetUsuario(login, senha);
        }
    }
}
