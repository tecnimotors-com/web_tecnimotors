using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.Distribuidores.Interface;
using ApiDockerTecnimotors.Repositories.Distribuidores.Model;
using Dapper;
using Npgsql;

namespace ApiDockerTecnimotors.Repositories.Distribuidores.Repo
{
    public class DistribuidoresRepository(PostgreSQLConfiguration connectionString) : IDistribuidoresRepository
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;

        private NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<Tldistribuidores>> ListadoDistribuidores()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT iddistribuidores, TRIM(nombre) AS nombre, TRIM(ruc) AS ruc, TRIM(distrito) AS distrito, 
                        TRIM(provincia) AS provincia, TRIM(departamento) AS departamento, TRIM(telefono) AS telefono, 
                        TRIM(correo) AS correo, TRIM(direccion) AS direccion, REPLACE(TRIM(direccion), ' ', '%20') AS direccion2
                        FROM public.distribuidores limit 100;
                       ";
            return await db.QueryAsync<Tldistribuidores>(sql, new { });
        }

        public async Task<Tldistribuidores> DetailDistribuidores(int idDistribuidores)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT iddistribuidores, TRIM(nombre) AS nombre, TRIM(ruc) AS ruc, TRIM(distrito) AS distrito, 
                        TRIM(provincia) AS provincia, TRIM(departamento) AS departamento, TRIM(telefono) AS telefono, 
                        TRIM(correo) AS correo, TRIM(direccion) AS direccion, REPLACE(TRIM(direccion), ' ', '%20') AS direccion2
                        FROM public.distribuidores 
                        WHERE iddistribuidores = @Iddistribuidores
              ";

            var result = await db.QuerySingleOrDefaultAsync<Tldistribuidores>(sql, new { Iddistribuidores = idDistribuidores });

            return result!;
        }

        public async Task<IEnumerable<Tldistribuidores>> ListadoDetalleDistribuidore(string Depa, string Provin, string Distri)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT iddistribuidores, TRIM(nombre) AS nombre, TRIM(ruc) AS ruc,
                        TRIM(distrito) AS distrito, 
                        TRIM(provincia) AS provincia,
                        TRIM(departamento) AS departamento, TRIM(telefono) AS telefono, TRIM(correo) AS correo,
                        TRIM(direccion) AS direccion, REPLACE(TRIM(direccion), ' ', '%20') AS direccion2
                        FROM public.distribuidores where 
                        Upper(trim(departamento)) = UPPER(TRIM('" + Depa + @"')) AND 
                        UPPER(TRIM(provincia))= UPPER(TRIM('" + Provin + @"')) AND
                        UPPER(TRIM(distrito))= UPPER(TRIM('" + Distri + @"')) limit 100
                       ";

            return await db.QueryAsync<Tldistribuidores>(sql, new { });
        }


    }
}
