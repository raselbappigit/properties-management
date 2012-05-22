using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public  class UnitValue
    {
        [Key]
        public int UnitValueId { get; set; }

        [Required(ErrorMessage = "Unit value name is required")]
        [Display(Name = "Unit Of Value")]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Note { get; set; }

        //one to many relationship with unit type
        [Required(ErrorMessage = "Unit type name is required")]
        public int UnitTypeId { get; set; }
        [ForeignKey("UnitTypeId")]
        public virtual UnitType UnitType { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

    }
}
