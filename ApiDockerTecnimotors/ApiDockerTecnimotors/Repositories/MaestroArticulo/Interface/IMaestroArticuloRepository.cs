using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;

namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface
{
    public interface IMaestroArticuloRepository
    {
        public Task<IEnumerable<TrFromGeneral>> ListArticulosGeneral();

        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public Task<IEnumerable<LstmodelAro>> AllListadoCocadaAro(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelCodada>> AllListadoCocadaCocada(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelMarca>> AllListadoCocadaMarca(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelTipoUso>> AllListadoCocadaTipoUso(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<TlArticulo>> AllListadoCocadaArticulo(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);

        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public Task<TlDetalleArticulo> DetalleArticulo(int Id);
    }
}
