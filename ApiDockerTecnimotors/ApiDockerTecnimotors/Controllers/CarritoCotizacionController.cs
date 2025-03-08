using ApiDockerTecnimotors.Repositories.CarritoList.Interface;
using ApiDockerTecnimotors.Repositories.CarritoList.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoCotizacionController(ICarritoList ICarritoList) : ControllerBase
    {
        private readonly ICarritoList icarritoList = ICarritoList;

        [HttpPost("ListadoCarritoList")]
        public async Task<ActionResult> ListadoCarritoList([FromBody] TrUuidCoti trUuid)
        {
            if (trUuid.Uuidcliente == "")
            {
                return Ok();
            }
            var result = await icarritoList.ListadoCarritoList(trUuid.Uuidcliente!);
            return Ok(result);
        }

        [HttpPost("RegisterCarritoList")]
        public async Task<ActionResult> RegisterCarritoList([FromBody] TrModelsCarrito trModels)
        {
            //Verificar si el Producto ya existe en la Carrito
            var existingItem = await icarritoList.GetCarritoListItemByCode(trModels.Uuidcliente!, trModels.Codigo!);
            if (existingItem != null)
            {
                existingItem.Quantity = trModels.Cantidad;
                existingItem.Cantidad = trModels.Cantidad;
                existingItem.Estado = "Activo";
                await icarritoList.UpdateCarritoListItem(existingItem);
                return Ok();
            }
            else
            {
                //Si no existem, registrar un nuevo producto en el carrito
                var result = await icarritoList.RegistrarCarritoList(trModels);
                return Ok(result);
            }
        }

        [HttpPost("RemoveCarritoList")]
        public async Task<IActionResult> RemoveFromCarritoList([FromBody] RemoveFromCarritoListRequest request)
        {
            if (request == null)
            {
                return BadRequest("envio de informacion vacio");
            }

            var result = await icarritoList.RemoveFromCarritoList(request.Uuidcliente!, request.Codigo!);

            if (result)
            {
                return Ok();
            }

            return NotFound("Item not found in Carrito Cotización");
        }

        [HttpPost("UpdateCantidadCarrito")]
        public async Task<IActionResult> UpdateCantidadCarrito([FromBody] UpdateFromCarritoModal request)
        {
            if (request == null)
            {
                return BadRequest("envio de informacion vacio");
            }
            var result = await icarritoList.UpdateCantidadCarrito(request.Uuidcliente!, request.Codigo!, request.Cantidad!);
            if (result)
            {
                return Ok();
            }

            return NotFound("Item not found in Carrito Cotización");
        }


        [HttpPost("RegisterCotizacionList")]
        public async Task<IActionResult> RegisterCotizacionList([FromBody] TrLoginCarrito trModels)
        {
            if (trModels.ListCarrito == null)
            {
                return BadRequest("No se recibieron elementos en la cotizacion.");
            }

            foreach (var item in trModels.ListCarrito)
            {
                //Verificar si el Producto ya existe en la Carrito
                var existingItem = await icarritoList.GetCarritoListCotizacionByCode(trModels.Uuidcliente!, item.Codigo!);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Cantidad;
                    existingItem.Cantidad = item.Cantidad;
                    existingItem.Estado = "Activo";
                    await icarritoList.UpdateCarritoListItem(existingItem);
                }
                else
                {
                    var newitem = new TrModelsCarrito
                    {
                        Uuidcliente = trModels.Uuidcliente!,
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
                        Quantity = item.Cantidad,
                    };
                    //Si no existem, registrar un nuevo producto en el carrito
                    await icarritoList.RegistrarCarritoList(newitem);
                }
            }
            // No es necesario llamar a SaveChangesAsync() aquí, ya que se maneja en RegisterWishList
            return Ok(new { message = "Wishlist Registro Cotizacion" });
        }

        [HttpPost("RegistrarCarritologin")]
        public async Task<IActionResult> AddtoCarritoList([FromBody] ListCarritoRegister listCarritoRegiser)
        {
            if (listCarritoRegiser.ListCarritoLogin == null)
            {
                return BadRequest("No se recibieron elementos en la listao de carrito");
            }

            foreach (var item in listCarritoRegiser.ListCarritoLogin)
            {
                //Verificar si el producto ya existe en el carrito de cotizacion
                var existingItem = await icarritoList.CotizacionRegistrer(listCarritoRegiser.Uuidcliente!, item.Codigo!.Trim()!);
                if (existingItem != null)
                {
                    existingItem.Ordernshopping = listCarritoRegiser.Quote;
                    existingItem.Quantity = item.Cantidad;
                    existingItem.Cantidad = item.Cantidad;
                    existingItem.Estado = "Cotizado";
                    await icarritoList.UpdateCotizadorRegister(existingItem);
                }
                else
                {
                    var newitem = new TrModelsCarrito
                    {
                        Uuidcliente = listCarritoRegiser.Uuidcliente!,
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
                        Quantity = item.Cantidad,
                    };
                    //Si no existem, registrar un nuevo producto en el carrito
                    var result = await icarritoList.RegistrarCarritoList(newitem);
                    return Ok(result);
                }
            }
            // No es necesario llamar a SaveChangesAsync() aquí, ya que se maneja en RegisterWishList
            return Ok(new { message = "Wishlist actualizada correctamente." });
        }
    }
}
