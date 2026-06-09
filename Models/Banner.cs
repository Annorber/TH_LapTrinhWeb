using System.ComponentModel.DataAnnotations;

namespace TH_LTW_Buoi02.Models
{
    public class Banner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên Banner")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? Link { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thứ tự hiển thị")]
        public int Order { get; set; }
    }
}
