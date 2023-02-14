using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Services;

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

        public Usuario GetUsuario(string login, string senha)
        {
            return _usuarioService.GetUsuario(login, senha);
        }
    }
}
