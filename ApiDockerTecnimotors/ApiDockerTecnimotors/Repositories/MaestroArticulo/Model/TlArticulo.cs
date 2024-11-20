namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Model
{
    public class TlArticulo
    {
        public string? Marca { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Cocada { get; set; }
        public string? Ancho { get; set; }
        public string? Perfil { get; set; }
        public string? Aro { get; set; }
        public string? Tipouso { get; set; }
    }

    public class TlMarca
    {
        public int Id { get; set; }
        public string? Marca { get; set; }
    }
    public class Trfrom
    {
        public string? Value { get; set; }
    }
    public class TrFormvalue
    {
        public string? Value { get; set; }
        public string? Cocada { get; set; }
    }
}
