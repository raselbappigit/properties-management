using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}