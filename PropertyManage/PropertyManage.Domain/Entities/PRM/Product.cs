using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Land name is required")]
        [Display(Name = "Land Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Land Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Land cost is required")]
        [Display(Name = "Land Cost")]
        public decimal MainCost { get; set; }

        [Display(Name = "Land Other Cost")]
        public decimal? OtherCost { get; set; }

        //one to many relationship with category
        [Required(ErrorMessage = "Land category is required")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        //one to many relationship with project
        [Required(ErrorMessage = "Land project is required")]
        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}
