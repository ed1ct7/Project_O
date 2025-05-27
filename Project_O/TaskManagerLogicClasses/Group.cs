using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using System.IO;
using YandexDisk;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Media;

namespace TaskManagerLogic.Classes
{
    public class Group
    {
        public string GroupName;
        public bool isMaster = true;
        public string[][] Timetable = new string[][]
            {
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  },
                new string[] {  }
            };
        public List<string> UniqueLessons = new List<string>();

        static public async Task<bool> CheckCode(string code) {
            return true;

        }
        static public async Task<bool> isValidGroupName(string groupName) {
            YandexDrive drive = new YandexDrive();
            return await drive.CheckFileNameExists("/Groups", groupName);
        }
        static public async Task<Group?> CreateGroup(string GroupName, string Password)
        {
            
            string date = TMDateFormatter.ToString(DateTime.Now);
            YandexDrive drive = new YandexDrive();
            await drive.CreateNewFolder($"/Groups/{GroupName}");
            await drive.CreateNewFolder($"/Groups/{GroupName}/Files");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/timetable{date}.csv", "Timetable\n");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/settings{date}.csv", $"Password\n{Password}");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/users{date}.csv", "Users;Admins\n");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/tasks{date}.csv", "Name;Subject;Desc;Files;CreateDate;EditDate;DeadlineDate;Priority\n");
            return new Group(GroupName);
            
            

        }
        static public async Task<bool> CheckUserInGroup(string GroupName, string UserName)
        {
            YandexDrive drive = new YandexDrive();

            await drive.DownloadFile($"/Groups/{GroupName}/{drive.GetFileNameByStart("users", $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);

            return CSVreader.ReadStringByColumns("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName, "users*.csv"), new string[] { "Users" }, new string[] { UserName }).Split(";")[2].Split(",").Contains(GroupName);
        }
        public Group(string GroupName)
        {
            this.GroupName = GroupName;
            Directory.CreateDirectory("C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            UpdateTimeTable(); Trace.WriteLine("this.UniqueLessons");


        }
        public async Task ActualizeGroupFiles()
        {
            YandexDrive drive = new YandexDrive();

            foreach (string filename in new string[] { "settings", "users", "tasks" })
            {
                if (File.Exists("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"))) File.Delete("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"));
                await drive.DownloadFile($"/Groups/{GroupName}/{await drive.GetFileNameByStart(filename, $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            }
        }
        public SubjectTask? GetTaskCreatedAtDate(string Subject, DateTime date)
        {
            var Lines = CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"));
            for (int i = 1; i < Lines.Count; i++)
            {
                var line = Lines[i].Split(";");
                if (line[1] == Subject && TMDateFormatter.ToDate(line[4]).Date == date.Date) 
                    return new SubjectTask(line[0], line[1], line[2], line[3].Split(',').ToList(), TMDateFormatter.ToDate(line[4]), TMDateFormatter.ToDate(line[5]), TMDateFormatter.ToDate(line[6]), Convert.ToDouble(line[7]));

            }
            return null;

        }
        public async Task<SubjectTask> UploadTask(string Name, string Subject, string Description, List<string> Files, DateTime CreateDate, DateTime DeadlineDate, double PriorityCoef)
        {
            await ActualizeGroupFiles();
            CSVreader.AddNewRecord("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"),
                $"{Name};{Subject};{Description};{string.Join(',', Files)};{TMDateFormatter.ToString(CreateDate)};{TMDateFormatter.ToString(DateTime.Now)};{TMDateFormatter.ToString(DeadlineDate)};{PriorityCoef}");
            YandexDrive drive = new YandexDrive();
            string oldFileName = await drive.GetFileNameByStart("tasks", $"/Groups/{GroupName}");
            string newFileName = $"tasks{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.DeleteFile($"/Groups/{GroupName}/{oldFileName}");
            await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            return new SubjectTask(Name, Subject, Description, Files, CreateDate, DateTime.Now, DeadlineDate, PriorityCoef);
        }

        public async Task<SubjectTask> UpdateTask(string Name, string Subject, string Description, List<string> Files, DateTime CreateDate, DateTime DeadlineDate, double PriorityCoef)
        {
            await ActualizeGroupFiles();
            var lines = CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"));
            int n = 0;
            for (int i = 1; i < lines.Count; i++)
            {
                Trace.WriteLine($"{lines[i].Split(";")[1]} == {Subject} && {TMDateFormatter.ToDate(lines[i].Split(";")[4]).Date} == {CreateDate.Date}");
                if (lines[i].Split(";")[1] == Subject && TMDateFormatter.ToDate(lines[i].Split(";")[4]).Date == CreateDate.Date)
                {
                    n = i; break;
                }
            }
            CSVreader.EditLine("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"),
                $"{Name};{Subject};{Description};{string.Join(',')};{TMDateFormatter.ToString(CreateDate)};{TMDateFormatter.ToString(DateTime.Now)};{TMDateFormatter.ToString(DeadlineDate)};{PriorityCoef}", n);
            YandexDrive drive = new YandexDrive();
            string oldFileName = await drive.GetFileNameByStart("tasks", $"/Groups/{GroupName}");
            string newFileName = $"tasks{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);

            await drive.DeleteFile($"/Groups/{GroupName}/{oldFileName}");
            await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            return new SubjectTask(Name, Subject, Description, Files, CreateDate, DateTime.Now, DeadlineDate, PriorityCoef);
        }
        public async Task UploadTimeTable()
        {
            List<string> newTimetable = new List<string>();
            for (int i = 0; i < 14; i++)
            {
                newTimetable.Add(string.Join(";", Timetable[i]));
            }
            CSVreader.Write("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "" + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"timetable*.csv"), newTimetable);
            YandexDrive drive = new YandexDrive();
            string oldFileName = await drive.GetFileNameByStart("timetable", $"/Groups/{GroupName}");
            string newFileName = $"timetable{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.DeleteFile($"/Groups/{GroupName}/{oldFileName}");
            await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);


        }
        public void UpdateTimeTable()
        {

            var lines = CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "" + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"timetable*.csv"));
            this.UniqueLessons = new List<string>();
            for (int i = 0; i < 14; i++) {
                if (lines[i] != "")
                {
                    Timetable[i] = lines[i].Split(';');
                    foreach (string line in Timetable[i])
                    {
                        if (!this.UniqueLessons.Contains(line))
                        {
                            UniqueLessons.Add(line);
                        }
                    }
                    
                }

            }
            this.UniqueLessons.Sort();
            
           
        }
    }
}
