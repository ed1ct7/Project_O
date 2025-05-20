using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerLogic.Classes
{
    internal class SubjectTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public List<string> Files { get; set; }
        public double PriorityCoef { get; set; }

        public SubjectTask(string Name, string Description, string Type, DateTime CreateDate, DateTime DeadlineDate, List<string> Files, double PriorityCoef)
        {
            this.Name = Name;   
            this.Description = Description;
            this.Type = Type;
            this.CreateDate = CreateDate;
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
