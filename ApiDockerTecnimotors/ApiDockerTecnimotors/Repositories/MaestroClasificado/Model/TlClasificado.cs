namespace ApiDockerTecnimotors.Repositories.MaestroClasificado.Model
{
    public class SearchDataLlanta
    {
        public List<LstmodelAro>? Listaro { get; set; }
        public List<LstmodelCodada>? Listcocada { get; set; }
        public List<LstmodelMarca>? Listmarca { get; set; }
        public List<LstmodelTipoUso>? LisTtipouso { get; set; }
        public List<TlArticulo>? ListArticulo { get; set; }
    }
    public class TrFrombodyLlanta
    {
        public string? Ancho { get; set; }
        public string? Perfil { get; set; }
        public string? Aro { get; set; }
        public string? Cocada { get; set; }
        public string? Marca { get; set; }
        public string? TipoUso { get; set; }
    }
    public class TlArticulo
    {
        public int Id { get; set; }
        public string? Marca { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Cocada { get; set; }
        public string? Ancho { get; set; }
        public string? Perfil { get; set; }
        public string? Aro { get; set; }
        public string? Tipouso { get; set; }
        public string? Estado { get; set; }
    }
    public class LstmodelAro
    {
        public string? Aro { get; set; }
    }

    public class LstmodelCodada
    {
        public string? Cocada { get; set; }
    }

    public class LstmodelMarca
    {
        public string? Marcaoriginal { get; set; }
    }

    public class LstmodelTipoUso
    {
        public string? Tipouso { get; set; }
    }
    public class TlLlanta
    {
        public string? Anchoperfil { get; set; }
    }
    public class TlClasificado
    {
        public string? Aplicacion { get; set; }
    }
    public class TLcodigo
    {
        public string? Codigo { get; set; }
    }
    public class TrAceite
    {
        public string? TipoMarca { get; set; }
    }
    public class Trcatrepuesto
    {
        public string? Categoria { get; set; }
    }
    public class Trmarcarepuesto
    {
        public string? Marca { get; set; }
    }
    public class TlRepuesto
    {
        public string? Tipo_articulo { get; set; }
        public string? Descripcion_tipo_articulo { get; set; }
    }
    public class TlMaestroGeneral
    {
        public string? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Unidadmedida { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Tipo { get; set; }
        public string? Familia { get; set; }
        public string? Subfamilia { get; set; }
        public string? Abreviado { get; set; }
        public string? Codigoequivalente { get; set; }
        public string? Aplicacion { get; set; }
        public string? Categoria { get; set; }
        public string? Marca { get; set; }
        public string? Producto { get; set; }
        public string? Medida { get; set; }
        public string? Medidaestandarizado { get; set; }
        public string? Modelo { get; set; }
        public string? Tipo1 { get; set; }
        public string? Tipo2 { get; set; }
        public string? Tipo3 { get; set; }
        public string? Tipo4 { get; set; }
        public string? Tipo5 { get; set; }
        public string? Tipo6 { get; set; }
        public string? Tipo7 { get; set; }
        public string? Gironegocio { get; set; }
        public string? Equivalencia { get; set; }
        public string? Fabricante { get; set; }
        public string? Categoriageneral { get; set; }
        public string? Clasificacionproveedor { get; set; }
        public string? Estado { get; set; }
        public string? Pathimagen { get; set; }
    }

    public class TlMaestroModelo
    {
        public string? Marcaoriginal { get; set; }
    }
    public class TlMaestroMarca
    {
        public string? Marca { get; set; }
    }
}
