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
    }
}
