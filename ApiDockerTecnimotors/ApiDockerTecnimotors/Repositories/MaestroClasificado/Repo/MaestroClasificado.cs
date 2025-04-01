using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Interface;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Model;
using Dapper;
using Npgsql;
using System.Text;

namespace ApiDockerTecnimotors.Repositories.MaestroClasificado.Repo
{
    public class MaestroClasificado(PostgreSQLConfiguration connectionString) : IMaestroClasificado
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;

        private NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> ExisteCodigoInterno(string codigoInterno)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT COUNT(1) FROM public.maestro_articulo_clasificado 
                        WHERE TRIM(codigo) = TRIM(@CodigoInterno) AND estado = '1'
                      ";

            var count = await db.ExecuteScalarAsync<int>(sql, new { CodigoInterno = codigoInterno });
            return count > 0;
        }

        public async Task<IEnumerable<TlClasificado>> ListadoCategoriaVehiculos()
        {
            var db = DbConnection();

            var sql = @"
						SELECT DISTINCT aplicacion 
                        FROM public.maestro_articulo_clasificado 
                        WHERE categoriageneral = 'Vehiculos' 
                          AND estado = '1' 
                          AND aplicacion IS NOT null 
                          AND familia != '998' 
                        ORDER BY aplicacion ASC
                       ";

            return await db.QueryAsync<TlClasificado>(sql, new { });
        }
        public async Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralArticulo()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT * FROM public.maestro_articulo_clasificado where estado = '1' AND familia != '998' 
                       ";

            return await db.QueryAsync<TlMaestroGeneral>(sql, new { });
        }
        public async Task ActualizarPathImagen(string codigo, string pathImagen)
        {
            var db = DbConnection(); // Asegúrate de que DbConnection() devuelva una conexión válida
            var sql = @"
            UPDATE public.maestro_articulo_clasificado
            SET pathimagen = @PathImagen
            WHERE TRIM(codigo) = TRIM(@Codigo)"; // Usar TRIM para evitar problemas con espacios

            var parameters = new
            {
                Codigo = codigo,
                PathImagen = pathImagen
            };

            await db.ExecuteAsync(sql, parameters); // Ejecutar la consulta
        }
        public async Task ActualizarPathImagenBatch(List<(string Codigo, string PathImagen)> actualizaciones)
        {
            using (var db = DbConnection())
            {
                await db.OpenAsync();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"
                                    UPDATE public.maestro_articulo_clasificado
                                    SET pathimagen = @PathImagen
                                    WHERE TRIM(codigo) = TRIM(@Codigo)";

                        foreach (var (codigo, pathImagen) in actualizaciones)
                        {
                            var parameters = new
                            {
                                Codigo = codigo,
                                PathImagen = pathImagen
                            };

                            await db.ExecuteAsync(sql, parameters, transaction); // Ejecutar la consulta dentro de la transacción
                        }

                        transaction.Commit(); // Confirmar la transacción
                    }
                    catch
                    {
                        transaction.Rollback(); // Revertir la transacción en caso de error
                        throw; // Maneja el error según sea necesario
                    }
                }
            }
        }

        /*
        public async Task ActualizarPathImagenBatch(List<(string Codigo, string PathImagen)> actualizaciones)
        {
            var db = DbConnection(); // Asegúrate de que DbConnection() devuelva una conexión válida
            var sql = @"
                        UPDATE public.maestro_articulo_clasificado
                        SET pathimagen = @PathImagen
                        WHERE TRIM(codigo) = TRIM(@Codigo)";

            foreach (var (codigo, pathImagen) in actualizaciones)
            {
                var parameters = new
                {
                    Codigo = codigo,
                    PathImagen = pathImagen
                };

                await db.ExecuteAsync(sql, parameters); // Ejecutar la consulta
            }
        }
        */
        public async Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralCategoria(string motocicleta)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id, codigo, descripcion, unidadmedida, marcaoriginal, tipo, familia, subfamilia, abreviado, 
                               codigoequivalente, aplicacion, categoria, marca, producto, medida, medidaestandarizado, 
                               modelo, tipo1, tipo2, tipo3, tipo4, tipo5, tipo6, tipo7, gironegocio, equivalencia, 
                               fabricante, categoriageneral, clasificacionproveedor, estado, pathimagen
                        FROM public.maestro_articulo_clasificado 
                        WHERE categoriageneral = 'Vehiculos' 
                          AND aplicacion = '" + motocicleta + @"'
                          AND estado = '1' 
                          AND aplicacion IS NOT null 
                          AND familia != '998' 
                        ORDER BY aplicacion ASC
                       ";

            return await db.QueryAsync<TlMaestroGeneral>(sql, new { });
        }

        public async Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralVehiculos(string categoria, string medida, string marca)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT id, codigo, descripcion, unidadmedida, marcaoriginal, tipo, familia, subfamilia, abreviado, ");
            sql.AppendLine("codigoequivalente, aplicacion, categoria, marca, producto, medida, medidaestandarizado, ");
            sql.AppendLine("modelo, tipo1, tipo2, tipo3, tipo4, tipo5, tipo6, tipo7, gironegocio, equivalencia, ");
            sql.AppendLine("fabricante, categoriageneral, clasificacionproveedor, estado, pathimagen ");
            sql.AppendLine("FROM public.maestro_articulo_clasificado ");
            sql.AppendLine("WHERE categoriageneral = 'Vehiculos' AND estado = '1' AND aplicacion IS NOT NULL AND familia != '998' ");

            // Lista para almacenar los parámetros
            var parameters = new DynamicParameters();

            // Agregar condiciones basadas en los parámetros proporcionados
            if (!string.IsNullOrEmpty(categoria))
            {
                sql.AppendLine("AND aplicacion = @Categoria ");
                parameters.Add("Categoria", categoria);
            }

            if (!string.IsNullOrEmpty(medida))
            {
                sql.AppendLine("AND marcaoriginal = @Medida ");
                parameters.Add("Medida", medida);
            }

            if (!string.IsNullOrEmpty(marca))
            {
                sql.AppendLine("AND marca = @Marca ");
                parameters.Add("Marca", marca);
            }

            sql.AppendLine("ORDER BY aplicacion ASC");

            using (var db = DbConnection())
            {
                await db.OpenAsync();
                return await db.QueryAsync<TlMaestroGeneral>(sql.ToString(), parameters);
            }
        }
        public async Task<IEnumerable<TlMaestroModelo>> ListadoModeloVehiculo(string categoria, string marca)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT DISTINCT marcaoriginal FROM public.maestro_articulo_clasificado ");
            sql.AppendLine("WHERE categoriageneral = 'Vehiculos' AND estado = '1' AND marcaoriginal IS NOT NULL AND familia != '998' ");

            // Lista para almacenar los parámetros
            var parameters = new DynamicParameters();

            // Agregar condiciones basadas en los parámetros proporcionados
            if (!string.IsNullOrEmpty(categoria))
            {
                sql.AppendLine("AND aplicacion = @Categoria ");
                parameters.Add("Categoria", categoria);
            }

            if (!string.IsNullOrEmpty(marca))
            {
                sql.AppendLine("AND marca = @Marca ");
                parameters.Add("Marca", marca);
            }

            using (var db = DbConnection())
            {
                await db.OpenAsync();
                return await db.QueryAsync<TlMaestroModelo>(sql.ToString(), parameters);
            }
        }
        public async Task<IEnumerable<TlMaestroMarca>> ListarMarcaVehiculo(string categoria, string medida)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT DISTINCT marca FROM public.maestro_articulo_clasificado ");
            sql.AppendLine("WHERE categoriageneral = 'Vehiculos' AND estado = '1' AND marca IS NOT NULL AND familia != '998' ");

            // Lista para almacenar los parámetros
            var parameters = new DynamicParameters();

            // Agregar condiciones basadas en los parámetros proporcionados
            if (!string.IsNullOrEmpty(categoria))
            {
                sql.AppendLine("AND aplicacion = @Categoria ");
                parameters.Add("Categoria", categoria);
            }

            if (!string.IsNullOrEmpty(medida))
            {
                sql.AppendLine("AND marcaoriginal = @Medida ");
                parameters.Add("Medida", medida);
            }


            using (var db = DbConnection())
            {
                await db.OpenAsync();
                return await db.QueryAsync<TlMaestroMarca>(sql.ToString(), parameters);
            }
        }
        public async Task<TlMaestroGeneral> DetalleVehiculo(int Id)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT id, codigo, descripcion, unidadmedida, marcaoriginal, tipo, familia,
                        subfamilia, abreviado, codigoequivalente, aplicacion, categoria, marca, 
                        producto, medida, medidaestandarizado, modelo, tipo1, tipo2, tipo3, tipo4,
                        tipo5, tipo6, tipo7, gironegocio, equivalencia, fabricante, categoriageneral, 
                        clasificacionproveedor, estado, pathimagen FROM public.maestro_articulo_clasificado WHERE id = " + Id;

            var result = await db.QueryFirstOrDefaultAsync<TlMaestroGeneral>(sql, new { });
            return result!;
        }

        /*-----------------------aceite-------------------------*/
        public async Task<IEnumerable<Trmarcarepuesto>> TipoMarcaAceite()
        {
            var db = DbConnection();

            var sql = @"
						SELECT distinct marca FROM public.maestro_articulo_clasificado as mart
						where mart.familia != '998' and mart.tipo = '98' AND mart.subfamilia = '016' AND mart.estado = '1'
					   ";
            return await db.QueryAsync<Trmarcarepuesto>(sql, new { });
        }
        public async Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralAceite(string TipoMarca)
        {
            var db = DbConnection();
            if (TipoMarca == "")
            {
                var sql = @"
                            SELECT * FROM public.maestro_articulo_clasificado as mart
                            where mart.familia != '998' and mart.tipo = '98' AND mart.subfamilia = '016' AND mart.estado = '1'
                           ";
                return await db.QueryAsync<TlMaestroGeneral>(sql, new { });
            }
            else
            {
                var sql = @"
                            SELECT * FROM public.maestro_articulo_clasificado as mart
                            where mart.familia != '998' and mart.tipo = '98' AND mart.subfamilia = '016' AND mart.estado = '1'
                            AND marca = '" + TipoMarca + "'";

                return await db.QueryAsync<TlMaestroGeneral>(sql, new { });
            }
        }
        public async Task<IEnumerable<Trcatrepuesto>> ListadoRepuestoCategoria()
        {
            var db = DbConnection();

            var sql = @"
                        select distinct categoria from public.maestro_articulo_clasificado
                        where categoria ILIKE '%Repuestos%' and estado = '1' and categoriageneral is not null and marca is not null
                       ";
            return await db.QueryAsync<Trcatrepuesto>(sql, new { });
        }
        public async Task<IEnumerable<Trmarcarepuesto>> ListadoRepuestoMarca(string Categoria)
        {
            var db = DbConnection();

            var sql = @"
                        select distinct marca from public.maestro_articulo_clasificado 
                        where categoria ILIKE '%" + Categoria + @"%' and estado = '1' and 
                        categoriageneral is not null and marca is not null
                       ";

            return await db.QueryAsync<Trmarcarepuesto>(sql, new { });
        }
        public async Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralRepuesto(string Categoria, string Marca)
        {
            var db = DbConnection();

            // Inicializa la consulta base
            var sql = @"select * from public.maestro_articulo_clasificado where categoria ILIKE '%Repuestos%' and estado = '1'
                        and categoriageneral is not null and marca is not null";

            // Lista para almacenar los parámetros
            var parameters = new DynamicParameters();

            // Agrega condiciones a la consulta según los parámetros
            if (!string.IsNullOrWhiteSpace(Categoria))
            {
                sql += " AND categoria ILIKE @Categoria";
                parameters.Add("Categoria", $"%{Categoria}%");
            }

            if (!string.IsNullOrWhiteSpace(Marca))
            {
                sql += " AND marca = @Marca";
                parameters.Add("Marca", Marca);
            }

            try
            {
                return await db.QueryAsync<TlMaestroGeneral>(sql, parameters);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los repuestos", ex);
            }
        }
        public async Task<IEnumerable<Trcatrepuesto>> ListadoTipoCamaras()
        {
            var db = DbConnection();

            var sql = @"
                        select distinct categoria from public.maestro_articulo_clasificado where categoria 
                        ILIKE '%Camaras%' and estado = '1' and categoriageneral is not null and marca is not null
                       ";
            return await db.QueryAsync<Trcatrepuesto>(sql, new { });
        }
        public async Task<IEnumerable<Trmarcarepuesto>> ListadoCamaraMarca(string Categoria)
        {
            var db = DbConnection();

            var sql = @"
                        select distinct marca from public.maestro_articulo_clasificado 
                        where categoria ILIKE '%" + Categoria + @"%' and estado = '1' and 
                        categoriageneral is not null and marca is not null";
            return await db.QueryAsync<Trmarcarepuesto>(sql, new { });
        }
        public async Task<IEnumerable<TlMaestroGeneral>> ListadoGeneralCamara(string Categoria, string Marca)
        {
            var db = DbConnection();

            var sql = @"select * from public.maestro_articulo_clasificado where categoria 
                        ILIKE '%Camaras%' and estado = '1' and categoriageneral is not null and marca is not null";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(Categoria))
            {
                sql += " AND categoria ILIKE @Categoria";
                parameters.Add("Categoria", $"%{Categoria}");
            }

            if (!string.IsNullOrWhiteSpace(Marca))
            {
                sql += " AND marca = @Marca";
                parameters.Add("Marca", Marca);
            }
            try
            {
                return await db.QueryAsync<TlMaestroGeneral>(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los repuestos", ex);
            }
        }

        public async Task<IEnumerable<TlLlanta>> ListadoAnchoPerfilLLANTA()
        {
            var db = DbConnection();

            var sql = @"
                    SELECT distinct CONCAT(
	                    (CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(subquery.marca, ' ', 2),'R',1) WHEN split_part(subquery.marca,' ',2) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(subquery.marca, ' ', 2),'X',1) ELSE '--' END)
	                    WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 3),'/',1) WHEN split_part(subquery.marca,' ',3) LIKE '%AT%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'AT',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%A%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'A',2),'X',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',1),'A',2) ELSE '--' END)
	                    WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1)  ELSE '--' END)
	                    WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(subquery.marca, ' ', 2),'/',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 2),'X',1),'AT',2) ELSE '--' END)
	                    ELSE '--' END) ,' ', (CASE WHEN split_part(subquery.marca, ' ', 1) LIKE 'HD%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END) 
	                    WHEN split_part(subquery.marca, ' ', 1) LIKE 'DUN%' THEN (CASE WHEN split_part(subquery.marca,' ',3) LIKE '%B%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'B',1) WHEN split_part(subquery.marca,' ',3) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'ZR',1) WHEN split_part(subquery.marca,' ',3) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'X',2),'R',1) 
	                    WHEN split_part(subquery.marca,' ',3) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'R',1) WHEN split_part(subquery.marca,' ',3) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca, ' ', 3),'/',2),'-',1) WHEN split_part(subquery.marca,' ',3) LIKE '%-%' THEN split_part(split_part(subquery.marca, ' ', 3),'-',1) ELSE '' END) 
	                    WHEN split_part(subquery.marca, ' ', 1) LIKE 'W-%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
	                    WHEN split_part(subquery.marca, ' ', 1) LIKE 'CS%' THEN (CASE WHEN split_part(subquery.marca,' ',2) LIKE '%X%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'X',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%ZR%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'ZR',1),'/',2) WHEN split_part(subquery.marca,' ',2) LIKE '%R%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'R',1),'/',2) 
	                    WHEN split_part(subquery.marca,' ',2) LIKE '%/%' THEN split_part(split_part(split_part(subquery.marca,' ',2),'/',2),'-',1) WHEN split_part(subquery.marca,' ',2) LIKE '%-%' THEN split_part(split_part(subquery.marca,' ',2),'-',1) ELSE '' END)
	                    ELSE '' END)) as anchoperfil FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
	                    AND estado = '1' order by id asc) AS subquery
                    ";
            return await db.QueryAsync<TlLlanta>(sql, new { });
        }
        public async Task<IEnumerable<LstmodelAro>> AllListadoCocadaAroLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
        public async Task<IEnumerable<LstmodelCodada>> AllListadoCocadaCocadaLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
        public async Task<IEnumerable<LstmodelMarca>> AllListadoCocadaMarcaLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
        public async Task<IEnumerable<LstmodelTipoUso>> AllListadoCocadaTipoUsoLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
        {
            var db = DbConnection();
            if (Marca == "")
            {
                var sql = @"
						SELECT DISTINCT
						CASE WHEN subquery.marca LIKE '%TT%' THEN 'TT' WHEN subquery.marca LIKE '%TL%' THEN 'TL' ELSE 'No Tiene' END AS tipouso
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
        public async Task<IEnumerable<TlArticulo>> AllListadoCocadaArticuloLLANTA(string Ancho, string Perfil, string Aro, string Cocada, string Marca, string TipoUso)
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
	 
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
	 
						FROM (SELECT id, marcaoriginal AS marca, codigo, descripcion, estado FROM public.maestro_articulo_clasificado WHERE tipo BETWEEN '02' AND '03' AND familia = '003'
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
    
        public async Task<IEnumerable<TlMedida>> ListadoLLantaMedida()
        {
            var db = DbConnection();

            var sql = @"
                        select distinct medida from public.maestro_articulo_clasificado where categoria 
                        ILIKE '%Llantas%' and estado = '1' and categoriageneral is not null and marca is
                        not null and medida is not null and modelo is not null
                       ";
            return await db.QueryAsync<TlMedida>(sql, new { });
        }
        public async Task<IEnumerable<TlModelo>> ListadoLLantaModelo()
        {
            var db = DbConnection();

            var sql = @"
                        select distinct modelo from public.maestro_articulo_clasificado where categoria 
                        ILIKE '%Llantas%' and estado = '1' and categoriageneral is not null and marca is
                        not null and medida is not null and modelo is not null
                       ";
            return await db.QueryAsync<TlModelo>(sql, new { });
        }
        public async Task<IEnumerable<TlMarca>> ListadoLLantaMarca()
        {
            var db = DbConnection();

            var sql = @"
                        select distinct marca from public.maestro_articulo_clasificado where categoria 
                        ILIKE '%Llantas%' and estado = '1' and categoriageneral is not null and marca is
                        not null and medida is not null and modelo is not null
                       ";
            return await db.QueryAsync<TlMarca>(sql, new { });
        }
        public async Task<IEnumerable<TlCategoria>> ListadoLLantaCategoria()
        {
            var db = DbConnection();

            var sql = @"
                        select distinct categoria from public.maestro_articulo_clasificado where categoria 
                        ILIKE '%Llantas%' and estado = '1' and categoriageneral is not null and marca is 
                        not null and medida is not null and modelo is not null
                       ";
            return await db.QueryAsync<TlCategoria>(sql, new { });
        }
        
    }
}
