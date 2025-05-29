using TaskManagerLogic.Classes;
internal class Program
{
    private async static Task AddNewKey(string newKey)
    {
        YandexDrive drive = new YandexDrive();
        await drive.DownloadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager");
        CSVreader.EditLine("C:\\ProgramData\\TaskManager\\Secret.mdf", CSVreader.ReadStringByNumber("C:\\ProgramData\\TaskManager\\Secret.mdf", 0) + $";{newKey}", 0);
        await drive.DeleteFile("/Secret.mdf");
        await drive.UploadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager\\Secret.mdf");
        File.Delete("C:\\ProgramData\\TaskManager\\Secret.mdf");
    }
    private async static Task GetKeys()
    {
        YandexDrive drive = new YandexDrive();
        await drive.DownloadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager");
        var lines = CSVreader.ReadStringByNumber("C:\\ProgramData\\TaskManager\\Secret.mdf", 0);
        Console.WriteLine(lines);
        File.Delete("C:\\ProgramData\\TaskManager\\Secret.mdf");
    }
    private async static Task DeleteKey(string deleteKey)
    {
        YandexDrive drive = new YandexDrive();
        await drive.DownloadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager");
        CSVreader.EditLine("C:\\ProgramData\\TaskManager\\Secret.mdf", CSVreader.ReadStringByNumber("C:\\ProgramData\\TaskManager\\Secret.mdf", 0).Replace($";{deleteKey}", ""), 0);
        await drive.DeleteFile("/Secret.mdf");
        await drive.UploadFile("/Secret.mdf", "C:\\ProgramData\\TaskManager\\Secret.mdf");
        File.Delete("C:\\ProgramData\\TaskManager\\Secret.mdf");
    }

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в генератор ключей для Project_O");
        bool isTrue = true;
        while (isTrue) { 
            Console.WriteLine("1 - Добавить ключ\n2 - Сгенерировать ключ\n3 - Получить все существующие ключи\n4 - Удалить ключ\n0 - Выход");
            Console.Write("Выбор: ");
            int choose = Convert.ToInt32(Console.ReadLine());
            switch (choose)
            {
                case 1:
                    {
                        Console.Write("Ввод нового ключа: ");
                        await AddNewKey(Console.ReadLine());
                        break;
                    }
                case 2:
                    {
                        string pass = ""; 
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        var random = new Random();
                        for (int i = 0; i < 20; i++)
                        {
                            pass += chars[random.Next(chars.Length)];
                        }
                        Console.WriteLine($"Сгенерированный пароль: {pass}");
                        break;
                    }
                case 3: 
                    {
                        await GetKeys();
                        break;
                    }
                case 4:
                    {
                        Console.Write("Ввод ключа для удаления: ");
                        await DeleteKey(Console.ReadLine());
                        break;
                    }
                case 0:
                    {
                        isTrue = false;
                        break;
                    }
            }
        }
        
    }
}