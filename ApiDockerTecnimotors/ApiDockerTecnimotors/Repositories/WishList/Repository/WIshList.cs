using ApiDockerTecnimotors.Context;
using ApiDockerTecnimotors.Repositories.WishList.Interface;
using ApiDockerTecnimotors.Repositories.WishList.Models;
using Dapper;
using Npgsql;

namespace ApiDockerTecnimotors.Repositories.WishList.Repository
{
    public class WIshList(PostgreSQLConfiguration connectionString) : IWishList
    {
        private readonly PostgreSQLConfiguration _connectionString = connectionString;
        private NpgsqlConnection DbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<TlModels>> ListadoWishList()
        {
            var db = DbConnection();

            var sql = @"
                        SELECT idwishlish, uuidcliente, ordenwishlist, descripcion, unidad, categoria, marca, marcaoriginal, medida, modelo, medidaestandarizado,
                        id, codigo, familia, subfamilia, tipo, cantidad, sku, producto, vendor, quantity, color, pathimagen, estado
                        FROM public.wishlist where estado='Activo' order by ordenwishlist asc
                       ";

            return await db.QueryAsync<TlModels>(sql, new { });
        }

        public async Task<IEnumerable<TlModels>> ListadoWishList(string uuidCliente)
        {
            var db = DbConnection();

            var sql = @"
                        SELECT idwishlish, uuidcliente, ordenwishlist, descripcion, unidad, categoria, marca, marcaoriginal, medida, modelo, medidaestandarizado,
                        id, codigo, familia, subfamilia, tipo, cantidad, sku, producto, vendor, quantity, color, pathimagen, estado
                        FROM public.wishlist where estado='Activo' and uuidcliente = @Uuidcliente order by ordenwishlist asc
                       ";

            return await db.QueryAsync<TlModels>(sql, new { Uuidcliente = uuidCliente });
        }
        
        public async Task<TlModels> GetWishlistItemByCode(string uuidCliente, string codigo)
        {
            var db = DbConnection();
            var sql = @"
                SELECT * FROM public.wishlist 
                WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo;
               ";
            var result = await db.QueryFirstOrDefaultAsync<TlModels>(sql, new { Uuidcliente = uuidCliente, Codigo = codigo });
            return result!;
        }

        public async Task<bool> UpdateWishlistItem(TlModels item)
        {
            var db = DbConnection();
            var sql = @"
                UPDATE public.wishlist
                SET estado = @Estado
                WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo;
            ";

            var result = await db.ExecuteAsync(sql, new { Estado = item.Estado, Uuidcliente = item.Uuidcliente, Codigo = item.Codigo });
            return result > 0; // Devuelve true si se actualizó al menos un registro
        }
        
        public async Task<bool> RemoveFromWishlist(string uuidCliente, string codigo)
        {
            var db = DbConnection();

            var sql = @"
                        UPDATE public.wishlist
                        SET estado = 'Inactivo'
                        WHERE uuidcliente = @Uuidcliente AND codigo = @Codigo;
                      ";

            var result = await db.ExecuteAsync(sql, new { Uuidcliente = uuidCliente, Codigo = codigo });
            return result > 0; // Devuelve true si se actualizó al menos un registro
        }
        
        public async Task<string> GetNextOrderNumber(string uuidCliente)
        {
            // Obtener el último orden activo para el cliente
            var db = DbConnection();

            var sql = @"
                        SELECT ordenwishlist
                        FROM public.wishlist
                        WHERE uuidcliente = @Uuidcliente AND estado = 'Activo'
                        ORDER BY idwishlish DESC
                        LIMIT 1;
                       ";

            var lastOrder = await db.QueryFirstOrDefaultAsync<string>(sql, new { Uuidcliente = uuidCliente });

            int lastOrderNumber;

            if (lastOrder != null)
            {
                // Extraer el número de orden y convertirlo a entero
                lastOrderNumber = int.Parse(lastOrder[3..]);
            }
            else
            {
                // Si no hay órdenes, iniciar con 1
                lastOrderNumber = 0; // Esto se usará para generar WLD000001
            }

            lastOrderNumber++; // Incrementar el número de orden
            var orderNumber = $"WLD{lastOrderNumber:D6}"; // Formatear el número de orden

            return orderNumber;
        }

        public async Task<bool> RegisterWishList(TrModels Trmodels)
        {
            var db = DbConnection();

            var sql = @"
                    INSERT INTO public.wishlist(
	                    uuidcliente, ordenwishlist, descripcion, unidad, categoria, marca, marcaoriginal,
                        medida, modelo, medidaestandarizado, id, codigo, familia, subfamilia,
                        tipo, cantidad, sku, producto, vendor, quantity, color, pathimagen, estado, fecharegistro)
	                    VALUES ( @Uuidcliente, @Ordenwishlist, @Descripcion, @Unidad, @Categoria, @Marca, @Marcaoriginal,
                        @Medida, @Modelo, @Medidaestandarizado, @Id, @Codigo, @Familia, @Subfamilia, @Tipo,
                        @Cantidad, @Sku, @Producto, @Vendor, @Quantity, @Color, @Pathimagen, @Estado, @Fecharegistro);
                       ";
            var result = await db.ExecuteAsync(sql, new
            {
                Trmodels.Idwishlish,
                Trmodels.Uuidcliente,
                Trmodels.Ordenwishlist,
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
                Trmodels.Cantidad,
                Trmodels.Sku,
                Trmodels.Producto,
                Trmodels.Vendor,
                Trmodels.Quantity,
                Trmodels.Color,
                Trmodels.Pathimagen,
                Trmodels.Estado,
                Trmodels.Fecharegistro,
            });
            return result > 0;
        }
    }
}
