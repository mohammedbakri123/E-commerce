    using E_commerce_Endpoints.DTO.Favorite.Request;
    using E_commerce_Endpoints.DTO.Favorite.Response;
    using E_commerce_Endpoints.Shared;

    namespace E_commerce_Endpoints.Services.Interfaces
    {
        public interface IFavoriteService
        {
            Task<ServiceResult<FavoriteDTO>> Add(AddFavoriteDTO dto);
            Task<ServiceResult<FavoriteDTO>> Update(UpdateFavoriteDTO dto);
            Task<ServiceResult<FavoriteDTO>> GetByID(int id);
            Task<ServiceResult<IEnumerable<FavoriteDTO>>> GetAll(int? userId = null, int? variantId = null);
            Task<ServiceResult<bool>> Delete(int id);
        }
    }

