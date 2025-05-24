using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using System.IO;
using YandexDisk;

namespace TaskManagerLogic.Classes
{
    public class Group
    {
        public string GroupName;
        static public async Task<bool> CheckCode(string code) { return true; }
        static public async Task<Group> CreateGroup(string GroupName)
        { 
            string date = TMDateFormatter.ToString(DateTime.Now);
            YandexDrive drive = new YandexDrive();
            await drive.CreateNewFolder($"/Groups/{GroupName}");
            await drive.CreateNewFolder($"/Groups/{GroupName}/Files");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/settings{date}.csv", "Name;Password;Subjects;Timetable\n");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/users{date}.csv", "Users;Admins\n");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/tasks{date}.csv", "Name;Subject;Desc;Files;CreateDate;DeadlineDate;Priority\n");
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
        }
        private async Task ActualizeGroupFiles()
        {
            YandexDrive drive = new YandexDrive();
            
            foreach (string filename in new string[] {"settings", "users", "tasks"})
            {
                if (File.Exists("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"))) File.Delete("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"));
                await drive.DownloadFile($"/Groups/{GroupName}/{drive.GetFileNameByStart(filename, $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            }
        }
        public async Task<SubjectTask?> GetTaskAtDate(string Subject, DateTime date)
        {
            await ActualizeGroupFiles();
            var Lines = CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"));
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i].Split(";");
            }
            return null;

        }
        public async Task<SubjectTask> UploadTask(string Name, string Subject, string Description, List<string> Files, DateTime DeadlineDate, double PriorityCoef)
        {
            await ActualizeGroupFiles();
            CSVreader.AddNewRecord("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" 
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"), 
                $"{Name};{Subject};{Description};{string.Join(',')};{TMDateFormatter.ToString(DateTime.Now)};{TMDateFormatter.ToString(DateTime.Now)};{TMDateFormatter.ToString(DeadlineDate)};{PriorityCoef}");
            YandexDrive drive = new YandexDrive();
            string oldFileName = await drive.GetFileNameByStart("tasks", $"/Groups/{GroupName}");
            string newFileName = $"users{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.UploadFile($"/Groups/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.DeleteFile($"/Groups/{oldFileName}");
            await drive.UploadFile($"/Groups/{GroupName}/{newFileName}","C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            return new SubjectTask(Name, Subject, Description, Files, DateTime.Now, DateTime.Now, DeadlineDate, PriorityCoef);
        }

        public async Task<SubjectTask> UpdateTask(string Name, string Subject, string Description, List<string> Files, DateTime CreateDate, DateTime DeadlineDate, double PriorityCoef)
        {
            await ActualizeGroupFiles();
            var lines =  CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"));
            int n = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Split(";")[1] == Subject && lines[i].Split(";")[4] == TMDateFormatter.ToString(CreateDate))
                {
                    n = i; break;
                }
            }
            
            CSVreader.EditLine("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"tasks*.csv"),
                $"{Name};{Subject};{Description};{string.Join(',')};{TMDateFormatter.ToString(CreateDate)};{TMDateFormatter.ToStringWTime(DateTime.Now)};{TMDateFormatter.ToString(DeadlineDate)};{PriorityCoef}", n);
            YandexDrive drive = new YandexDrive();
            string oldFileName = await drive.GetFileNameByStart("tasks", $"/Groups/{GroupName}");
            string newFileName = $"users{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.UploadFile($"/Groups/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.DeleteFile($"/Groups/{oldFileName}");
            await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            return new SubjectTask(Name, Subject, Description, Files, CreateDate, DateTime.Now, DeadlineDate, PriorityCoef);
        }
    }
}
