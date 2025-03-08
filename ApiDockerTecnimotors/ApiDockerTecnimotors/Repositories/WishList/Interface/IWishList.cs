using ApiDockerTecnimotors.Repositories.WishList.Models;

namespace ApiDockerTecnimotors.Repositories.WishList.Interface
{
    public interface IWishList
    {
        public Task<IEnumerable<TlModels>> ListadoWishList();
        public Task<IEnumerable<TlModels>> ListadoWishList(string uuidCliente);
        public Task<bool> RegisterWishList(TrModels Trmodels);
        public Task<string> GetNextOrderNumber(string uuidCliente);
        public Task<bool> RemoveFromWishlist(string uuidCliente, string codigo);
        public Task<TlModels> GetWishlistItemByCode(string uuidCliente, string codigo);
        public Task<bool> UpdateWishlistItem(TlModels item);
    }
}
