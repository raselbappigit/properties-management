using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class UnitType
    {
        [Key]
        public int UnitTypeId { get; set; }

        [Required(ErrorMessage = "Unit type name is required")]
        [Display(Name = "Unit Of Type")]
        [MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<UnitValue> UnitValues { get; set; }
    }
}
