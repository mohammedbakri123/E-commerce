namespace E_commerce_Endpoints.DTO.Admin.Response
{
    public class AdminDTO
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string? AdminLastName { get; set; }
        public int? PermissionsValue { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? UserName { get; set; } 
    }
}
