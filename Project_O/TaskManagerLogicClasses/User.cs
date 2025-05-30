using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YandexDisk;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace TaskManagerLogic.Classes
{
    
    public class User
    {
        // Хэширование текста
        static private string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public string UserName { get; private set; }
        public Dictionary<Group, bool> Groups { get; private set; }
        public User(string UserName, Dictionary<Group, bool> Groups)
        {
            this.UserName = UserName;  
            this.Groups = Groups;
        }
        
        // Скачать данные о пользователях
        static public async Task ActualizeUsersBase()
        {

            YandexDrive drive = new YandexDrive();
            string CurrentFileName = CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager", "users*.csv");
            bool isActual = await drive.CheckFileNameExists("/Users", CurrentFileName);
            if (!isActual)
            {
                if (File.Exists("C:\\ProgramData" + "\\TaskManager\\" + CurrentFileName))
                File.Delete("C:\\ProgramData" + "\\TaskManager\\" + CurrentFileName);
                await drive.DownloadFile($"/Users/{await drive.GetFileNameByStart("user", "/Users")}", "C:\\ProgramData" + "\\TaskManager");
                
            };
        }
        // UserException с кодом ошибки 1 - такого пользователя нет
        // UserException с кодом ошибки 2 - неверный пароль 
        // Для обработки catch (UserException ex) when (ex.ErrorCode == N)

        // Проверка введённой информации о польователе
        static private async Task<bool> CheckUserInfo(string UserName, string Password) 
        {
            await ActualizeUsersBase();
            string[] userdata;
            foreach (string Line in CSVreader.Read("C:\\ProgramData" + "\\TaskManager" + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager", "users*.csv")))
            {
                userdata = Line.Split(";");
                if (userdata[0] == UserName) {
                    if (userdata[1] == Hash(Password))
                    {
                        return true;
                    }
                    else throw new UserException("Неверный пароль", 2);
                }
            }
            throw new UserException("Пользователь не существует", 1);
        }
        //  Составить словарь групп пользователя
        static public async Task<Dictionary<Group, bool>> CreateGroupsList(string UserName)
        {
            await ActualizeUsersBase();
            Dictionary<Group, bool> groups = new Dictionary<Group, bool>();
            var groupNames = CSVreader.ReadStringByColumns($"C:\\ProgramData\\TaskManager\\{CSVreader.GetFileNameByMask("C:\\ProgramData\\TaskManager","users*.csv")}", new string[] {"User"}, new string[] {UserName}).Split(";")[2].Split(",");
            foreach (string group in groupNames)
            {
                if (group != "") {
                    var gr = new Group(group);
                    gr.ActualizeGroupFiles();
                    groups.Add(gr, await gr.IsUserMaster(UserName));
                };
            }
            return groups;
        }
        // Вход в аккаунт
        static public async Task<User?> Login(string UserName, string Password)
        {
            bool isExist = await CheckUserInfo(UserName, Password);
            if (isExist)
            {
                return new User(UserName, await CreateGroupsList(UserName));
            }
            else return null;
        }

        // UserException с кодом ошибки 3 - пользователь с таким ником уже существует
        // Регистрация аккаунта
        static public async Task<User?> Register(string UserName, string Password)
        {
            try
            {
                bool isExist = await CheckUserInfo(UserName, Password);
                throw new UserException("Пользователь с таким именем уже существует", 3);
            }
            catch (UserException ex) when (ex.ErrorCode == 1)
            {
                YandexDrive drive = new YandexDrive();
                CSVreader.AddNewRecord("C:\\ProgramData" + "\\TaskManager" + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager", "users*.csv"), $"{UserName};{Hash(Password)};");
                string oldFileName = await drive.GetFileNameByStart("user", "/Users");
                string newFileName = $"users{TMDateFormatter.ToString(DateTime.Now)}.csv";
                File.Move("C:\\ProgramData" + "\\TaskManager\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + newFileName);
                await drive.UploadFile($"/Users/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + newFileName);
                await drive.DeleteFile($"/Users/{oldFileName}");
                return new User(UserName, await CreateGroupsList(UserName));
            }
            catch (UserException ex) when (ex.ErrorCode == 2)
            {
                throw new UserException("Пользователь с таким именем уже существует", 3);
            }
        }
        // Group Exception с кодом 5 - Группа не существует
        // Group Exception с кодом 6 - Неверный пароль группы
        // Подключить пользователя к группе
        public async Task ConnectToGroup(string GroupName, string Password, bool isMaster = false)
        {
            if (!await Group.isValidGroupName(GroupName)) throw new GroupException("Группа не существует", 5);
            if (!await Group.CheckGroupPassword(GroupName, Password)) throw new GroupException("Неверный пароль группы", 6);
            await ActualizeUsersBase();
            YandexDrive drive = new YandexDrive();
            var UserData = CSVreader.ReadStringByColumns("C:\\ProgramData" + "\\TaskManager" + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager", "users*.csv"), new string[] { "UserName" }, new string[] { UserName }).Split(";");
            if (UserData[2] != "") GroupName = "," + GroupName;
            CSVreader.WriteStringByColumns("C:\\ProgramData" + "\\TaskManager" + "\\" + CSVreader.GetFileNameByMask("C:\\ProgramData" + "\\TaskManager", "users*.csv"), new string[] { "UserName" }, new string[] { UserName }, $"{UserData[0]};{UserData[1]};{UserData[2]+GroupName}");
            string oldFileName = await drive.GetFileNameByStart("user", "/Users");
            string newFileName = $"users{TMDateFormatter.ToString(DateTime.Now)}.csv";
            File.Move("C:\\ProgramData" + "\\TaskManager\\" + oldFileName, "C:\\ProgramData" + "\\TaskManager\\" + newFileName);
            await drive.UploadFile($"/Users/{newFileName}", "C:\\ProgramData" + "\\TaskManager\\" + newFileName);
            await drive.DeleteFile($"/Users/{oldFileName}");
            var group = new Group(GroupName);
            Groups.Add(group, isMaster);
        }

    }
}
