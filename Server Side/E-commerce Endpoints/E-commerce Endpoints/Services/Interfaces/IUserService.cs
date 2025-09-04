using E_commerce_Endpoints.Data.Entities;
using E_commerce_Endpoints.DTO.User.Request;
using E_commerce_Endpoints.DTO.User.Response;
using E_commerce_Endpoints.Shared;

namespace E_commerce_Endpoints.Services.Interfaces
{
    public interface IUserService
    {
        
        public Task<ServiceResult<UserDTO>> AddUserAsync(AddUserDTO userDTO);


        public Task<ServiceResult<IEnumerable<UserDTO>>> GetAllAsync(string? role = null, bool? activeOnly = null);


        public Task<ServiceResult<UserDTO>> GetByIdAsync(int id);

        public Task<ServiceResult<UserDTO>> GetByEmailAsync(string email);

        public Task <ServiceResult<bool>>DeleteByIDAsync(int id);

        public Task<ServiceResult<UserDTO>> UpdateAsync(UpdateUserDTO updateUserDTO);

        public Task<ServiceResult<bool>> ChangePassword(ChangePasswordDTO changePassword);





    }
}
