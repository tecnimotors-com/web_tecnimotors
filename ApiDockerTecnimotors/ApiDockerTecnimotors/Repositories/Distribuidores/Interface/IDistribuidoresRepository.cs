using ApiDockerTecnimotors.Repositories.Distribuidores.Model;

namespace ApiDockerTecnimotors.Repositories.Distribuidores.Interface
{
    public interface IDistribuidoresRepository
    {
        public Task<IEnumerable<Tldistribuidores>> ListadoDistribuidores();
        public Task<Tldistribuidores> DetailDistribuidores(int idDistribuidores);
        public Task<IEnumerable<Tldistribuidores>> ListadoDetalleDistribuidore(string Depa, string Provin, string Distri);
        public Task<IEnumerable<Tldistribuidores>> ListadoGeneralDistribuidores(TlFilterDistribuidor tlfilterDistri);
    }
}
