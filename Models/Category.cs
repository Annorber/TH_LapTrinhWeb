using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TH_LTW_Buoi02.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public List<Product>? Products { get; set; }
    }
}
