using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Category.Request
{
    public class UpdateCategoryDTO
    {
        [Required]
        public int id { set; get; }
        [Required]
        public string Name { set; get; }

    }
}
