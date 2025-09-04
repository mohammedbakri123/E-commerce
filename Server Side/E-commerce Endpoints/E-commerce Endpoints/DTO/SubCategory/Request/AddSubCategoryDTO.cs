using System.ComponentModel.DataAnnotations;

namespace E_commerce_Endpoints.DTO.Category.Request
{
    public class AddSubCategoryDTO
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string Name { set; get; }
    }
}
