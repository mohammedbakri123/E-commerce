using E_commerce_Endpoints.DTO.Variant.Request;
using E_commerce_Endpoints.DTO.Variant.Response;
using E_commerce_Endpoints.Shared;
namespace E_commerce_Endpoints.Services.Interfaces
{
   

   
        public interface IVariantService
        {
            Task<ServiceResult<VariantDTO>> Add(AddVariantDTO variantDTO);
            Task<ServiceResult<VariantDTO>> Update(UpdateVariantDTO variantDTO);
            Task<ServiceResult<VariantDTO>> GetByID(int id);
            Task<ServiceResult<IEnumerable<VariantDTO>>> GetAll(int? productId = null, bool? status = null);

            Task<ServiceResult<bool>> Delete(int id);
            Task<ServiceResult<IEnumerable<VariantDTO>>> GetAllWithQuantityAndPrice(int? productId = null, bool? status = null);
            Task<ServiceResult<VariantDTO>> GetByIDWithPriceAndQuantity(int id);

        }
}

