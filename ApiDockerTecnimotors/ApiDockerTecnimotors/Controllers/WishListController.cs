using ApiDockerTecnimotors.Repositories.WishList.Interface;
using ApiDockerTecnimotors.Repositories.WishList.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiDockerTecnimotors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController(IWishList IWishList) : ControllerBase
    {
        private readonly IWishList Iwishlist = IWishList;

        [HttpPost("RegisterWishList")]
        public async Task<ActionResult> RegisterWishList([FromBody] TrModels trModels)
        {
            // Verificar si el producto ya existe en la wishlist
            var existingItem = await Iwishlist.GetWishlistItemByCode(trModels.Uuidcliente!, trModels.Codigo!);

            if (existingItem != null)
            {
                // Si existe, actualizar el estado a "Activo"
                existingItem.Estado = "Activo";
                await Iwishlist.UpdateWishlistItem(existingItem);
                return Ok();
            }
            else
            {
                // Si no existe, registrar un nuevo producto
                var result = await Iwishlist.RegisterWishList(trModels);
                return Ok(result);
            }
        }

        [HttpPost("ListadoWishList")]
        public async Task<ActionResult> ListadoWishList([FromBody] TrUuid TrUuid)
        {
            if (TrUuid.Uuidcliente == "")
            {
                return Ok();
            }
            var result = await Iwishlist.ListadoWishList(TrUuid.Uuidcliente!);
            return Ok(result);
        }

        [HttpPost("NextOrderNumber")]
        public async Task<IActionResult> GetNextOrderNumber([FromBody] Trtrial trtrial)
        {
            var orderNumber = await Iwishlist.GetNextOrderNumber(trtrial.Uuidcliente!);
            return Ok(new { orderNumber }); // Devolver un objeto JSON
        }


        [HttpPost("removeWishList")]
        public async Task<IActionResult> RemoveFromWishlist([FromBody] RemoveFromWishlistRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            var result = await Iwishlist.RemoveFromWishlist(request.Uuidcliente!, request.Codigo!);
            if (result)
            {
                return Ok();
            }

            return NotFound("Item not found in wishlist");
        }

        //continua ppara tmb CarritoList ya que ya esta registrarWishLogin
        [HttpPost("RegistrarWishlogin")]
        public async Task<IActionResult> AddToWishlist([FromBody] ListWishRegister listRegister)
        {
            if (listRegister.ListWishLogin == null)
            {
                return BadRequest("No se recibieron elementos en la wishlist.");
            }

            foreach (var item in listRegister.ListWishLogin)
            {
                // Verificar si el producto ya existe en la wishlist
                var existingItem = await Iwishlist.GetWishlistItemByCode(listRegister.Uuidcliente!, item.Codigo!.Trim()!);
                if (existingItem != null)
                {
                    // Si existe, actualizar el estado a "Activo"
                    existingItem.Estado = "Activo";
                    await Iwishlist.UpdateWishlistItem(existingItem);
                }
                else
                {
                    var orderNumber = await Iwishlist.GetNextOrderNumber(listRegister.Uuidcliente!);
                    // Si no existe, registrar un nuevo wishlist
                    var newItem = new TrModels
                    {
                        Uuidcliente = listRegister.Uuidcliente!,
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
                        Estado = "Activo", // Inicializar el estado
                        Fecharegistro = DateTime.UtcNow // Establecer la fecha de registro
                    };

                    // Registrar el nuevo elemento en la base de datos
                    await Iwishlist.RegisterWishList(newItem);

                }
            }

            // No es necesario llamar a SaveChangesAsync() aquí, ya que se maneja en RegisterWishList
            return Ok(new { message = "Wishlist actualizada correctamente." });
        }
    }
}
