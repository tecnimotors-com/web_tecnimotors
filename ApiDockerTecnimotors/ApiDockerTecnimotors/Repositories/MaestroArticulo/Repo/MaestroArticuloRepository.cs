
using ApiAlmacen.Context;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Interface;
using ApiDockerTecnimotors.Repositories.MaestroArticulo.Model;
using Dapper;
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
        public async Task<IEnumerable<TrFromGeneral>> ListArticulosGeneral()
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

        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

        public async Task<TlDetalleArticulo> DetalleArticulo(int Id)
        {
            var db = DbConnection();
            var sql = @"
						SELECT id, TRIM(unidad_medida) AS unidad_medida, TRIM(abreviado) AS abreviado, TRIM(codigo_equivalente) AS codigo_equivalente,
						TRIM(estado) AS estado, TRIM(subquery.marca) AS marca, TRIM(codigo) as codigo, TRIM(descripcion) as descripcion,
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
	 
						FROM (SELECT id, unidad_medida, abreviado, codigo_equivalente, marca AS marca, codigo, descripcion, estado
						FROM public.maestro_articulos WHERE tipo BETWEEN '02' AND '03' AND familia = '003' AND estado = '1'AND id = " + Id + @"
						order by id asc) AS subquery
                       ";

            var result = await db.QueryFirstOrDefaultAsync<TlDetalleArticulo>(sql, new { });
            return result!;
        }

        /*------------------ tipo producto camara --------------*/
        public async Task<IEnumerable<TlCategoriesCamara>> ListCategorieCamara()
        {
            var db = DbConnection();
            var sql = @"
						SELECT distinct mta.tipo_articulo, mta.descripcion_tipo_articulo
						FROM public.maestro_articulos as mart
						join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo
						where mart.familia != '998' and mart.tipo BETWEEN '06' AND '07' and
						trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
					   ";
            return await db.QueryAsync<TlCategoriesCamara>(sql, new { });
        }

        public async Task<IEnumerable<TlmodeloCamara>> ListModeloCamara(string txtcategoria, string txtmarca)
        {
            var db = DbConnection();
            if (txtcategoria == "0" && txtmarca == "0")
            {
                var sql = @"
							SELECT id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos AS mart
							WHERE mart.familia != '998' AND mart.tipo BETWEEN '06' AND '07' AND
							TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
					   ";
                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
            else if (txtcategoria == "0" && txtmarca != "0")
            {
                var sql = @"
							SELECT id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos AS mart
							WHERE mart.familia != '998' AND mart.tipo BETWEEN '06' AND '07' AND
							TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
							AND mart.descripcion LIKE '%" + txtmarca + @"%'
						   ";
                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
            else if (txtcategoria != "0" && txtmarca == "0")
            {
                var sql = @"
							SELECT id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos AS mart
							WHERE mart.familia != '998' AND mart.tipo = '" + txtcategoria + @"' AND
							TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
						   ";
                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
            else if (txtcategoria != "0" && txtmarca != "0")
            {
                var sql = @"
							SELECT id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos AS mart
							WHERE mart.familia != '998' AND mart.tipo = '" + txtcategoria + @"' AND
							TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
							AND mart.descripcion LIKE '%" + txtmarca + @"%'
						  ";
                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
            else
            {
                var sql = @"
							SELECT id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos AS mart
							WHERE mart.familia != '998' AND mart.tipo BETWEEN '06' AND '07' AND
							TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
						  ";
                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
        }
        public async Task<IEnumerable<TlListCamaraAll>> ListadoCamaraGeneral(string txtcategoria, string txtmarca)
        {
            var db = DbConnection();
            if (txtcategoria == "0" && txtmarca == "0")
            {
                var sql = @"
							SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
							mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							where mart.familia != '998' and mart.tipo BETWEEN '06' AND '07' AND
							trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
						   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }
            else if (txtcategoria == "0" && txtmarca != "0")
            {
                var sql = @"
							SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
							mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							where mart.familia != '998' and mart.tipo BETWEEN '06' AND '07' AND
							trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744' and mart.descripcion like '%" + txtmarca + @"%'
						   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }

            else if (txtcategoria != "0" && txtmarca == "0")
            {
                var sql = @"
							SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
							mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							where mart.familia != '998' and mart.tipo = '" + txtcategoria + @"' and
							trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
						   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }

            else if (txtcategoria != "0" && txtmarca != "0")
            {
                var sql = @"
							SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
							mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							where mart.familia != '998' and mart.tipo = '" + txtcategoria + @"' and
							trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744' AND mart.descripcion like '%" + txtmarca + @"%'
						   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }

            else
            {
                var sql = @"
							SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
							mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							where mart.familia != '998' and mart.tipo BETWEEN '06' AND '07' AND
							trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
						   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }
        }

        public async Task<TlListCamaraAll> DetalleCamaraAll(int Id)
        {
            var db = DbConnection();
            var sql = @"
						SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
						mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado,
						TRIM(mart.codigo_equivalente) AS codigo_equivalente FROM public.maestro_articulos as mart
						join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
						where mart.id= " + Id + @"
                       ";

            var result = await db.QueryFirstOrDefaultAsync<TlListCamaraAll>(sql, new { });
            return result!;
        }

        public async Task<IEnumerable<TlListCamaraAll>> ListadoCamaraGeneralModelo(string IdCamara)
        {
            var db = DbConnection();

            var sql = @"
						SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
						mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
						FROM public.maestro_articulos as mart
						join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
						where mart.id= " + IdCamara + @"
					  ";
            return await db.QueryAsync<TlListCamaraAll>(sql, new { });

        }


        /*---------------------- Aceite y Lubricantes----------------------*/
        public async Task<IEnumerable<TlCategoriesCamara>> ListCategorieAceite()
        {
            var db = DbConnection();
            var sql = @"
						SELECT DISTINCT mta.tipo_articulo, mta.descripcion_tipo_articulo
						FROM public.maestro_articulos AS mart
						JOIN public.maestro_tipo_articulo AS mta ON mart.tipo = mta.tipo_articulo
						WHERE mart.familia != '998' AND mart.tipo = '98' AND subfamilia = '016'
						AND TRIM(mart.codigo) NOT IN ('VTM0032743', 'VTM0032744');
					   ";
            return await db.QueryAsync<TlCategoriesCamara>(sql, new { });
        }

        public async Task<IEnumerable<TlmodeloCamara>> ListModeloAceite(string txtcategoria)
        {
            var db = DbConnection();
            var sql = @"
						SELECT id, TRIM(mart.marca) AS descripcion_modificada
						FROM public.maestro_articulos AS mart
						WHERE mart.familia != '998' AND mart.tipo = '98' AND subfamilia = '016' AND
						TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
					   ";
            return await db.QueryAsync<TlmodeloCamara>(sql, new { });
        }

        public async Task<IEnumerable<TlListCamaraAll>> ListadoAceiteGeneral(string txtcategoria)
        {
            var db = DbConnection();
            if (txtcategoria == "0")
            {
                var sql = @"
						SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
						mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
						FROM public.maestro_articulos as mart
						join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
						where mart.familia != '998' and mart.tipo = '98' AND subfamilia = '016' AND
						trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
					   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }
            else
            {
                var sql = @"
						SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
						mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
						FROM public.maestro_articulos as mart
						join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
						where mart.familia != '998' and mart.tipo = '98' AND subfamilia = '016' AND
						trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
					   ";
                return await db.QueryAsync<TlListCamaraAll>(sql, new { });
            }
        }


        /*--------------------Vehiculo----------------------*/
        public async Task<IEnumerable<TlmodeloCamara>> ListModeloVehiculo(string txtcategoria)
        {
            var db = DbConnection();

            var sql = @"
						SELECT id, TRIM(mart.marca) AS descripcion_modificada
						FROM public.maestro_articulos AS mart
						WHERE mart.familia != '998' AND mart.tipo = '01' AND subfamilia = '099' 
						AND TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
						AND TRIM(mart.descripcion) like '" + txtcategoria + @"%'
					   ";

            return await db.QueryAsync<TlmodeloCamara>(sql, new { });

        }

        public async Task<IEnumerable<TlListCamaraAll>> ListadoVehiculoGeneral(string txtcategoria)
        {
            var db = DbConnection();

            var sql = @"
						SELECT distinct mart.id, TRIM(mart.codigo) AS codigo, trim(mart.descripcion) as descripcion, mart.unidad_medida, trim(mart.marca) as marca,
						mart.tipo, mta.tipo_articulo, mta.descripcion_tipo_articulo,mart.familia, mart.subfamilia, TRIM(mart.abreviado) AS abreviado, TRIM(mart.codigo_equivalente) AS codigo_equivalente
						FROM public.maestro_articulos as mart
						join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
						where mart.familia != '998' and mart.tipo = '01' AND subfamilia = '099' AND
						trim(mart.codigo) != 'VTM0032743' and trim(mart.codigo) != 'VTM0032744'
						AND TRIM(mart.descripcion) like '" + txtcategoria + @"%'
                       ";
            return await db.QueryAsync<TlListCamaraAll>(sql, new { });

        }

        /*------------------Respuesto Motocicleta-------------*/

        public async Task<IEnumerable<TlCategoriesCamara>> ListadoRepuestoTipoCategoria(string txtcategoria)
        {
            var db = DbConnection();

            if (txtcategoria == "MOTOCICLETA")
            {
                var sql = @"
							SELECT DISTINCT mta.tipo_articulo, mta.descripcion_tipo_articulo--mart.tipo, mart.familia, mart.subfamilia
							FROM public.maestro_articulos AS mart
							JOIN public.maestro_tipo_articulo AS mta ON mart.tipo = mta.tipo_articulo
							WHERE mart.familia not in ('998') and mart.tipo not in ('97')
							AND mart.tipo IN ('98', '30', '11', '32', '23','33','34','29','25')
							AND TRIM(mart.codigo) NOT IN ('VTM0032743', 'VTM0032744')
							order by mta.tipo_articulo desc			
						   ";
                return await db.QueryAsync<TlCategoriesCamara>(sql, new { });
            }
            else
            {
                var sql = @"
							SELECT DISTINCT mta.tipo_articulo, mta.descripcion_tipo_articulo--mart.tipo, mart.familia, mart.subfamilia
							FROM public.maestro_articulos AS mart
							JOIN public.maestro_tipo_articulo AS mta ON mart.tipo = mta.tipo_articulo
							WHERE mart.familia not in ('998') and mart.tipo not in ('97')
							AND mart.tipo IN ('99','55','54','53','52') 
							AND TRIM(mart.codigo) NOT IN ('VTM0032743', 'VTM0032744')
							order by mta.tipo_articulo desc			
						   ";
                return await db.QueryAsync<TlCategoriesCamara>(sql, new { });
            }
        }
        public async Task<IEnumerable<TlmodeloCamara>> ListadoModeloRepuesto(string TipoCategoria)
        {
            var db = DbConnection();

            if (TipoCategoria == "0")
            {
                var sql = @"
							SELECT distinct id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							WHERE mart.familia != '998'
							AND TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
							AND mart.tipo IN ('99','55','54','53','52','98', '30', '11', '32', '23','33','34','29','25') 
						   ";
                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
            else
            {
                var sql = @"
							SELECT distinct id, TRIM(mart.marca) AS descripcion_modificada
							FROM public.maestro_articulos as mart
							join public.maestro_tipo_articulo as mta on mart.tipo= mta.tipo_articulo 
							WHERE mart.familia != '998'
							AND TRIM(mart.codigo) != 'VTM0032743' AND TRIM(mart.codigo) != 'VTM0032744'
							AND mart.tipo = '" + TipoCategoria + @"'
						   ";

                return await db.QueryAsync<TlmodeloCamara>(sql, new { });
            }
        }

    }
}
