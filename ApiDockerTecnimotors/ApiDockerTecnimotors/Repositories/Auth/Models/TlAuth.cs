namespace ApiDockerTecnimotors.Repositories.Auth.Models
{
    public class User
    {
        public string? Correo { get; set; }
        public string? Password { get; set; }
    }
    public class TlAuth
    {
        public string? Uuid { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Celular { get; set; }
        public string? Password { get; set; }
        public string? Repassword { get; set; }
        public bool Termaccept { get; set; }
        public string? Fecharegistro { get; set; }
        public string? Estado { get; set; }

    }
}
