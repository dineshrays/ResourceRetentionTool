using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RetentionTool.Models
{
    public class CurrentInfoModel
    {
         public int Id { get; set; }
        public Nullable<int> P_Id { get; set; }
        public string Designation { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DOJ { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateOfRelieving { get; set; }
        public string ReportingManager { get; set; }
        public string JobType { get; set; }
        public string DeployedCompanyDetails { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DeployedFromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DeployedToDate { get; set; }
        public string CompanyMailId { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string ModeOfPayment { get; set; }
        public string WorkLocation { get; set; }
        public string Department { get; set; }
        public string Grade { get; set; }
        public Nullable<int> Salary { get; set; }
        public Nullable<int> CTC { get; set; }
    
        public virtual PersonalInfo PersonalInfo { get; set; }
    }
}