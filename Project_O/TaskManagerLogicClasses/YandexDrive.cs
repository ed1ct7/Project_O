using System;
using System.Net.Http.Headers;
using YandexDisk;
using YandexDisk.Client.Http;
using YandexDisk.Client;
using YandexDisk.Client.Protocol;
using YandexDisk.Client.Clients;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Reflection.Metadata;

namespace TaskManagerLogic.Classes;
public class YandexDrive
{
    private string key;
    private IDiskApi diskApi;
    public YandexDrive()
    {
        Console.WriteLine("Связь создана");
        this.key = "y0__xCSl7X1Axjv2DYgh5uk3BLUqwrEBO6f9QgbmNLExiQySF-75Q";
        this.diskApi = new DiskHttpApi(key);
    }
    public async Task GetData()
    {
        var diskInfo = await diskApi.MetaInfo.GetDiskInfoAsync();
        Console.WriteLine($"Всего места: {diskInfo.TotalSpace}");
        Console.WriteLine($"Занято: {diskInfo.UsedSpace}");
    }
    public async Task CreateNewFolder(string FolderName)
    {
        await diskApi.Commands.CreateDictionaryAsync(FolderName);
    }

    public async Task DownloadFile(string FilePath, string SavePath)
    {
        await diskApi.Files.DownloadFileAsync(
            path: FilePath,
            localFile: $"{SavePath}\\{FilePath.Split("/")[FilePath.Split("/").Length-1]}"
            );
    }

    public async Task UploadFile(string DiskFilePath, string LocalFilePath)
    {
        var cts = new CancellationTokenSource();
        await diskApi.Files.UploadFileAsync(
               path: DiskFilePath, 
               overwrite: true,                 
               localFile: LocalFilePath,    
               cancellationToken: cts.Token
               );
    }

    public async Task DeleteFile(string FilePath)
    {
        var cts = new CancellationTokenSource();
        await diskApi.Commands.DeleteAsync(new DeleteFileRequest
        {
            Path = FilePath,
            Permanently = false
        }, cts.Token);
    }

    public async Task<bool> CheckFileNameExists(string DiskFilePath, string LocalFileName)
    {
        var folderInfo = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
        {
            Path = DiskFilePath,
            Limit = 1000,
            Offset = 0
        });
        bool isValid = false;
        foreach (var file in folderInfo.Embedded.Items)
        {
            if (file.Name == LocalFileName)
            {
                
                isValid = true;
                break;
            }
            
        }
        return isValid;
    }
    public async Task<string> GetFileNameByStart(string FileStart, string FilePath)
    {
        string FileName = "";
        var folderInfo = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
        {
            Path = FilePath,
            Limit = 1000,
            Offset = 0
        });

        foreach (var file in folderInfo.Embedded.Items)
        {
            if (file.Name.StartsWith(FileStart))
            {
                return file.Name;
            }
        }
        return "";
    }

    public async Task CreateEmptyFile(string DiskFilePath)
    {
        var cts = new CancellationTokenSource();
        string tempPath = Path.GetTempFileName();
        await diskApi.Files.UploadFileAsync(
                    path: DiskFilePath,
                    overwrite: false,
                    localFile: tempPath,
                    cancellationToken: cts.Token);
        File.Delete(tempPath);

    }
    public async Task CreateFileWithContent(string DiskFilePath, string Content)
    {
        var cts = new CancellationTokenSource();
        string tempPath = Path.GetTempFileName();
        File.WriteAllText(tempPath, Content);
        await diskApi.Files.UploadFileAsync(
                    path: DiskFilePath,
                    overwrite: false,
                    localFile: tempPath,
                    cancellationToken: cts.Token);
        File.Delete(tempPath);

    }
    public async Task Maind()
    {

        try
        {
            await GetData();
            await CreateNewFolder("Pomogite/ArbuZ");

            string folderPath = "/";
            var folderInfo = await diskApi.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = folderPath,
                Limit = 1000,
                Offset = 0
            });
            foreach (var file in folderInfo.Embedded.Items)
            {
                Console.WriteLine(file.Name);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
    


}