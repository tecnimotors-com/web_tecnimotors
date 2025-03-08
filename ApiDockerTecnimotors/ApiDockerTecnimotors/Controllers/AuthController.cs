using ApiDockerTecnimotors.Repositories.Auth.Interface;
using ApiDockerTecnimotors.Repositories.Auth.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthInterface IAuthinterface) : ControllerBase
    {
        private readonly IAuthInterface iathInterface = IAuthinterface;

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] TlAuth user)
        {
            // Verificar si el correo ya existe
            var existingUser = await iathInterface.DetailCliente(user.Correo!);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Correo ya existe" });
            }

            // Verificar que las contraseñas coincidan
            if (user.Password != user.Repassword)
            {
                return BadRequest(new { message = "Las contraseñas no coinciden" });
            }

            // Generar un UUID para el usuario
            user.Uuid = Guid.NewGuid().ToString();

            // Hash de la contraseña
            user.Password = HashPassword(user.Password!);
            user.Repassword = HashPassword(user.Repassword!);

            // Llamar al método para registrar el nuevo cliente
            var success = await iathInterface.RegisterCliente(user); // Asegúrate de usar await aquí
            if (success)
            {
                return Ok(new { message = "Usuario registrado exitosamente." });
            }
            else
            {
                return StatusCode(500, new { message = "Error al registrar el usuario." });
            }
        }

        private static string HashPassword(string password)
        {
            // Convertir la contraseña a bytes
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            // Convertir los bytes a un string hexadecimal
            StringBuilder builder = new();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            // Verificar que el correo y la contraseña no estén vacíos
            if (string.IsNullOrEmpty(user.Correo) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { msg = "Correo y contraseña son obligatorios." });
            }

            // Obtener el usuario de la base de datos
            var existingUser = await iathInterface.DetailCliente(user.Correo);
            if (existingUser == null)
            {
                return BadRequest(new { msg = "Credenciales no válidas." });
            }

            // Hashear la contraseña ingresada
            var hashedPassword = HashPassword(user.Password);

            // Comparar la contraseña hasheada ingresada con la almacenada
            if (hashedPassword != existingUser.Password)
            {
                return BadRequest(new { msg = "Credenciales no válidas." });
            }

            // Si las credenciales son válidas, puedes devolver un token o un mensaje de éxito
            return Ok(new { msg = "Inicie sesión exitosamente.", body = existingUser });
        }

        [HttpPost("DetailClienteUuid")]
        public async Task<IActionResult> DetailClienteUuid([FromBody] TrDetailCliente trlcliente)
        {
            var result = await iathInterface.DetailClienteUuid(trlcliente.Uuid!);
            return Ok(result);
        }
        public class TrDetailCliente
        {
            public string? Uuid { get; set; }
        }
    }
}