using ApiDockerTecnimotors.Repositories.MaestroClasificado.Model;

namespace ApiDockerTecnimotors.Repositories.MaestroClasificado.Interface
{
    public interface IMaestroClasificado
    {
        public Task<bool> ExisteCodigoInterno(string codigoInterno);
        public Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralArticulo();
        public Task ActualizarPathImagen(string codigo, string pathImagen);
        public Task ActualizarPathImagenBatch(List<(string Codigo, string PathImagen)> actualizaciones);
        public Task<IEnumerable<TlClasificado>> ListadoCategoriaVehiculos();
        public Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralCategoria(string motocicleta);
        public Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralVehiculos(string categoria, string medida, string marca);
        public Task<IEnumerable<TlMaestroModelo>> ListadoModeloVehiculo(string categoria, string marca);
        public Task<IEnumerable<TlMaestroMarca>> ListarMarcaVehiculo(string categoria, string medida);
        public Task<TlMaestroGeneral> DetalleVehiculo(int Id);
    }
}
