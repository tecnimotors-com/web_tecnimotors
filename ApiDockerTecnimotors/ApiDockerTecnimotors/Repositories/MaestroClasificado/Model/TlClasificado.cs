namespace ApiDockerTecnimotors.Repositories.MaestroClasificado.Model
{
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
