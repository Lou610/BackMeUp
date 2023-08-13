using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackUpMe.Helper;


namespace BackUpMe.Services
{
    public sealed class FTPService
    {
        public string GetFiles()
        {
            FTPHelper.FolderWatcher();

            return $"File / Files uploaded successfully";
        }
    }
}
