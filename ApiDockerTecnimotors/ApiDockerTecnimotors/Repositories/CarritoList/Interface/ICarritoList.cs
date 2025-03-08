using ApiDockerTecnimotors.Repositories.CarritoList.Models;

namespace ApiDockerTecnimotors.Repositories.CarritoList.Interface
{
    public interface ICarritoList
    {
        public Task<IEnumerable<TlModelsCarrito>> ListadoCarritoList();
        public Task<IEnumerable<TlModelsCarrito>> ListadoCarritoList(string uuidCliente);
        public Task<TlModelsCarrito> GetCarritoListItemByCode(string uuidCliente, string codigo);
        public Task<bool> UpdateCarritoListItem(TlModelsCarrito item);
        public Task<bool> RemoveFromCarritoList(string uuidcliente, string codigo);
        public Task<bool> UpdateCantidadCarrito(string uuidcliente, string codigo, int cantidad);
        public Task<bool> RegistrarCarritoList(TrModelsCarrito Trmodels);
        public Task<bool> UpdateCarritoCotizacionItem(TlModelsCarrito item);
        public Task<TlModelsCarrito> GetCarritoListCotizacionByCode(string uuidCliente, string codigo);
        public Task<TlModelsCarrito> CotizacionRegistrer(string uuidCliente, string codigo);
        public Task<bool> UpdateCotizadorRegister(TlModelsCarrito item);
    }
}
