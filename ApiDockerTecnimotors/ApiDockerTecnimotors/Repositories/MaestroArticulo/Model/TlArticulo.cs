namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Model
{
    public class SearchData
    {
        public List<LstmodelAro>? Listaro { get; set; }
        public List<LstmodelCodada>? Listcocada { get; set; }
        public List<LstmodelMarca>? Listmarca { get; set; }
        public List<LstmodelTipoUso>? LisTtipouso { get; set; }
        public List<TlArticulo>? ListArticulo { get; set; }
    }

    public class TrFrombody
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

    public class TrFromGeneral
    {
        public int Id { get; set; }
        public string? Anchoperfil { get; set; }
    }

    public class TlDetalleArticulo
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Unidad_medida { get; set; }
        public string? Marca { get; set; }
        public string? Abreviado { get; set; }
        public string? Codigo_equivalente { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Cocada { get; set; }
        public string? Ancho { get; set; }
        public string? Perfil { get; set; }
        public string? Aro { get; set; }
        public string? Tipouso { get; set; }
        public string? Estado { get; set; }
    }
}
