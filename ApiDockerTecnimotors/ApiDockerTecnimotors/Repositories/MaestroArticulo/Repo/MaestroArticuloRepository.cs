
using ApiAlmacen.Context;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Repo
{
    public class MaestroArticuloRepository(PostgreSQLConfiguration connectionString) : IMaestroArticuloRepository
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;
        protected NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<TlArticulo>> GetArticulosAsync()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN 'KENDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN 'MAXXIS'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'CHENG SHIN'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'TSK'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN 'KATANA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN 'SUNF'
							 ELSE '' END AS marcaoriginal,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 ELSE '' END AS ancho,
	
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 3), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 3), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 3), '-', 1) like '%/%'  THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 3), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 ELSE '' END AS perfil,
	

						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 ELSE '' END AS aro,
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion FROM public.maestro_articulos where tipo between '02' and '03' AND familia = '003'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }

        public async Task<IEnumerable<TlArticulo>> GetAllSinFiltroArticulo(string value)
        {
            var db = DbConnection();
            var sql = @"
                        SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN 'KENDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN 'MAXXIS'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'CHENG SHIN'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'TSK'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN 'KATANA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN 'SUNF'
							 ELSE '' END AS marcaoriginal,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 ELSE '' END AS ancho,
	
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 3), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 3), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 3), '-', 1) like '%/%'  THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 3), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 ELSE '' END AS perfil,
	

						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 ELSE '' END AS aro,
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion FROM public.maestro_articulos where tipo between '02' and '03' AND familia = '003'
						AND marca like '%" + value +@"%' order by id asc) AS subquery
                       ";
            return await db.QueryAsync<TlArticulo>(sql, new { });
        }

		public async Task<IEnumerable<TlArticulo>> GetAllFiltroMarcaCocada(string value,string cocada)
		{
			var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN 'KENDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN 'MAXXIS'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'CHENG SHIN'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'TSK'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN 'KATANA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN 'SUNF'
							 ELSE '' END AS marcaoriginal,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) ELSE '' END)
							 ELSE '' END AS ancho,
	
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 3), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 3), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 3), '-', 1) like '%/%'  THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 3), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) ELSE (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%/%' THEN (case when split_part(split_part(subquery.marca, ' ', 2), '-', 1) like '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2), '-', 1),'/',2) ELSE split_part(split_part(subquery.marca, ' ', 2), '-', 1) END) END) END)
							 ELSE '' END AS perfil,
	

						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'K%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CSM%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'F%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'A-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2)ELSE '' END) 
							 ELSE '' END AS aro,
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion FROM public.maestro_articulos where tipo between '02' and '03' AND familia = '003'
						AND marca like '%" + value +@"%' AND codigo_equivalente like '%"+ cocada + @"%' order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }
        public async Task<IEnumerable<TlMarca>> GetMarcaAsync()
        {
            var db = DbConnection();
            var sql = @"
                        SELECT id, TRIM(marca) AS marca  FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003' ORDER BY subfamilia::INTEGER DESC
                       ";
            return await db.QueryAsync<TlMarca>(sql, new { });
        }
    }
}
