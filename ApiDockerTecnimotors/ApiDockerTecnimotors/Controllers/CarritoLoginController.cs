using ApiDockerTecnimotors.Repositories.CarritoList.Interface;
using ApiDockerTecnimotors.Repositories.CarritoList.Models;
using ApiDockerTecnimotors.Repositories.WishList.Interface;
using ApiDockerTecnimotors.Repositories.WishList.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoLoginController(ICarritoList ICarritoList, IWishList IWishList) : ControllerBase
    {
        private readonly IWishList IwishList = IWishList;
        private readonly ICarritoList IcarritoList = ICarritoList;

        [HttpPost("RegistrarCarritoYWishlist")]
        public async Task<IActionResult> RegistrarCarritoYWishlist([FromBody] RegistrarCarritoYWishlistRequest request)
        {
            if (request.ListCarritoLogin == null && request.ListWishLogin == null)
            {
                return BadRequest("No se recibieron elementos en el carrito ni en la wishlist.");
            }

            // Registrar carrito
            if (request.ListCarritoLogin != null)
            {
                foreach (var item in request.ListCarritoLogin)
                {
                    var existingItem = await IcarritoList.GetCarritoListItemByCode(request.Uuidcliente!, item.Codigo!.Trim()!);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = item.Cantidad;
                        existingItem.Cantidad = item.Cantidad;
                        existingItem.Estado = "Activo";
                        await IcarritoList.UpdateCarritoListItem(existingItem);
                    }
                    else
                    {
                        var newitem = new TrModelsCarrito
                        {
                            Uuidcliente = request.Uuidcliente!,
                            Ordernshopping = "",
                            Descripcion = item.Descripcion,
                            Unidad = item.Unidad,
                            Categoria = item.Categoria,
                            Marca = item.Marca,
                            Marcaoriginal = item.Marcaoriginal,
                            Medida = item.Medida,
                            Modelo = item.Modelo,
                            Medidaestandarizado = item.Medidaestandarizado,
                            Id = item.Id,
                            Codigo = item.Codigo,
                            Familia = item.Familia,
                            Subfamilia = item.Subfamilia,
                            Tipo = item.Tipo,
                            Sku = item.Sku,
                            Producto = item.Producto,
                            Vendor = item.Vendor,
                            Color = item.Color,
                            Pathimagen = item.Pathimagen,
                            Estado = "Activo",
                            Fecharegistro = DateTime.UtcNow,
                            Cantidad = item.Cantidad,
                            Quantity = item.Quantity,
                        };
                        await IcarritoList.RegistrarCarritoList(newitem);
                    }
                }
            }

            // Registrar wishlist
            if (request.ListWishLogin != null)
            {
                foreach (var item in request.ListWishLogin)
                {
                    var existingItem = await IwishList.GetWishlistItemByCode(request.Uuidcliente!, item.Codigo!.Trim()!);
                    if (existingItem != null)
                    {
                        existingItem.Estado = "Activo";
                        await IwishList.UpdateWishlistItem(existingItem);
                    }
                    else
                    {
                        var orderNumber = await IwishList.GetNextOrderNumber(request.Uuidcliente!);
                        var newItem = new TrModels
                        {
                            Uuidcliente = request.Uuidcliente!,
                            Ordenwishlist = orderNumber,
                            Descripcion = item.Descripcion,
                            Unidad = item.Unidad,
                            Categoria = item.Categoria,
                            Marca = item.Marca,
                            Marcaoriginal = item.Marcaoriginal,
                            Medida = item.Medida,
                            Modelo = item.Modelo,
                            Medidaestandarizado = item.Medidaestandarizado,
                            Id = item.Id,
                            Codigo = item.Codigo.Trim(),
                            Familia = item.Familia,
                            Subfamilia = item.Subfamilia,
                            Tipo = item.Tipo,
                            Cantidad = item.Cantidad,
                            Sku = item.Sku,
                            Producto = item.Producto,
                            Vendor = item.Vendor,
                            Quantity = item.Quantity,
                            Color = item.Color,
                            Pathimagen = item.Pathimagen,
                            Estado = "Activo",
                            Fecharegistro = DateTime.UtcNow
                        };
                        await IwishList.RegisterWishList(newItem);
                    }
                }
            }

            return Ok(new { message = "Carrito y wishlist actualizados correctamente." });
        }
    }

    public class RegistrarCarritoYWishlistRequest
    {
        public string? Uuidcliente { get; set; }
        public List<ListCarrito>? ListCarritoLogin { get; set; }
        public List<ListWish>? ListWishLogin { get; set; }
    }
}


