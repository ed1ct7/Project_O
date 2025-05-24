using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerLogic.Classes
{
    public class SubjectTask
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public List<string> Files { get; set; }
        public double PriorityCoef { get; set; }

        public SubjectTask(string Name, string Subject, string Description, List<string> Files, DateTime CreateDate, DateTime EditDate, DateTime DeadlineDate, double PriorityCoef)
        {
            this.Name = Name;
            this.Subject = Subject;
            this.Description = Description;
            this.CreateDate = CreateDate;
            this.EditDate = EditDate;
            this.DeadlineDate = DeadlineDate;
            this.Files = Files;
            this.PriorityCoef = PriorityCoef;
        }
        public double CalcPriority()
        {
            //Double.Clamp();
            //Double.Lerp();
            return 1;
        }
    }
}
