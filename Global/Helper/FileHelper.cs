using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HELP.GlobalFile.Global.Helper
{
    public static class FileHelper
    {
        public static async Task<string> Save(IFormFile file,string rootPath)
            {
                DateTime now = DateTime.Now;
                string urlPrefix = string.Format($"{now.Year}\\{now.Month}\\{now.Day}");
                string directory = Path.Combine(rootPath, urlPrefix);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string fileName = Path.Combine(DateTime.Now.ToString("HH_mm_ss") + "_" + Path.GetFileName(file.FileName));
                string relativePath = Path.Combine(urlPrefix, fileName);
                using (var fs = File.Create(Path.Combine(directory, fileName)))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
                return relativePath;
            }

    }
}
