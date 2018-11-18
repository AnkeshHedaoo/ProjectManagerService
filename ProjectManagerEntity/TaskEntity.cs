using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerEntity
{
    public class TaskEntity
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public int Priority { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string TaskStatus { get; set; }
    }
}
