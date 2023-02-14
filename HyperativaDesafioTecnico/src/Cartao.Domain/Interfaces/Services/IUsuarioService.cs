using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.Domain.Interfaces.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario GetUsuario(string login, string senha);
    }
}
