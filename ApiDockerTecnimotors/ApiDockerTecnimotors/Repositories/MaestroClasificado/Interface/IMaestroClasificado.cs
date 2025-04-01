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

        /*-----------------------aceite-------------------------*/
        /*--------------------------------------------------------*/
        public Task<IEnumerable<Trmarcarepuesto>> TipoMarcaAceite();
        public Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralAceite(string TipoMarca);
        public Task<IEnumerable<Trcatrepuesto>> ListadoRepuestoCategoria();
        public Task<IEnumerable<Trmarcarepuesto>> ListadoRepuestoMarca(string Categoria);
        public Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralRepuesto(string Categoria, string Marca);
        public Task<IEnumerable<Trcatrepuesto>> ListadoTipoCamaras();
        public Task<IEnumerable<Trmarcarepuesto>> ListadoCamaraMarca(string Categoria);
        public Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralCamara(string Categoria, string Marca);
        public Task<IEnumerable<TlLlanta>> ListadoAnchoPerfilLLANTA();
        public Task<IEnumerable<LstmodelAro>> AllListadoCocadaAroLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelCodada>> AllListadoCocadaCocadaLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelMarca>> AllListadoCocadaMarcaLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelTipoUso>> AllListadoCocadaTipoUsoLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<TlArticulo>> AllListadoCocadaArticuloLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        /*-----------------------------------------------------------------*/
        public Task<IEnumerable<TlMedida>> ListadoLLantaMedida();
        public Task<IEnumerable<TlModelo>> ListadoLLantaModelo();
        public Task<IEnumerable<TlMarca>> ListadoLLantaMarca();
        public Task<IEnumerable<TlCategoria>> ListadoLLantaCategoria();
    }
}
