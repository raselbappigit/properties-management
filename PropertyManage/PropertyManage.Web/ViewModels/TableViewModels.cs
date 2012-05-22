using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyManage.Web
{
    public class UserTableModels
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string CreateDate { get; set; }
    }

    public class UnitTypeTableModels
    {
        public string UnitValueId { get; set; }
        public string Name { get; set; }
    }

    public class UnitValueTableModels
    {
        public string UnitValueId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string UnitTypeName { get; set; }
    }

    public class ProjectTableModels
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string EstimatedArea { get; set; }
        public string UnitValueName { get; set; }
        public string DateCreated { get; set; }
    }

    public class SupplierTableModels
    {
        public string SupplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
    }

    public class CategoryTableModels
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
    }

    public class ProductTableModels
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string MainCost { get; set; }
        public string OtherCost { get; set; }
        public string CategoryName { get; set; }
        public string ProjectName { get; set; }
        public string UnitValueName { get; set; }
    }
}