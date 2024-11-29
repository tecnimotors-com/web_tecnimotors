
using ApiAlmacen.Context;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections;

namespace ApiDockerTecnimotors.Repositories.MaestroArticulo.Repo
{
    public class MaestroArticuloRepository(PostgreSQLConfiguration connectionString) : IMaestroArticuloRepository
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;
        protected NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<TrFromGeneral>> GetArticulosAsync()
        {
            var db = DbConnection();

            var sql = @"
						SELECT distinct CONCAT(
							(CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END)
							,' ',
							(CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END)) as anchoperfil
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TrFromGeneral>(sql, new { });
        }

        public async Task<IEnumerable<TlArticulo>> GetArticulosAsync(string limit, string offset)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' order by id asc limit " + limit + @" offset " + offset + @" ) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }

        public async Task<IEnumerable<TrFromGeneral>> GetPrincipalAsync(string limit, string offset)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id,
							CONCAT(
							(CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END)
							,' ',
							(CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END)) as anchoperfil
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' order by id asc limit " + limit + @" offset " + offset + @" ) AS subquery
                       ";

            return await db.QueryAsync<TrFromGeneral>(sql, new { });
        }

        public async Task<IEnumerable<TlArticulo>> GetAllSinFiltroArticulo(string value)
        {
            var db = DbConnection();
            var sql = @"
                    
                       ";
            return await db.QueryAsync<TlArticulo>(sql, new { });
        }

        public async Task<IEnumerable<TlArticulo>> GetAllFiltroMarcaCocada(string value, string cocada)
        {
            var db = DbConnection();

            var sql = @"
						
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }
        public async Task<IEnumerable<TlMarca>> GetMarcaAsync()
        {
            var db = DbConnection();
            var sql = @"
                       
                       ";
            return await db.QueryAsync<TlMarca>(sql, new { });
        }


        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfil(string Ancho, string Perfil)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelAro>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfil(string Ancho, string Perfil)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelCodada>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfil(string Ancho, string Perfil)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelMarca>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfil(string Ancho, string Perfil)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
        }
        public async Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfil(string Ancho, string Perfil)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Ancho + @"%%" + Perfil + @"%' order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }

        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public async Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilAro(string Ancho, string Perfil, string Aro)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%" + Aro + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelAro>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilAro(string Ancho, string Perfil, string Aro)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%" + Aro + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelCodada>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilAro(string Ancho, string Perfil, string Aro)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%" + Aro + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelMarca>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilAro(string Ancho, string Perfil, string Aro)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%" + Aro + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
        }
        public async Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilAro(string Ancho, string Perfil, string Aro)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Ancho + @"%%" + Perfil + @"%" + Aro + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public async Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilCocada(string Ancho, string Perfil, string Cocada)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelAro>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilCocada(string Ancho, string Perfil, string Cocada)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelCodada>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilCocada(string Ancho, string Perfil, string Cocada)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelMarca>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilCocada(string Ancho, string Perfil, string Cocada)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
        }
        public async Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilCocada(string Ancho, string Perfil, string Cocada)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public async Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilMarca(string Ancho, string Perfil, string Marca)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Marca + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelAro>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilMarca(string Ancho, string Perfil, string Marca)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Marca + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelCodada>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilMarca(string Ancho, string Perfil, string Marca)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Marca + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelMarca>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilMarca(string Ancho, string Perfil, string Marca)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Marca + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
        }
        public async Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilMarca(string Ancho, string Perfil, string Marca)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like '%" + Marca + @"%%" + Ancho + @"%%" + Perfil + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }

        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public async Task<IEnumerable<LstmodelAro>> ListadoAroAnchoPerfilTL(string Ancho, string Perfil, string TipoUso)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like  '%" + Ancho + @"%%" + Perfil + @"%%" + TipoUso + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelAro>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelCodada>> ListadoCocadaAnchoPerfilTL(string Ancho, string Perfil, string TipoUso)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like  '%" + Ancho + @"%%" + Perfil + @"%%" + TipoUso + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelCodada>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelMarca>> ListadoMarcaAnchoPerfilTL(string Ancho, string Perfil, string TipoUso)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like  '%" + Ancho + @"%%" + Perfil + @"%%" + TipoUso + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelMarca>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelTipoUso>> ListadoTipoUsoAnchoPerfilTL(string Ancho, string Perfil, string TipoUso)
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like  '%" + Ancho + @"%%" + Perfil + @"%%" + TipoUso + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
        }
        public async Task<IEnumerable<TlArticulo>> ListadoArticuloAnchoPerfilTL(string Ancho, string Perfil, string TipoUso)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1'  and marca like  '%" + Ancho + @"%%" + Perfil + @"%%" + TipoUso + @"%'
						order by id asc) AS subquery
                       ";

            return await db.QueryAsync<TlArticulo>(sql, new { });
        }


        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public async Task<IEnumerable<LstmodelAro>> AllListadoCocadaAro(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
        {
            var db = DbConnection();
            if (Marca == "")
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						order by id asc) AS subquery
                       ";
                return await db.QueryAsync<LstmodelAro>(sql, new { });
            }
            else
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						AND CASE WHEN split_part(marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
						 WHEN split_part(marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
						 WHEN split_part(marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
						 WHEN split_part(marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
						 ELSE '' END  = '" + Marca + @"'
						order by id asc) AS subquery
                       ";
                return await db.QueryAsync<LstmodelAro>(sql, new { });
            }
        }
        public async Task<IEnumerable<LstmodelCodada>> AllListadoCocadaCocada(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
        {
            var db = DbConnection();
            if (Marca == "")
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<LstmodelCodada>(sql, new { });
            }
            else
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						AND CASE WHEN split_part(marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
						 WHEN split_part(marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
						 WHEN split_part(marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
						 WHEN split_part(marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
						 ELSE '' END  = '" + Marca + @"'
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<LstmodelCodada>(sql, new { });
            }
        }
        public async Task<IEnumerable<LstmodelMarca>> AllListadoCocadaMarca(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
        {
            var db = DbConnection();
            if (Marca == "")
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<LstmodelMarca>(sql, new { });
            }
            else
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						AND CASE WHEN split_part(marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
						 WHEN split_part(marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
						 WHEN split_part(marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
						 WHEN split_part(marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
						 ELSE '' END  = '" + Marca + @"'
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<LstmodelMarca>(sql, new { });
            }
        }
        public async Task<IEnumerable<LstmodelTipoUso>> AllListadoCocadaTipoUso(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
        {
            var db = DbConnection();
            if (Marca == "")
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
            }
            else
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						AND CASE WHEN split_part(marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
						 WHEN split_part(marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
						 WHEN split_part(marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
						 WHEN split_part(marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
						 ELSE '' END  = '" + Marca + @"'
						 order by id asc) AS subquery
                       ";

                return await db.QueryAsync<LstmodelTipoUso>(sql, new { });
            }
        }
        public async Task<IEnumerable<TlArticulo>> AllListadoCocadaArticulo(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
        {
            var db = DbConnection();
            if (Marca == "")
            {
                var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<TlArticulo>(sql, new { });

            }
            else
            {
                var sql = @"
						SELECT id, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
							 ELSE '' END AS marcaoriginal,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN split_part(subquery.marca, ' ', 1)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN split_part(subquery.marca, ' ', 2) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN split_part(subquery.marca, ' ', 1) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN split_part(subquery.marca, ' ', 1) 
							 ELSE '' END AS cocada,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
							 ELSE '--' END as ancho,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
							 ELSE '' END AS perfil,
	 
						CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2), '-', 2) WHEN split_part(subquery.marca, ' ', 2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca, ' ', 3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3), '-', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'B',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'ZR',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3), '/', 2),'R',2) WHEN split_part(subquery.marca, ' ', 3) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 3), 'R', 2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca, ' ', 2) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 2),'-',2) ELSE '' END)
							 WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(subquery.marca,' ',2),'ZR',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca,' ',2),'R',2) WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',2) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',2) ELSE '' END)
							 ELSE '' END AS aro,
	 
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE '' END AS tipouso, estado
	 
						FROM (SELECT id, marca AS marca, codigo, descripcion, estado FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
						AND estado = '1' AND marca like '%" + Cocada + @"%%" + Ancho + @"%%" + Perfil + @"%%" + Aro + @"%'
						AND CASE WHEN marca LIKE '%TT%' THEN 'TT' WHEN marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END  Like '%" + TipoUso + @"%' 
						AND CASE WHEN split_part(marca, ' ', 1) LIKE 'HD%' THEN 'CELIMO' 
						WHEN split_part(marca, ' ', 1) LIKE 'DUN%' THEN 'DUNLOP'
						WHEN split_part(marca, ' ', 1) LIKE 'W-%' THEN 'WANDA'
						WHEN split_part(marca, ' ', 1) LIKE 'CS%' THEN 'MAXXIS'
						ELSE '' END  = '" + Marca + @"'
						order by id asc) AS subquery
                       ";

                return await db.QueryAsync<TlArticulo>(sql, new { });
            }

        }
    }
}