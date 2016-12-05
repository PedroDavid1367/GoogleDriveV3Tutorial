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
    static string[] Scopes = { DriveService.Scope.Drive };
    static string ApplicationName = "DriveV3 API .NET Quickstart";

    public static void Main(string[] args)
    {
      //ListGoogleDriveFiles();
      //UpdateGoogleDriveFile();
      //DownloadGoogleDriveFile();
      //CreateShortcutToFile();
      UpdateShortcutToFile();

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
  }
}
