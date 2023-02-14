using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {

        Usuario GetUsuario(string login, string senha);

    }
}
