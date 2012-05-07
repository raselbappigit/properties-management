using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class Profile
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Sur Name")]
        [MaxLength(100)]
        public string SurName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Address")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mobile Number")]
        [MaxLength(50)]
        public string MobileNumber { get; set; }

        [Display(Name = "Profile Thumb Image")]
        public string ThumbImageUrl { get; set; }

        [Display(Name = "Profile Small Image")]
        public string SmallImageUrl { get; set; }

        //one to one relationship with user
        public string UserName { get; set; }
        public User User { get; set; }

    }
}
