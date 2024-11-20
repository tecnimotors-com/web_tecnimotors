using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;

namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface
{
    public interface IMaestroArticuloRepository
    {
        public Task<IEnumerable<TlArticulo>> GetArticulosAsync();
        public Task<IEnumerable<TlMarca>> GetMarcaAsync();
        public Task<IEnumerable<TlArticulo>> GetAllSinFiltroArticulo(string value);
        public Task<IEnumerable<TlArticulo>> GetAllFiltroMarcaCocada(string value, string cocada);
    }
}
