using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModels
{
    public class ProductFormViewModel
    {
        public long? ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? RentPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int ConditionPercent { get; set; }

        [Required]
        public long CategoryID { get; set; }

        public string Image { get; set; } // ảnh đại diện

        public IFormFile ImageFile { get; set; } // upload ảnh đại diện

        public List<IFormFile> AdditionalImages { get; set; } // nhiều ảnh phụ
    }
}
