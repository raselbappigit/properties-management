using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "Supplier Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Supplier Address is required")]
        [Display(Name = "Supplier Address")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Supplier Phone is required")]
        [Display(Name = "Supplier Phone")]
        [MaxLength(200)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Supplier Mobile is required")]
        [Display(Name = "Supplier Mobile")]
        [MaxLength(200)]
        public string Mobile { get; set; }

        [Display(Name = "Supplier Fax")]
        [MaxLength(200)]
        public string Fax { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Supplier Email is required")]
        [Display(Name = "Supplier Email")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Person Name is required")]
        [Display(Name = "Contact Person Name")]
        [MaxLength(200)]
        public string ContactPerson { get; set; }

        [Display(Name = "Person Designation")]
        [MaxLength(200)]
        public string ContactDesignation { get; set; }

    }
}
