namespace E_commerce_Endpoints.DTO.User.Response
{
    public class UserDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? Status { get; set; }

        public string Role { get; set; }

    }
}
