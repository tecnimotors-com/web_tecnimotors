using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;

namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface
{
    public interface IMaestroArticuloRepository
    {
        public Task<IEnumerable<TrFromGeneral>> GetArticulosAsync();
        public Task<IEnumerable<TlArticulo>> GetArticulosAsync(string limit, string offset);
        public Task<IEnumerable<TrFromGeneral>> GetPrincipalAsync(string limit, string offset);
        public Task<IEnumerable<TlMarca>> GetMarcaAsync();
        public Task<IEnumerable<TlArticulo>> GetAllSinFiltroArticulo(string value);
        public Task<IEnumerable<TlArticulo>> GetAllFiltroMarcaCocada(string value, string cocada);

        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfil(string Ancho, string Perfil);
        public Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfil(string Ancho, string Perfil);
        public Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfil(string Ancho, string Perfil);
        public Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfil(string Ancho, string Perfil);
        public Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfil(string Ancho, string Perfil);
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilAro(string Ancho, string Perfil, string Aro);
        public Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilAro(string Ancho, string Perfil, string Aro);
        public Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilAro(string Ancho, string Perfil, string Aro);
        public Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilAro(string Ancho, string Perfil, string Aro);
        public Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilAro(string Ancho, string Perfil, string Aro);
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilCocada(string Ancho, string Perfil, string Cocada);
        public Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilCocada(string Ancho, string Perfil, string Cocada);
        public Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilCocada(string Ancho, string Perfil, string Cocada);
        public Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilCocada(string Ancho, string Perfil, string Cocada);
        public Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilCocada(string Ancho, string Perfil, string Cocada);
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilMarca(string Ancho, string Perfil, string Marca);
        public Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilMarca(string Ancho, string Perfil, string Marca);
        public Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilMarca(string Ancho, string Perfil, string Marca);
        public Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilMarca(string Ancho, string Perfil, string Marca);
        public Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilMarca(string Ancho, string Perfil, string Marca);
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilTL(string Ancho, string Perfil, string TipoUso);
        public Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilTL(string Ancho, string Perfil, string TipoUso);
        public Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilTL(string Ancho, string Perfil, string TipoUso);
        public Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilTL(string Ancho, string Perfil, string TipoUso);
        public Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilTL(string Ancho, string Perfil, string TipoUso);








        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public Task<IEnumerable<LstmodelAro>> AllListadoCocadaAro(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelCodada>> AllListadoCocadaCocada(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelMarca>> AllListadoCocadaMarca(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<LstmodelTipoUso>> AllListadoCocadaTipoUso(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
        public Task<IEnumerable<TlArticulo>> AllListadoCocadaArticulo(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso);
    }
}
