using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Favorite.Request
{
    public class AddFavoriteDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int VariantId { get; set; }
    }
}
