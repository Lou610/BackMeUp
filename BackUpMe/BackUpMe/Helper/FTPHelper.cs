using System;
using System.IO;
using System.Net;
using BackUpMe.Models;
using Microsoft.Extensions.Configuration;
using FluentFTP;

namespace BackUpMe.Helper
{
    public static class FTPHelper
    {
        /// <summary>
        /// This method will monitor a foler for changes and upload the changes to the FTP server
        /// </summary>
        /// <param name="Folder"></param>
        /// <returns>Rsult of boolean</returns>
        public static bool FolderWatcher()
        {
            FilePath filePath = new FilePath();

            filePath = GetFilePath();


            GreateDirectory(filePath.Path!);



            bool result = false;

            using (var watcher = new FileSystemWatcher(filePath.Path!))
            {
                OnFileCreated(filepath: filePath.Path!);
            }

            return result;
        }

        /// <summary>
        /// OnFile Created Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnFileCreated(string filepath)
        {
            FTPModel fTPModel = new FTPModel();

            string filePath = filepath;

            // Call FTP credentials
            fTPModel = GetFTPCredentials();

            // Call UploadFile method
            Upload(filePath, fTPModel);
        }
      
        /// <summary>
        /// Upload any new file that was found in the folder
        /// </summary>
        /// <param name="localFilePath"></param>
        /// <param name="ftpServerUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Upload(string folderPath, FTPModel fTPModel)
        {
            string[] files = Directory.GetFiles(folderPath);
            EmailHelper emailHelper = new EmailHelper();


            foreach (string file in files)
            {

                FileInfo fileInf = new FileInfo(file);
                
                string uri = fTPModel.FTPServer + "/" + fileInf.Name;
                
                FtpWebRequest reqFTP;

                // Create FtpWebRequest object from the Uri provided
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(fTPModel.FTPServer + "/" + fileInf.Name));
#pragma warning restore SYSLIB0014 // Type or member is obsolete

                // Provide the WebPermission Credintials
                reqFTP.Credentials = new NetworkCredential(fTPModel.FTPUsername, fTPModel.FTPPassword);
                
                // By default KeepAlive is true, where the control connection is not closed
                // after a command is executed.
                reqFTP.KeepAlive = false;
                
                // Specify the command to be executed.
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                
                // Specify the data transfer type.
                reqFTP.UseBinary = true;
                
                // Notify the server about the size of the uploaded file
                reqFTP.ContentLength = fileInf.Length;
                
                // The buffer size is set to 2kb
                int buffLength = 2048;
                
                byte[] buff = new byte[buffLength];
                
                int contentLen;
                
                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                FileStream fs = fileInf.OpenRead();
                
                try
                {
                    // Stream to which the file to be upload is written
                    Stream strm = reqFTP.GetRequestStream();
                
                    // Read from the file stream 2kb at a time
                    contentLen = fs.Read(buff, 0, buffLength);
                    
                    // Till Stream content ends
                    while (contentLen != 0)
                    {
                        // Write Content from the file stream to the FTP Upload Stream
                        strm.Write(buff, 0, contentLen);
                    
                        contentLen = fs.Read(buff, 0, buffLength);
                    }
                    // Close the file stream and the Request Stream
                    strm.Close();
                    fs.Close();

                    emailHelper.SendEmail(fileInf.Name);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                DeleteFile(file);

            }
        }


        public static void DeleteFile(string FileName)
        {
            File.Delete(FileName);
        }


        /// <summary>
        /// Gets the FTP details for the appsettings.json file
        /// </summary>
        /// <returns>return FTP Model</returns>
        public static FTPModel GetFTPCredentials()
        {
            FTPModel ftpModel = new FTPModel();

            ftpModel.FTPServer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FTP")["ftpServerUri"];
            ftpModel.FTPUsername = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FTP")["ftpUsername"];
            ftpModel.FTPPassword = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FTP")["ftpPassword"];

            return ftpModel;
        }

        /// <summary>
        /// Get the file path from the appsettings.json file
        /// </summary>
        /// <returns>Return file path model</returns>
        public static FilePath GetFilePath()
        {
            FilePath filePath = new FilePath();

            filePath.Path = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("FilePath")["LocalFilePath"];

            return filePath;
        }


        public static void GreateDirectory(string filePath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder: {ex.Message}");
            }

        }

    }
}
