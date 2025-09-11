namespace E_commerce_Endpoints.DTO.Favorite.Response
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int VariantId { get; set; }
        public string? VariantName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
