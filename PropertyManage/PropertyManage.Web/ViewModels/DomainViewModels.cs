using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Web
{
    public class UserRoleModel
    {
        public string UserName { get; set; }

        public ICollection<AssignRoleModel> AssignRoleModels { get; set; }
    }

    public class AssignRoleModel
    {
        public AssignRoleModel()
        {
            Assigned = false;
        }

        public string RoleName { get; set; }

        public bool Assigned { get; set; }
    }

    public class UnitValueViewModel
    {
        public int UnitValueId { get; set; }

        [Required(ErrorMessage = "Unit value name is required")]
        [Display(Name = "Unit of vale name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Unit of vale note")]
        public string Note { get; set; }

        public int UnitTypeId { get; set; }

        public IEnumerable<SelectUnitType> SelectUnitTypes { get; set; }
    }

    public class SelectUnitType
    {
        public int UnitTypeId { get; set; }
        public string Name { get; set; }
    }

}