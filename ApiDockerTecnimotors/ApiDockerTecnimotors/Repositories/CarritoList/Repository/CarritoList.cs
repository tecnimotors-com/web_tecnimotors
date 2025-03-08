using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.CarritoList.Interface;
using ApiDockerTecnimotors.Repositories.CarritoList.Models;
using ApiDockerTecnimotors.Repositories.MaestroClasificado.Model;
using Dapper;
using Npgsql;

namespace ApiDockerTecnimotors.Repositories.CarritoList.Repository
{
    public class CarritoList(PostgreSQLConfiguration connectionString) : ICarritoList
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;
        private NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<TlModelsCarrito>> ListadoCarritoList()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT idcarritocotizacion, uuidcliente, ordernshopping, descripcion, unidad, categoria, marca, marcaoriginal,
                        medida, modelo, medidaestandarizado, id, codigo, familia, subfamilia, tipo, sku, producto, vendor, color, 
                        pathimagen, estado, fecharegistro, cantidad, quantity
	                    FROM public.carritocotizacion where estado= 'Activo' order by idcarritocotizacion asc
                       ";
            return await db.QueryAsync<TlModelsCarrito>(sql, new { });
        }

        public async Task<IEnumerable<TlModelsCarrito>> ListadoCarritoList(string uuidCliente)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT idcarritocotizacion, uuidcliente, ordernshopping, descripcion, unidad, categoria, marca, marcaoriginal, 
                        medida, modelo, medidaestandarizado, id, codigo, familia, subfamilia, tipo, sku, producto, vendor, color, 
                        pathimagen, estado, fecharegistro, cantidad, quantity
	                    FROM public.carritocotizacion where estado= 'Activo' and uuidcliente = @Uuidcliente order by idcarritocotizacion asc
                       ";
            return await db.QueryAsync<TlModelsCarrito>(sql, new { Uuidcliente = uuidCliente });
        }

        public async Task<TlModelsCarrito> GetCarritoListItemByCode(string uuidCliente, string codigo)
        {
            var db = DbConnection();
            var sql = @"
                        SELECT * FROM public.carritocotizacion
                        WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado';
                       ";
            var result = await db.QueryFirstOrDefaultAsync<TlModelsCarrito>(sql, new { Uuidcliente = uuidCliente, Codigo = codigo });
            return result!;
        }

        public async Task<bool> UpdateCarritoListItem(TlModelsCarrito item)
        {
            try
            {
                var db = DbConnection();
                var sql = @"
                            UPDATE public.carritocotizacion
                            SET
                                estado = @Estado,
                                cantidad = @Cantidad,
                                quantity = @Cantidad
                            WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado' 
        ";

                // Asegúrate de incluir la propiedad Cantidad en el objeto anónimo
                var result = await db.ExecuteAsync(sql, new
                {
                    Estado = item.Estado!,
                    Uuidcliente = item.Uuidcliente!,
                    Codigo = item.Codigo!,
                    Cantidad = item.Cantidad!,
                    Quantity = item.Quantity!
                });

                return result > 0; // Devuelve true si se actualizó al menos un registro
            }
            catch (Exception ex)
            {
                // Manejo de errores: registrar el error o lanzar una excepción
                Console.WriteLine($"Error al actualizar el carrito: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveFromCarritoList(string uuidcliente, string codigo)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE public.carritocotizacion
                        SET estado = 'Inactivo'
                        WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado' ; 
                       ";

            var result = await db.ExecuteAsync(sql, new { Uuidcliente = uuidcliente, Codigo = codigo });
            return result > 0;
        }

        public async Task<bool> UpdateCantidadCarrito(string uuidcliente, string codigo, int cantidad)
        {
            try
            {
                var db = DbConnection();

                var sql = @"
                            UPDATE public.carritocotizacion
                            SET cantidad = @Cantidad,
                                quantity = @Cantidad
                            WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado' 
                          ";

                var result = await db.ExecuteAsync(sql, new
                {
                    Cantidad = cantidad,
                    Uuidcliente = uuidcliente,
                    Codigo = codigo
                });

                return result > 0; // Devuelve true si se actualizó al menos un registro
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrar el error o lanzar una excepción)
                Console.WriteLine($"Error al actualizar la cantidad en el carrito: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> RegistrarCarritoList(TrModelsCarrito Trmodels)
        {
            var db = DbConnection();

            var sql = @"
                        INSERT INTO public.carritocotizacion(
	                    uuidcliente, ordernshopping, descripcion, unidad, categoria, marca, marcaoriginal, medida, modelo, medidaestandarizado,
                        id, codigo, familia, subfamilia, tipo, sku, producto, vendor, color, pathimagen, estado, fecharegistro, cantidad, quantity)
	                    VALUES (@Uuidcliente, @Ordernshopping, @Descripcion, @Unidad, @Categoria, @Marca, @Marcaoriginal, @Medida, @Modelo, @Medidaestandarizado,
                        @Id, @Codigo, @Familia, @Subfamilia, @Tipo, @Sku, @Producto, @Vendor, @Color, @Pathimagen, @Estado, @Fecharegistro, @Cantidad, @Quantity);
                       ";

            var result = await db.ExecuteAsync(sql, new
            {
                Trmodels.Idcarritocotizacion,
                Trmodels.Uuidcliente,
                Trmodels.Ordernshopping,
                Trmodels.Descripcion,
                Trmodels.Unidad,
                Trmodels.Categoria,
                Trmodels.Marca,
                Trmodels.Marcaoriginal,
                Trmodels.Medida,
                Trmodels.Modelo,
                Trmodels.Medidaestandarizado,
                Trmodels.Id,
                Trmodels.Codigo,
                Trmodels.Familia,
                Trmodels.Subfamilia,
                Trmodels.Tipo,
                Trmodels.Sku,
                Trmodels.Producto,
                Trmodels.Vendor,
                Trmodels.Color,
                Trmodels.Pathimagen,
                Trmodels.Estado,
                Trmodels.Fecharegistro,
                Trmodels.Cantidad,
                Trmodels.Quantity
            });

            return result > 0; // Devuelve true si se insertó al menos un registro
        }


        public async Task<TlModelsCarrito> GetCarritoListCotizacionByCode(string uuidCliente, string codigo)
        {
            var db = DbConnection();
            var sql = @"
                        SELECT * FROM public.carritocotizacion
                        WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo AND estado != 'Cotizado';
                       ";
            var result = await db.QueryFirstOrDefaultAsync<TlModelsCarrito>(sql, new { Uuidcliente = uuidCliente, Codigo = codigo });
            return result!;
        }
        public async Task<bool> UpdateCarritoCotizacionItem(TlModelsCarrito item)
        {
            try
            {
                var db = DbConnection();
                var sql = @"
                            UPDATE public.carritocotizacion
                            SET
                                estado = @Estado,
                                cantidad = @Cantidad,
                                quantity = @Cantidad,
                                ordernshopping = @Ordernshopping
                            WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado' ;
        ";

                // Asegúrate de incluir la propiedad Cantidad en el objeto anónimo
                var result = await db.ExecuteAsync(sql, new
                {
                    Estado = item.Estado!,
                    Uuidcliente = item.Uuidcliente!,
                    Codigo = item.Codigo!,
                    Cantidad = item.Cantidad!,
                    Quantity = item.Quantity!,
                    Ordernshopping = item.Ordernshopping!
                });

                return result > 0; // Devuelve true si se actualizó al menos un registro
            }
            catch (Exception ex)
            {
                // Manejo de errores: registrar el error o lanzar una excepción
                Console.WriteLine($"Error al actualizar el carrito: {ex.Message}");
                return false;
            }
        }

        public async Task<TlModelsCarrito> CotizacionRegistrer(string uuidCliente, string codigo)
        {
            var db = DbConnection();
            var sql = @"
                        SELECT * FROM public.carritocotizacion
                        WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado' AND estado != 'Inactivo';
                       ";
            var result = await db.QueryFirstOrDefaultAsync<TlModelsCarrito>(sql, new { Uuidcliente = uuidCliente, Codigo = codigo });
            return result!;
        }

        public async Task<bool> UpdateCotizadorRegister(TlModelsCarrito item)
        {
            try
            {
                var db = DbConnection();
                var sql = @"
                            UPDATE public.carritocotizacion
                            SET
                                estado = @Estado,
                                cantidad = @Cantidad,
                                quantity = @Cantidad,
                                ordernshopping = @Ordernshopping
                            WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo and estado != 'Cotizado' AND estado != 'Inactivo';
        ";

                // Asegúrate de incluir la propiedad Cantidad en el objeto anónimo
                var result = await db.ExecuteAsync(sql, new
                {
                    Estado = item.Estado!,
                    Uuidcliente = item.Uuidcliente!,
                    Codigo = item.Codigo!,
                    Cantidad = item.Cantidad!,
                    Quantity = item.Quantity!,
                    Ordernshopping = item.Ordernshopping!
                });

                return result > 0; // Devuelve true si se actualizó al menos un registro
            }
            catch (Exception ex)
            {
                // Manejo de errores: registrar el error o lanzar una excepción
                Console.WriteLine($"Error al actualizar el carrito: {ex.Message}");
                return false;
            }
        }


    }
}
