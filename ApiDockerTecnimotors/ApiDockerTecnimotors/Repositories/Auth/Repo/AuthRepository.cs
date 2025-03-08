using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.Auth.Interface;
using ApiDockerTecnimotors.Repositories.Auth.Models;
using Dapper;
using Npgsql;

namespace ApiDockerTecnimotors.Repositories.Auth.Repo
{
    public class AuthRepository(PostgreSQLConfiguration connectionString) : IAuthInterface
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;

        private NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<TlAuth>> ListadoCliente()
        {
            var sql = @"
            SELECT uuid, nombre, apellido, correo, celular, password, repassword, termaccept, idcliente, fecharegistro, estado
            FROM public.cliente;";

            using var db = DbConnection();
            try
            {
                await db.OpenAsync();
                return await db.QueryAsync<TlAuth>(sql);
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrar el error o lanzar una excepción personalizada)
                throw new Exception("Error al obtener la lista de clientes", ex);
            }
        }

        public async Task<TlAuth> DetailCliente(string correo)
        {
            var sql = @"
            SELECT uuid, nombre, apellido, correo, celular, password, repassword, termaccept, idcliente, fecharegistro, estado
            FROM public.cliente 
            WHERE trim(correo) = trim(@Correo);";

            using var db = DbConnection();
            try
            {
                await db.OpenAsync();
                var result = await db.QueryFirstOrDefaultAsync<TlAuth>(sql, new { Correo = correo });
                return result!;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los detalles del cliente", ex);
            }
        }

        public async Task<bool> RegisterCliente(TlAuth Tlcliente)
        {
            var db = DbConnection();
            var sql = @"
                        INSERT INTO public.cliente(
	                        uuid, nombre, apellido, correo, celular, password, repassword, termaccept, fecharegistro, estado)
	                        VALUES (@Uuid, @Nombre, @Apellido, @Correo, @Celular, @Password, @Repassword, @Termaccept, @Fecharegistro, @Estado); ";

            var result = await db.ExecuteAsync(sql, new
            {
                Tlcliente.Uuid,
                Tlcliente.Nombre,
                Tlcliente.Apellido,
                Tlcliente.Correo,
                Tlcliente.Celular,
                Tlcliente.Password,
                Tlcliente.Repassword,
                Tlcliente.Termaccept,
                Tlcliente.Fecharegistro,
                Tlcliente.Estado
            });
            return result > 0;
        }
        public async Task<TlAuth> DetailClienteUuid(string uuid)
        {
            var db = DbConnection();

            var sql = @"SELECT * FROM public.cliente WHERE uuid = @Uuid";
            var result = await db.QueryFirstOrDefaultAsync<TlAuth>(sql, new { Uuid = uuid });
            return result!;

        }
    }
}
