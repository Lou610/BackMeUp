Have you ever wanted to just drop files and it gets moved to your FTP server?
Well look no further!

BackMeUp is a background worker written in .Net 8 that can be run on any windows 11 machines.

It will even email you once all your files has been uploaded.

Setup:
update the appsettings.json file with the following information:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "FTP": {
    "ftpServerUri": "FTP URL",
    "ftpUsername": "User",
    "ftpPassword": "Password"
  },
  "FilePath": {
    "LocalFilePath": "File path where to drop files"
  }
}

