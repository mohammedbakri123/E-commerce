namespace E_commerce_Endpoints.DTO.Authentication.Response
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }

    }
}
