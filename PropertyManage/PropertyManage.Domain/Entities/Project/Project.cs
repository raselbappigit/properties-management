using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class Project
    {
        public Project()
        {
            DateCreated = DateTime.Now;
        }

        [Key]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        [Display(Name = "Project Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Project location is required")]
        [Display(Name = "Project Location")]
        [MaxLength(200)]
        public string Location { get; set; }

        [Display(Name = "Project Description")]
        public string Description { get; set; }

        [Display(Name = "Project Estimated Area")]
        [MaxLength(200)]
        public string EstimatedArea { get; set; }

        [Display(Name = "Create Date")]
        public DateTime DateCreated { get; set; }

        //one to many relationship with unit value
        [Required(ErrorMessage = "Project unit is required")]
        public int UnitValueId { get; set; }
        [ForeignKey("UnitValueId")]
        public virtual UnitValue UnitValue { get; set; }

        public virtual ICollection<ProjectBlock> ProjectBlocks { get; set; }

    }
}
