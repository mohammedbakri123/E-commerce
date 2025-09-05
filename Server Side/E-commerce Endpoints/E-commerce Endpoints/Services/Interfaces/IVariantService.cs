namespace E_commerce_Endpoints.Services.Interfaces
{
    using global::E_commerce_Endpoints.DTO.Variant.Request;
    using global::E_commerce_Endpoints.DTO.Variant.Response;
    using global::E_commerce_Endpoints.Shared;

    namespace E_commerce_Endpoints.Services.Interfaces
    {
        public interface IVariantService
        {
            Task<ServiceResult<VariantDTO>> Add(AddVariantDTO variantDTO);
            Task<ServiceResult<VariantDTO>> Update(UpdateVariantDTO variantDTO);
            Task<ServiceResult<VariantDTO>> GetByID(int id);
            Task<ServiceResult<IEnumerable<VariantDTO>>> GetAll(int? productId = null, bool? status = null);

            Task<ServiceResult<bool>> Delete(int id);
        }
    }
}
