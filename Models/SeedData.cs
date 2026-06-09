using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TH_LTW_Buoi02.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Company));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
            }

            var adminEmail = "dohuyan.dmcl@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Quản Trị Viên",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Dohuyan@1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                }
            }

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Look for any categories.
                if (context.Categories.Any())
                {
                    return;   // DB has already been seeded
                }

                var categories = new Category[]
                {
                    new Category { Name = "Điện thoại" },
                    new Category { Name = "Laptop" },
                    new Category { Name = "Tai nghe" },
                    new Category { Name = "Loa" },
                    new Category { Name = "Đồng hồ" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                var dienThoai = categories.First(c => c.Name == "Điện thoại");
                var laptop = categories.First(c => c.Name == "Laptop");
                var taiNghe = categories.First(c => c.Name == "Tai nghe");
                var loa = categories.First(c => c.Name == "Loa");
                var dongHo = categories.First(c => c.Name == "Đồng hồ");

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "iPhone 15 Pro Max 256GB - Chính hãng VN/A",
                        Price = 29490000,
                        Description = "iPhone 15 Pro Max là dòng điện thoại cao cấp nhất của Apple với khung viền titanium siêu nhẹ, chip A17 Pro mạnh mẽ và cổng USB-C tiện lợi.",
                        ImageUrl = "/images/iphone_15_pro.png",
                        CategoryId = dienThoai.Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { Url = "/images/samsung_s24_ultra.png" },
                            new ProductImage { Url = "/images/apple_watch_ultra.png" },
                            new ProductImage { Url = "/images/sony_headphones.png" },
                            new ProductImage { Url = "/images/marshall_speaker.png" },
                            new ProductImage { Url = "/images/airpods_pro.png" }
                        }
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S24 Ultra AI 12GB/256GB",
                        Price = 31990000,
                        Description = "Samsung Galaxy S24 Ultra nổi bật với các tính năng Galaxy AI thông minh, camera 200MP zoom siêu phân giải và bút S Pen tích hợp.",
                        ImageUrl = "/images/samsung_s24_ultra.png",
                        CategoryId = dienThoai.Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { Url = "/images/iphone_15_pro.png" },
                            new ProductImage { Url = "/images/apple_watch_ultra.png" },
                            new ProductImage { Url = "/images/airpods_pro.png" }
                        }
                    },
                    new Product
                    {
                        Name = "Tai nghe Sony WH-1000XM5 Chống ồn chủ động",
                        Price = 8490000,
                        Description = "Tai nghe chụp tai chống ồn đỉnh cao từ Sony với màng loa chất lượng cao, thời lượng pin 30 giờ và âm thanh vô cùng sống động.",
                        ImageUrl = "/images/sony_headphones.png",
                        CategoryId = taiNghe.Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { Url = "/images/airpods_pro.png" },
                            new ProductImage { Url = "/images/marshall_speaker.png" }
                        }
                    },
                    new Product
                    {
                        Name = "Apple Watch Ultra 2 GPS + Cellular 49mm Titanium",
                        Price = 21490000,
                        Description = "Đồng hồ thông minh thể thao chuyên nghiệp với màn hình siêu sáng, thời lượng pin lên đến 36 giờ và thiết kế titanium bền bỉ.",
                        ImageUrl = "/images/apple_watch_ultra.png",
                        CategoryId = dongHo.Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { Url = "/images/airpods_pro.png" },
                            new ProductImage { Url = "/images/iphone_15_pro.png" }
                        }
                    },
                    new Product
                    {
                        Name = "Loa Bluetooth Marshall Stanmore III Chính hãng",
                        Price = 9290000,
                        Description = "Loa bluetooth gia đình cổ điển từ thương hiệu Marshall với âm thanh vang dội, kết nối ổn định và thiết kế sang trọng đặt trong phòng.",
                        ImageUrl = "/images/marshall_speaker.png",
                        CategoryId = loa.Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { Url = "/images/sony_headphones.png" }
                        }
                    },
                    new Product
                    {
                        Name = "Tai nghe Apple AirPods Pro Gen 2 - MagSafe",
                        Price = 5490000,
                        Description = "Tai nghe không dây AirPods Pro thế hệ 2 với chip H2 cải thiện khả năng chống ồn, âm thanh không gian cá nhân hóa và hộp sạc MagSafe.",
                        ImageUrl = "/images/airpods_pro.png",
                        CategoryId = taiNghe.Id,
                        Images = new List<ProductImage>
                        {
                            new ProductImage { Url = "/images/sony_headphones.png" },
                            new ProductImage { Url = "/images/apple_watch_ultra.png" }
                        }
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
