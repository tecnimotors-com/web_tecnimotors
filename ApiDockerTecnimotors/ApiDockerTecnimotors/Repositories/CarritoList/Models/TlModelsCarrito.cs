namespace ApiDockerTecnimotors.Repositories.CarritoList.Models
{
    public class TlModelsCarrito
    {
        public int Idcarritocotizacion { get; set; }
        public string? Uuidcliente { get; set; }
        public string? Ordernshopping { get; set; }
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
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Vendor { get; set; }
        public string? Color { get; set; }
        public string? Pathimagen { get; set; }
        public string? Estado { get; set; }
        public string? Fecharegistro { get; set; }
        public int Cantidad { get; set; }
        public int Quantity { get; set; }
    }

    public class TrModelsCarrito
    {
        public int Idcarritocotizacion { get; set; }
        public string? Uuidcliente { get; set; }
        public string? Ordernshopping { get; set; }
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
        public string? Sku { get; set; }
        public string? Producto { get; set; }
        public string? Vendor { get; set; }
        public string? Color { get; set; }
        public string? Pathimagen { get; set; }
        public string? Estado { get; set; }
        public DateTime? Fecharegistro { get; set; }
        public int Cantidad { get; set; }
        public int Quantity { get; set; }
    }

    public class TrCotizacionList
    {
        public List<TlModelsCarrito>? ListCarrito { get; set; }
        public string? Uuidcliente { get; set; }
        public string? Quote { get; set; }
    }
    public class TrtrialCoti
    {
        public string? Uuidcliente { get; set; }
    }

    public class RemoveFromCarritoListRequest
    {
        public string? Uuidcliente { get; set; }
        public string? Codigo { get; set; }

    }

    public class UpdateFromCarritoModal
    {
        public string? Uuidcliente { get; set; }
        public string? Codigo { get; set; }
        public int Cantidad { get; set; }
    }

    public class ListCarritoRegister
    {
        public List<TlModelsCarrito>? ListCarritoLogin { get; set; }
        public string? Uuidcliente { get; set; }
        public string? Quote { get; set; }
    }

    public class TrUuidCoti
    {
        public string? Uuidcliente { get; set; }
    }

    public class ListCarrito
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



    public class TrLoginCarrito
    {
        public List<TlModelsCarrito>? ListCarrito { get; set; }
        public string? Uuidcliente { get; set; }
    }
}
