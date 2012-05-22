using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PropertyManage.Domain
{
    public class BlockPlot
    {
        [Key]
        public int BlockPlotId { get; set; }

        [Display(Name = "Block Plot Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Block Plot Description")]
        public string Description { get; set; }

        [Display(Name = "Block Plot Size")]
        [MaxLength(200)]
        public string Size { get; set; }

        //one to many relationship with ProjectBlock
        public int ProjectBlockId { get; set; }
        [ForeignKey("ProjectBlockId")]
        public virtual ProjectBlock ProjectBlock { get; set; }
    }
}
