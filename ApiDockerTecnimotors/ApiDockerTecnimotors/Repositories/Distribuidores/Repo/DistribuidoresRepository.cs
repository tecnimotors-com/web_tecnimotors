using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.Distribuidores.Interface;
using ApiDockerTecnimotors.Repositories.Distribuidores.Model;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Model;
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
                        SELECT 
                            iddistribuidores, 
                            TRIM(nombre) AS nombre, 
                            TRIM(ruc) AS ruc, 
                            TRIM(distrito) AS distrito, 
                            TRIM(provincia) AS provincia, 
                            TRIM(departamento) AS departamento, 
                            TRIM(telefono) AS telefono, 
                            TRIM(correo) AS correo, 
                            TRIM(direccion) AS direccion, 
                            REPLACE(TRIM(direccion), ' ', '%20') AS direccion2
                        FROM 
                            public.distribuidores 
                        WHERE 
                            trim(ruc) IS NOT NULL 
                            AND trim(ruc) != '' 
                            AND trim(ruc) != '#N/D'
	                        AND trim(telefono)!=''
	                        AND trim(telefono)!='.'
                        ORDER BY 
                            TRIM(nombre) ASC ;
                       ";
            /*
            SELECT iddistribuidores, TRIM(nombre) AS nombre, TRIM(ruc) AS ruc, TRIM(distrito) AS distrito, 
            TRIM(provincia) AS provincia, TRIM(departamento) AS departamento, TRIM(telefono) AS telefono, 
            TRIM(correo) AS correo, TRIM(direccion) AS direccion, REPLACE(TRIM(direccion), ' ', '%20') AS direccion2
            FROM public.distribuidores limit 100;
            */
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
        /*
        public async Task<IEnumerable<Tldistribuidores>> ListadoGeneralDistribuidores(TlFilterDistribuidor tlfilterDistri)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT iddistribuidores, TRIM(nombre) AS nombre, TRIM(ruc) AS ruc, TRIM(distrito) AS distrito, 
                        TRIM(provincia) AS provincia, TRIM(departamento) AS departamento, TRIM(telefono) AS telefono, 
                        TRIM(correo) AS correo, TRIM(direccion) AS direccion, REPLACE(TRIM(direccion), ' ', '%20') AS direccion2
                        FROM public.distribuidores 
                        WHERE 
                            TRIM(ruc) IS NOT NULL 
                            AND TRIM(ruc) != '' 
                            AND TRIM(ruc) != '#N/D'
                            AND TRIM(telefono) != ''
                            AND TRIM(telefono) != '.'";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(tlfilterDistri.Departamento))
            {
                sql += " AND UPPER(TRIM(departamento)) = UPPER(TRIM(@Departamento))";
                parameters.Add("Departamento", $"{tlfilterDistri.Departamento}");
            }

            if (!string.IsNullOrWhiteSpace(tlfilterDistri.Provincia))
            {
                sql += " AND UPPER(TRIM(provincia)) = UPPER(TRIM(@Provincia))";
                parameters.Add("Provincia", $"{tlfilterDistri.Provincia}");
            }

            if (!string.IsNullOrWhiteSpace(tlfilterDistri.Distrito))
            {
                sql += " AND UPPER(TRIM(distrito)) = UPPER(TRIM(@Distrito))";
                parameters.Add("Distrito", $"{tlfilterDistri.Distrito}");
            }

            sql += " ORDER BY TRIM(nombre) ASC;"; // Agregamos la cláusula ORDER BY

            try
            {
                return await db.QueryAsync<Tldistribuidores>(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los distribuidores: " + ex.Message, ex);
            }
        }
        */

        public async Task<IEnumerable<Tldistribuidores>> ListadoGeneralDistribuidores(TlFilterDistribuidor tlfilterDistri)
        {
            var db = DbConnection();

            var sql = @"
            WITH DistribuidoresConNumeracion AS (
                SELECT 
                    iddistribuidores, 
                    TRIM(nombre) AS nombre, 
                    TRIM(ruc) AS ruc, 
                    TRIM(distrito) AS distrito, 
                    TRIM(provincia) AS provincia, 
                    TRIM(departamento) AS departamento, 
                    TRIM(telefono) AS telefono, 
                    TRIM(correo) AS correo, 
                    TRIM(direccion) AS direccion, 
                    REPLACE(TRIM(direccion), ' ', '%20') AS direccion2,
                    ROW_NUMBER() OVER (PARTITION BY TRIM(ruc) ORDER BY TRIM(nombre)) AS rn
                FROM 
                    public.distribuidores 
                WHERE 
                    TRIM(ruc) IS NOT NULL 
                    AND TRIM(ruc) != '' 
                    AND TRIM(ruc) != '#N/D' 
                    AND TRIM(telefono) != '' 
                    AND TRIM(telefono) != '.'";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(tlfilterDistri.Departamento))
            {
                sql += " AND UPPER(TRIM(departamento)) LIKE UPPER(TRIM(@Departamento))";
                parameters.Add("Departamento", $"{tlfilterDistri.Departamento}");
            }

            if (!string.IsNullOrWhiteSpace(tlfilterDistri.Provincia))
            {
                sql += " AND UPPER(TRIM(provincia)) LIKE UPPER(TRIM(@Provincia))";
                parameters.Add("Provincia", $"{tlfilterDistri.Provincia}");
            }

            if (!string.IsNullOrWhiteSpace(tlfilterDistri.Distrito))
            {
                sql += " AND UPPER(TRIM(distrito)) LIKE UPPER(TRIM(@Distrito))";
                parameters.Add("Distrito", $"{tlfilterDistri.Distrito}");
            }
            sql += @"
            )
            SELECT 
                iddistribuidores, 
                nombre, 
                ruc, 
                distrito, 
                provincia, 
                departamento, 
                telefono, 
                correo, 
                direccion, 
                direccion2
            FROM 
                DistribuidoresConNumeracion 
            WHERE 
                rn = 1 
            ORDER BY 
                TRIM(nombre) ASC;"; // Agregamos la cláusula ORDER BY

            try
            {
                return await db.QueryAsync<Tldistribuidores>(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los distribuidores: " + ex.Message, ex);
            }
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
