using E_commerce_Endpoints.DTO.Offer.Request;
using E_commerce_Endpoints.DTO.Offer.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IOfferService
    {
        Task<ServiceResult<OfferDTO>> Add(AddOfferDTO dto);
        Task<ServiceResult<OfferDTO>> Update(UpdateOfferDTO dto);
        Task<ServiceResult<OfferDTO>> GetByID(int id);
        Task<ServiceResult<IEnumerable<OfferDTO>>> GetAll(int? variantId = null, bool? isActive = null); 
        Task<ServiceResult<bool>> Delete(int id);
    }
}
