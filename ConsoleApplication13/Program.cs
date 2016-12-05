using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsoleApplication13
{
  public class Program
  {
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/drive-dotnet-quickstart.json
    static string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveAppdata };
    static string ApplicationName = "DriveV3 API .NET Quickstart";

    public static void Main(string[] args)
    {
      //ListGoogleDriveFiles();
      //UpdateGoogleDriveFile();
      //DownloadGoogleDriveFile();
      //CreateShortcutToFile();
      //UpdateShortcutToFile();
      //ImportFile();
      //DownloadFile();
      //DownloadIPSWFile();
      //CreateFolder();
      //InsertFileInFolder();
      //MoveFileBetweenFolders();
      //InsertFileIntoApplicationDataFolder();
      //ListFilesFromApplicationDataFolder();
      SearchOfImageFile();

      Console.ReadKey();
    }

    // Movies - November.txt (0B7M8H1cu-p5pQnNxdzR6Y2JXSXM)
    public static void DownloadGoogleDriveFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      var fileId = "0B7M8H1cu-p5pQnNxdzR6Y2JXSXM";
      var request = service.Files.Export(fileId, "text/plain");
      var streamForFile = new System.IO.MemoryStream();
      // Add a handler which will be notified on progress changes.
      // It will notify on each chunk download and when the
      // download is completed or failed.
      request.MediaDownloader.ProgressChanged += ManageProgress;
        //(IDownloadProgress progress) =>
        //{
        //  switch (progress.Status)
        //  {
        //    case DownloadStatus.Downloading:
        //      {
        //        Console.WriteLine(progress.BytesDownloaded);
        //        break;
        //      }
        //    case DownloadStatus.Completed:
        //      {
        //        Console.WriteLine("Download complete.");
        //        break;
        //      }
        //    case DownloadStatus.Failed:
        //      {
        //        Console.WriteLine("Download failed.");
        //        break;
        //      }
        //  }
        //};
      request.Download(streamForFile);
    }

    private static void ManageProgress(IDownloadProgress progress)
    {
      switch (progress.Status)
      {
        case DownloadStatus.Downloading:
        {
          Console.WriteLine(progress.BytesDownloaded);
          break;
        }
        case DownloadStatus.Completed:
        {
          Console.WriteLine("Download complete.");
          break;
        }
        case DownloadStatus.Failed:
        {
          Console.WriteLine("Download failed.");
          break;
        }
      }
    }

    public static void UpdateGoogleDriveFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      using (var stream = new FileStream(@"C:\Users\Pedro David\Desktop\test.txt", FileMode.Open))
      {
        // Create Drive API service.
        var service = new DriveService(new BaseClientService.Initializer()
        {
          HttpClientInitializer = credential,
          ApplicationName = ApplicationName,
        });

        // Define parameters of request.
        var fileToUpdate = new Google.Apis.Drive.v3.Data.File();
        fileToUpdate.Name = "Updated Movies - November.txt";
        fileToUpdate.MimeType = "application/vnd.google-apps.document";
        //var updateRequest = service.Files.Update(fileToUpdate, "0B7M8H1cu-p5pQnNxdzR6Y2JXSXM");
        //var updatedFile = updateRequest.Execute();
        //Console.WriteLine("{0} ({1}) {2}", updatedFile.Name, updatedFile.Id, updatedFile.MimeType);

        var updateRequest = service.Files.Update(fileToUpdate, "0B7M8H1cu-p5pQnNxdzR6Y2JXSXM", stream, "application/vnd.google-apps.document");
        var updatedFile = updateRequest.Upload();
      }
    }

    public static void ListGoogleDriveFiles()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      FilesResource.ListRequest listRequest = service.Files.List();
      listRequest.PageSize = 10;
      listRequest.Fields = "nextPageToken, files(id, name, mimeType)";

      // List files.
      IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
          .Files;
      Console.WriteLine("Files:");
      if (files != null && files.Count > 0)
      {
        foreach (var file in files)
        {
          //var json = new JavaScriptSerializer().Serialize(file);
          //Console.WriteLine(json);
          Console.WriteLine("{0} ({1}) {2}", file.Name, file.Id, file.MimeType);
        }
      }
      else
      {
        Console.WriteLine("No files found.");
      }
    }

    public static void CreateShortcutToFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileMetadata = new Google.Apis.Drive.v3.Data.File();
      fileMetadata.Name = "Created Shortcut";
      fileMetadata.MimeType = "application/vnd.google-apps.drive-sdk";
      var request = service.Files.Create(fileMetadata);
      request.Fields = "id, mimeType";
      var file = request.Execute();
      Console.WriteLine("File ID: {0} File MimeType: {1}", file.Id, file.MimeType);
    }

    //Created Shortcut (0B7M8H1cu-p5pM3NVb2FwQVhBdGs) application/vnd.google-apps.drive-sdk.50787884944
    public static void UpdateShortcutToFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileId = "0B7M8H1cu-p5pM3NVb2FwQVhBdGs";
      var fileMetadata = new Google.Apis.Drive.v3.Data.File();
      fileMetadata.ModifiedTime = DateTime.Now;
      var request = service.Files.Update(fileMetadata, fileId);
      request.Fields = "id, modifiedTime";
      var file = request.Execute();
      Console.WriteLine("Modified time: " + file.ModifiedTime);
    }

    public static void ImportFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileMetadata = new Google.Apis.Drive.v3.Data.File();
      fileMetadata.Name = "My Report";
      fileMetadata.MimeType = "application/vnd.google-apps.spreadsheet";
      FilesResource.CreateMediaUpload request;
      using (var stream = new System.IO.FileStream("files/report.csv",
                              System.IO.FileMode.Open))
      {
        request = service.Files.Create(
            fileMetadata, stream, "text/csv");
        request.Fields = "id";
        request.Upload();
      }
      var file = request.ResponseBody;
      Console.WriteLine("File ID: " + file.Id);
    }

    //1i9PQWBzZswXQMBhypkEoNKFsJQ-jVgr3u2OMU4xXg7c
    public static void DownloadFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileId = "1i9PQWBzZswXQMBhypkEoNKFsJQ-jVgr3u2OMU4xXg7c";
      var request = service.Files.Export(fileId, "text/csv");
      var inMemoryStream = new System.IO.MemoryStream();

      // Add a handler which will be notified on progress changes.
      // It will notify on each chunk download and when the
      // download is completed or failed.
      request.MediaDownloader.ProgressChanged += ManageProgress;
      request.Download(inMemoryStream);

      using (var outputStream = new FileStream("downloaded-data.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite))
      {
        inMemoryStream.WriteTo(outputStream);
      }
    }

    //iPhone1,2_3.1.3_7E18_Restore.ipsw (0B7M8H1cu-p5pT1c4bFgtMjljdDg) application/x-zip
    public static void DownloadIPSWFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileId = "0B7M8H1cu-p5pT1c4bFgtMjljdDg";
      var request = service.Files.Get(fileId);
      var inMemoryStream = new System.IO.MemoryStream();

      // Add a handler which will be notified on progress changes.
      // It will notify on each chunk download and when the
      // download is completed or failed.
      request.MediaDownloader.ProgressChanged += ManageProgress;
      request.Download(inMemoryStream);

      using (var outputStream = new FileStream("downloaded-firmware.ipsw", FileMode.OpenOrCreate, FileAccess.ReadWrite))
      {
        inMemoryStream.WriteTo(outputStream);
      }
    }

    public static void CreateFolder()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileMetadata = new Google.Apis.Drive.v3.Data.File();
      fileMetadata.Name = "VS-Tests";
      fileMetadata.MimeType = "application/vnd.google-apps.folder";
      var request = service.Files.Create(fileMetadata);
      request.Fields = "id";
      var file = request.Execute();
      Console.WriteLine("Folder ID: " + file.Id);
    }

    //VS-Tests (0B7M8H1cu-p5pMy14eUlCNzM2Z00) application/vnd.google-apps.folder
    public static void InsertFileInFolder()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var folderId = "0B7M8H1cu-p5pMy14eUlCNzM2Z00";
      var fileMetadata = new Google.Apis.Drive.v3.Data.File();
      fileMetadata.Name = "inserted-file-to-folder.csv";
      fileMetadata.Parents = new List<string> { folderId };
      FilesResource.CreateMediaUpload request;
      using (var stream = new System.IO.FileStream("files/report.csv",
          System.IO.FileMode.Open))
      {
        request = service.Files.Create(
            fileMetadata, stream, "text/csv");
        request.Fields = "id";
        request.Upload();
      }
      var file = request.ResponseBody;
      Console.WriteLine("File ID: " + file.Id);
    }

    //VS-Tests(0B7M8H1cu-p5pMy14eUlCNzM2Z00) application/vnd.google-apps.folder
    //Updated Movies - November.txt (0B7M8H1cu-p5pQnNxdzR6Y2JXSXM) text/plain
    //Test-01 (0B7M8H1cu-p5pallXbklpYlBlVGM) application/vnd.google-apps.folder
    public static void MoveFileBetweenFolders()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileId = "0B7M8H1cu-p5pQnNxdzR6Y2JXSXM";
      var folderId = "0B7M8H1cu-p5pMy14eUlCNzM2Z00";
      // Retrieve the existing parents to remove
      var getRequest = service.Files.Get(fileId);
      getRequest.Fields = "parents";
      var file = getRequest.Execute();
      var previousParents = string.Join(",", file.Parents);
      // Move the file to the new folder
      var updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId);
      updateRequest.Fields = "id, parents";
      updateRequest.AddParents = folderId;
      updateRequest.RemoveParents = previousParents;
      file = updateRequest.Execute();
    }

    public static void InsertFileIntoApplicationDataFolder()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var fileMetadata = new Google.Apis.Drive.v3.Data.File();
      fileMetadata.Name = "config.json";
      fileMetadata.Parents = new List<string>() { "appDataFolder" };
      FilesResource.CreateMediaUpload request;
      using (var stream = new System.IO.FileStream("files/config.json",
          System.IO.FileMode.Open))
      {
        request = service.Files.Create(
            fileMetadata, stream, "application/json");
        request.Fields = "id";
        var eg = request.Upload();
      }
      var file = request.ResponseBody;
      Console.WriteLine("File ID: " + file.Id);
    }

    public static void ListFilesFromApplicationDataFolder()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      var request = service.Files.List();
      request.Spaces = "appDataFolder";
      request.Fields = "nextPageToken, files(id, name)";
      request.PageSize = 10;
      var result = request.Execute();
      foreach (var file in result.Files)
      {
        Console.WriteLine(string.Format(
            "Found file: {0} ({1})", file.Name, file.Id));
      }
    }

    public static void SearchOfImageFile()
    {
      UserCredential credential;

      using (var stream =
          new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = System.Environment.GetFolderPath(
          System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/driveV3-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }

      // Create Drive API service.
      var service = new DriveService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      string pageToken = null;
      do
      {
        var request = service.Files.List();
        request.Q = "mimeType='text/csv'";
        request.Spaces = "drive";
        request.Fields = "nextPageToken, files(id, name)";
        request.PageToken = pageToken;
        var result = request.Execute();
        foreach (var file in result.Files)
        {
          Console.WriteLine(string.Format(
                  "Found file: {0} ({1})", file.Name, file.Id));
        }
        pageToken = result.NextPageToken;
      } while (pageToken != null);
    }
  }
}
