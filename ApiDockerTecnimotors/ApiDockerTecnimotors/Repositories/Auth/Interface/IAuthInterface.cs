using ApiDockerTecnimotors.Repositories.Auth.Models;

namespace ApiDockerTecnimotors.Repositories.Auth.Interface
{
    public interface IAuthInterface
    {
        public Task<IEnumerable<TlAuth>> ListadoCliente();
        public Task<TlAuth> DetailCliente(string correo);
        public Task<bool> RegisterCliente(TlAuth Tlcliente);
        public Task<TlAuth> DetailClienteUuid(string uuid);
    }
}
