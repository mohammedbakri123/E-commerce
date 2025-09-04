using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Category.Request
{
    public class AddCategoryDTO
    {
        [Required]
        public string Name { set; get; }
    }
}
