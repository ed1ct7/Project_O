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
        public Dictionary<DateTime, string[]> scheduleShifts { get; private set; }
        // Group exception с кодом 1 - неверный ключ
        
        // Функция проверки кода
        static public async Task<bool> IsValidKey(string code) 
        {
            YandexDrive drive = new YandexDrive();
            await drive.DownloadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager");
            var lines = CSVreader.ReadStringByNumber("C:\\ProgramData\\TaskManager\\Secret.mdf", 0);
            File.Delete("C:\\ProgramData\\TaskManager\\Secret.mdf");
            return lines.Split(";").Contains(code); ;
        }
        // Использовать код
        private async static Task UseKey(string deleteKey)
        {
            YandexDrive drive = new YandexDrive();
            await drive.DownloadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager");
            CSVreader.EditLine("C:\\ProgramData\\TaskManager\\Secret.mdf", CSVreader.ReadStringByNumber("C:\\ProgramData\\TaskManager\\Secret.mdf", 0).Replace($";{deleteKey}", ""), 0);
            await drive.DeleteFile("/Secret.mdf");
            await drive.UploadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager\\Secret.mdf");
            File.Delete("C:\\ProgramData\\TaskManager\\Secret.mdf");
        }
        // Group exception с кодом 2 - такая группа уже существует
        static public async Task<bool> isValidGroupName(string groupName) {
            YandexDrive drive = new YandexDrive();
            return await drive.CheckFileNameExists("/Groups", groupName);
        }
        static public async Task<bool> CheckGroupPassword(string GroupName, string Password) {
            YandexDrive drive = new YandexDrive();
            Directory.CreateDirectory("C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            await drive.DownloadFile($"/Groups/{GroupName}/{await drive.GetFileNameByStart("settings", $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            return CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" +
                CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"), 0).Split(";")[1] == Password;

        }
        // Создание группы и базовых файлов на диске
        static public async Task<Group?> CreateGroup(string GroupName, string Password, string Author, string Key)
        {
            if (!await IsValidKey(Key)) throw new GroupException("Такого кода нет", 1);
            if (await isValidGroupName(GroupName)) throw new GroupException("Группа уже существует", 2);
            
            string date = TMDateFormatter.ToString(DateTime.Now);
            YandexDrive drive = new YandexDrive();
            await drive.CreateNewFolder($"/Groups/{GroupName}");
            await drive.CreateNewFolder($"/Groups/{GroupName}/Files");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/timetable{date}.csv", "Timetable\n");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/settings{date}.csv", $"Password;{Password}");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/users{date}.csv", $"{Author}\n{Author}");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/tasks{date}.csv", "Name;Subject;Desc;Files;CreateDate;EditDate;DeadlineDate;Priority\n");
            await drive.CreateFileWithContent($"/Groups/{GroupName}/scheduleshifts{date}.csv", "Date;Timetable");
            await UseKey(Key);
            return new Group(GroupName);
        }
        // Проверка на наличие пользователя в группе
        static public async Task<bool> CheckUserInGroup(string GroupName, string UserName)
        {
            YandexDrive drive = new YandexDrive();
            Directory.CreateDirectory("C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            await drive.DownloadFile($"/Groups/{GroupName}/{await drive.GetFileNameByStart("users", $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);

            return CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName, "users*.csv"), 0).Split(";").Contains(UserName);
        }
        // Проверка на то, является пользователь администратором группы
        public async Task<bool> IsUserMaster(string UserName)
        {
            YandexDrive drive = new YandexDrive();

            await drive.DownloadFile($"/Groups/{GroupName}/{drive.GetFileNameByStart("users", $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);

            return CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName, "users*.csv"), 1).Split(";").Contains(UserName);
        }
        public Group(string GroupName)
        {
            this.GroupName = GroupName;
            Directory.CreateDirectory("C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            ActualizeGroupFiles();


        }
        // Функция, скачивающая все данные группы
        public async Task ActualizeGroupFiles()
        {
            YandexDrive drive = new YandexDrive();

            foreach (string filename in new string[] { "settings", "users", "tasks", "timetable", "scheduleshifts" })
            {
                if (File.Exists("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"))) File.Delete("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"{filename}*.csv"));
                await drive.DownloadFile($"/Groups/{GroupName}/{await drive.GetFileNameByStart(filename, $"/Groups/{GroupName}")}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName);
            }
        }
        // Функция добавления пользователя в список группы
        // Group Exception с кодом 3 - пользователь уже есть в группе
        public async Task AddUser(string UserName, bool isMaster = false)
        {
            await ActualizeGroupFiles();
            if (CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" +
                CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"), 0).Split(";").Contains(UserName))
            {
                CSVreader.EditLine("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"),
                CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" +
                CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"), 0) + $";{UserName}", 0);
                if (isMaster) await AddMaster(UserName);
                YandexDrive drive = new YandexDrive();
                string oldFileName = await drive.GetFileNameByStart("users", $"/Groups/{GroupName}");
                string newFileName = $"users{TMDateFormatter.ToString(DateTime.Now)}.csv";
                File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
                await drive.DeleteFile($"/Groups/{GroupName}/{oldFileName}");
                await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            }
            
        }
        // Функция выдачи прав администратора пользователю
        // Group Exception с кодом 4 - пользователь уже является администратором
        public async Task AddMaster(string UserName) 
        {
            await ActualizeGroupFiles();
            if (CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" +
                CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"), 1).Split(";").Contains(UserName))
            {
                CSVreader.EditLine("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"),
                CSVreader.ReadStringByNumber("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" +
                CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"users*.csv"), 1) + $";{UserName}", 0);
                YandexDrive drive = new YandexDrive();
                string oldFileName = await drive.GetFileNameByStart("users", $"/Groups/{GroupName}");
                string newFileName = $"users{TMDateFormatter.ToString(DateTime.Now)}.csv";
                File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
                await drive.DeleteFile($"/Groups/{GroupName}/{oldFileName}");
                await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            }
        }
        // Получение задания, созданного в указанный день
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
        // Загрузка задания на диск
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
        // Обновление задания на диске
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
        // Функция загрузки расписания
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
        // Обновление переменной расписания
        public void UpdateTimeTable()
        {
            var lines = CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "" + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"timetable*.csv"));
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
        // Загрузка списка замен
        public async Task UploadScheduleShifts()
        {
            List<string> newShifts = new List<string>();
            foreach (DateTime key in scheduleShifts.Keys)
            {
                newShifts.Add($"{TMDateFormatter.ToStringWTime(key)};{string.Join(",", scheduleShifts[key])}");
            }
            CSVreader.Write("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "" + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"scheduleshifts*.csv"), newShifts);
            YandexDrive drive = new YandexDrive();
            string oldFileName = await drive.GetFileNameByStart("scheduleshifts", $"/Groups/{GroupName}");
            string newFileName = $"scheduleshifts{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
            await drive.DeleteFile($"/Groups/{GroupName}/{oldFileName}");
            await drive.UploadFile($"/Groups/{GroupName}/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\" + newFileName);
        }
        // Обновление списка замен
        public void UpdateScheduleShifts()
        {
            var lines = CSVreader.Read("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "" + "\\"
                + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager\\" + GroupName + "\\", $"scheduleshifts*.csv"));
            for (int i = 1; i < lines.Count; i++)
            {
                scheduleShifts.Add(TMDateFormatter.ToDateWTime(lines[i].Split(";")[0]).Date, lines[i].Split(";")[1].Split(","));
            }

        }
        // Добавление замены на дату
        public async Task addScheduleShiftAtDate(DateTime Date, string[] NewTimetable)
        {
            scheduleShifts[Date.Date] = NewTimetable;
            UpdateScheduleShifts();
        }
        // Удаление замены на дату
        public async Task deleteScheduleShiftAtDate(DateTime Date)
        {
            scheduleShifts.Remove(Date.Date);
            UpdateScheduleShifts();
        }
        
    }
}
