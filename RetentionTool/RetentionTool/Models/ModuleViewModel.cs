using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetentionTool.Models
{
    public class ModuleViewModel
    {
        public int Id { get; set; }
        public Nullable<long> Commonfield_Id { get; set; }
        public Nullable<long> Skill_Id { get; set; }
        // [Required]
        public string ModuleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        
        public IEnumerable<SelectListItem> CommonField { get; set; }
        public Nullable<long> SelectedCommonFields { get; set; }
        public IEnumerable<SelectListItem> Skill { get; set; }
        public Nullable<long> SelectedSkills { get; set; }

        public string CommonFieldName { get; set; }
        public string SkillName { get; set; }


        public int ModuleDetId { get; set; }
        public Nullable<int> Module_Id { get; set; }
        public string Topics { get; set; }
        public Nullable<System.TimeSpan> HoursRequired { get; set; }

    }



}