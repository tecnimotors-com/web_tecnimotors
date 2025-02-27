namespace ApiDockerTecnimotors.Repositories.Distribuidores.Model
{
    public class Tldistribuidores
    {
        public int Iddistribuidores { get; set; }
        public string? Nombre { get; set; }
        public string? Ruc { get; set; }
        public string? Distrito { get; set; }
        public string? Provincia { get; set; }
        public string? Departamento { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? Direccion2 { get; set; }
    }
    public class TlDetaildistri
    {
        public int Iddistribuidores { get; set; }
        public string? Direccion { get; set; }
        public string? Direccion2 { get; set; }
    }
}
