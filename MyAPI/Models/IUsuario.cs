using System.Collections.Generic;

namespace MyAPI.Models
{
    public interface IUsuario
    {
        IEnumerable<Usuario> ObterUsuarios();
        int IncluirUsuario(Usuario usuario);
        int AtualizarUsuario(Usuario usuario);
        Usuario ObterUsuarioPorId(int id);
        int ExcluirUsuario(int id);
    }
}
