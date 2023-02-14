using Dapper;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;

namespace HyperativaDesafio.Infra.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {

        public Usuario GetUsuario(string login, string senha)
        {
            var usuarioLocalizado = DbContext.Connection
                .Query<Usuario>($"Select * from usuario where login = '{login}' and senha = '{senha}'");

            return usuarioLocalizado.FirstOrDefault() ?? new Usuario() { id = 0, nome = "", senha = "" };

        }
    }
}
