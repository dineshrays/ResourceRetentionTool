using RetentionTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetentionTool.ViewModel
{
    public class TrainingViewModel
    {
        public List<Training> Training { get; set; }
        public Training TrainingVm { get; set; }

        public List<TrainingDet> TrainingDets { get; set; }
        public TrainingDet TrainingDetVm { get; set; }

    }
}