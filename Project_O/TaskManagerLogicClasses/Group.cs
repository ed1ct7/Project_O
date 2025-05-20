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
    internal class Group
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
            await drive.DownloadFile($"/Groups/{GroupName}/{drive.GetFileNameByStart("users", $"/Groups/{GroupName}")}", Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName);
            
            return CSVreader.ReadStringByColumns(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName, "users*.csv"), new string[] { "Users" }, new string[] { UserName }) != "";
        }
        public Group(string GroupName) 
        { 
            this.GroupName = GroupName; 
            Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName); 
        }
        private async Task ActualizeGroupFiles()
        {
            YandexDrive drive = new YandexDrive();
            
            foreach (string filename in new string[] {"settings", "users", "tasks"})
            {
                File.Delete(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"));
                await drive.DownloadFile($"/Groups/{GroupName}/{drive.GetFileNameByStart(filename, $"/Groups/{GroupName}")}", Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Downloads") + "\\TaskManager\\" + GroupName);
            }
        }
    }
}
