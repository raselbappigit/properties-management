using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class ProjectBlock
    {
        [Key]
        public int ProjectBlockId { get; set; }

        [Display(Name = "Project Blok Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Project Blok Description")]
        public string Description { get; set; }

        [Display(Name = "Project Block Size")]
        [MaxLength(200)]
        public string Size { get; set; }

        //one to many relationship with project
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public virtual ICollection<ProjectBlock> ProjectBlocks { get; set; }
    }
}
