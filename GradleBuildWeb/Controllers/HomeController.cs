using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Web.Mvc;
using GradleBuildWeb.Models;
using Newtonsoft.Json;

namespace GradleBuildWeb.Controllers
{
    public class HomeController : Controller
    {

        private static bool _isBuilding;
        private static bool _isCleaning;
        private bool _isPulling;

        private string ProjectPath = @"C:\Projects\TheApplication";
        private string apkPath = @"\app\build\outputs\apk\app-debug.apk";
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        private static string StartServerProcess(Configuration configuration)
        {
            string path = @"c:\GradleServerConfig.txt";
            string json = JsonConvert.SerializeObject(configuration);
            if (!System.IO.File.Exists(path))
            {
                using (var sw = System.IO.File.CreateText(path))
                {
                    sw.Write(json);
                }
            }
            else
            {
                System.IO.File.WriteAllText(path, json);
            }
            using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream("test-pipe"))
            {
                namedPipeServer.WaitForConnection();
                namedPipeServer.WriteByte(1);
                int byteFromClient = namedPipeServer.ReadByte();
                return byteFromClient.ToString();
            }
        }

        private string ServerCurrentStatus()
        {
            if (_isCleaning) return "server is busy with cleaning process";
            else if (_isBuilding) return "it's building";
            else if (_isPulling) return "server is busy getting latest CODE";
            else return string.Empty;
        }

        public ActionResult Build()
        {
            var status = ServerCurrentStatus();
            if (!status.Equals(string.Empty))
            {
                return Json(status);
            }

            _isBuilding = true;
            StartServerProcess(new Configuration { BuildCommand = "build", Path = "C:\\Projects\\TheApplication", MainOprationName = "Gradle" });
            _isBuilding = false;
            return Json("building has just Finsihed");
        }

        public ActionResult Clean()
        {
            var status = ServerCurrentStatus();
            if (!status.Equals(string.Empty))
            {
                return Json(status);
            }

            _isCleaning = true;
            StartServerProcess(new Configuration { BuildCommand = "clean", Path = "C:\\Projects\\TheApplication", MainOprationName = "Gradle" });
            _isCleaning = false;
            return Json("cleaning has just finished");
        }

        public ActionResult GitLatest()
        {
            var status = ServerCurrentStatus();
            if (!status.Equals(string.Empty))
            {
                return Json(status);
            }

            _isPulling = true;
            StartServerProcess(new Configuration { BuildCommand = "clean", Path = "C:\\Projects\\TheApplication", MainOprationName = "Git" });
            _isPulling = false;
            return Json("we have the latest code, comrade general");
        }

        public ActionResult ServerStatus()
        {
            if (_isBuilding)
                return Json("server is busy with building process");
            if (_isCleaning)
                return Json("server is busy with cleaning process");
            return Json("server is wating the next Ops");
        }

        public ActionResult Download(string zaPath)
        {
            byte[] fileBytes = GetFile(ProjectPath + apkPath);
            if (fileBytes != null)
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "app.apk");

            return Content("It seems that we are unable to find built version. why not trying to build it first");
        }

        private byte[] GetFile(string s)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(s);
                byte[] data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException(s);
                return data;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

    }
}