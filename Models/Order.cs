using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TH_LTW_Buoi02.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Vui lòng nhập tên người đặt")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? VoucherCode { get; set; }

        public string PaymentMethod { get; set; } = "Tiền mặt";

        public string OrderStatus { get; set; } = "Chờ xử lý";

        public string Notes { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
