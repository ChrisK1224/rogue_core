using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RapidHQLWeb.Models;
using Microsoft.AspNetCore.Hosting;

namespace RapidHQLWeb.Pages
{
    public class MapLargeDownloadModel : PageModel
    {
        static string path = Environment.CurrentDirectory;
        public static String downloadPath = path + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "MapLargeProj" + Path.DirectorySeparatorChar + "MapLargeProjectKatsaros.zip";
        public List<FileModel> Files { get; set; }
        public MapLargeDownloadModel(IHostingEnvironment _environment)
        {
            //this.Environment = _environment;
        }
        public void OnGet()
        {
            //Copy File names to Model collection.
            this.Files = new List<FileModel>();
            //foreach (string filePath in filePaths)
            //{
                this.Files.Add(new FileModel { FileName = Path.GetFileName(downloadPath) });
            //}
        }
        public FileResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = downloadPath;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
