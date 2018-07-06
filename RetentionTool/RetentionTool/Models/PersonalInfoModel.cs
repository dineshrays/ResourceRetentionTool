using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class PersonalInfoModel
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string PermanentAddress { get; set; }
        public string CommunicationAddress { get; set; }
        public Nullable<long> Contact { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PanNo { get; set; }
        public Nullable<long> AadharNo { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string  Image { get; set; }
    }
}