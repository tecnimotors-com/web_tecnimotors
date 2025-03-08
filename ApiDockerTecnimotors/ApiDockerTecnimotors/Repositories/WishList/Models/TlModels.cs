namespace ApiDockerTecnimotors.Repositories.WishList.Models
{
    public class TlModels
    {
        public int Idwishlish { get; set; }
        public string? Uuidcliente { get; set; }
        public string? Ordenwishlist { get; set; }
        public string? Descripcion { get; set; }
        public string? Unidad { get; set; }
        public string? Categoria { get; set; }
        public string? Marca { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Medida { get; set; }
        public string? Modelo { get; set; }
        public string? Medidaestandarizado { get; set; }
        public string? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Familia { get; set; }
        public string? Subfamilia { get; set; }
        public string? Tipo { get; set; }
        public string? Cantidad { get; set; }
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Vendor { get; set; }
        public string? Quantity { get; set; }
        public string? Color { get; set; }
        public string? Pathimagen { get; set; }
        public string? Estado { get; set; }
    }
    public class TrModels
    {
        public int Idwishlish { get; set; }
        public string? Uuidcliente { get; set; }
        public string? Ordenwishlist { get; set; }
        public string? Descripcion { get; set; }
        public string? Unidad { get; set; }
        public string? Categoria { get; set; }
        public string? Marca { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Medida { get; set; }
        public string? Modelo { get; set; }
        public string? Medidaestandarizado { get; set; }
        public string? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Familia { get; set; }
        public string? Subfamilia { get; set; }
        public string? Tipo { get; set; }
        public int Cantidad { get; set; }
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Vendor { get; set; }
        public int Quantity { get; set; }
        public string? Color { get; set; }
        public string? Pathimagen { get; set; }
        public string? Estado { get; set; }
        public DateTime? Fecharegistro { get; set; }
    }

    public class Trtrial
    {
        public string? Uuidcliente { get; set; }
    }
    public class RemoveFromWishlistRequest
    {
        public string? Uuidcliente { get; set; }
        public string? Codigo { get; set; }
    }

    public class ListWishRegister
    {
        public List<ListWish>? ListWishLogin { get; set; }
        public string? Uuidcliente { get; set; }
    }
    public class TrUuid
    {
        public string? Uuidcliente { get; set; }
    }
    public class ListWish
    {
        public string? Descripcion { get; set; }
        public string? Unidad { get; set; }
        public string? Categoria { get; set; }
        public string? Marca { get; set; }
        public string? Marcaoriginal { get; set; }
        public string? Medida { get; set; }
        public string? Modelo { get; set; }
        public string? Medidaestandarizado { get; set; }
        public string? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Familia { get; set; }
        public string? Subfamilia { get; set; }
        public string? Tipo { get; set; }
        public int Cantidad { get; set; }
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Vendor { get; set; }
        public int Quantity { get; set; }
        public string? Color { get; set; }
        public string? Pathimagen { get; set; }
    }

   }
