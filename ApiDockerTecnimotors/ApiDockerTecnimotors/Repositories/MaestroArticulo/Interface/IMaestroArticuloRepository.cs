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

        /*------------------ tipo producto camara --------------*/
        public Task<IEnumerable<TlCategoriesCamara>> ListCategorieCamara();
        public Task<IEnumerable<TlmodeloCamara>> ListModeloCamara(string txtcategoria, string txtmarca);
        public Task<IEnumerable<TlListCamaraAll>> ListadoCamaraGeneral(string txtcategoria, string txtmarca);
        public Task<TlListCamaraAll> DetalleCamaraAll(int Id);
        public Task<IEnumerable<TlListCamaraAll>> ListadoCamaraGeneralModelo(string IdCamara);
        /*------------------ tipo producto aceite --------------*/
        public Task<IEnumerable<TlCategoriesCamara>> ListCategorieAceite();
        public Task<IEnumerable<TlmodeloCamara>> ListModeloAceite(string txtcategoria);
        public Task<IEnumerable<TlListCamaraAll>> ListadoAceiteGeneral(string txtcategoria);
        /*--------------------Vehiculo----------------------*/
        public Task<IEnumerable<TlmodeloCamara>> ListModeloVehiculo(string txtcategoria);
        public Task<IEnumerable<TlListCamaraAll>> ListadoVehiculoGeneral(string txtcategoria);
        /*--------------------Repuesto----------------------*/
        public Task<IEnumerable<TlCategoriesCamara>> ListadoRepuestoTipoCategoria(string txtcategoria);
        public Task<IEnumerable<TlmodeloCamara>> ListadoModeloRepuesto(string TipoCategoria, string Categoria);
        public Task<IEnumerable<TlListCamaraAll>> ListadoRepuestoGeneralALl(string TipoCategoria);


    }
}
