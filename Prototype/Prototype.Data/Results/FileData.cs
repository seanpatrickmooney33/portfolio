using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Prototype.Data.Results
{
    public static class FileData
    {
        public static String GetFileData(String FileName)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo fileInfo = di.GetFiles(FileName).Where(x => x.Exists).FirstOrDefault();


            StreamReader sr = new StreamReader(fileInfo.OpenRead(), Encoding.UTF8, true, 1024);
            String data = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();


            return data;
        }
    }
}
