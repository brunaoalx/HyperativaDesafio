using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.Application.Interfaces
{
    public interface IUsuarioAppService : IAppServiceBase<Usuario>
    {
        Usuario GetUsuario(string login, string senha);
    }
}
