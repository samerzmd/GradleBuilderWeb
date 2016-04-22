using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using GradleBuildWeb.Models;

namespace GradleBuildWeb.Controllers
{
    public class HomeController : Controller
    {

        private static bool _isBuilding;
        private static bool _isCleaning;

        private string ProjectPath = @"C:\Projects\TheApplication";
        private string apkPath = @"\app\build\outputs\apk\app-debug.apk";
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Build()
        {
            if (_isCleaning)
                return Json("server is busy with cleaning process");

            if (_isBuilding)
                return Json("it's building");

            _isBuilding = true;
            var process = new BuildingProcess(ProjectPath);
            process.Exited += (sender, e) =>
            {
                _isBuilding = false;
                Console.WriteLine("shiiiit");
            };
            process.Start();
            return Json("building has just started");
        }
        public ActionResult Clean()
        {
            if (_isBuilding)
                return Json("server is busy with building process");

            if (_isCleaning)
                return Json("it's cleaing");

            _isCleaning = true;
            var process = new CleaningProcess(@"C:\Projects\TheApplication");
            process.Exited += (sender, e) =>
            {
                _isCleaning = false;
                Console.WriteLine("shiiiit");
            };
            process.Start();
            return Json("cleaning has just started");
        }

        public ActionResult ServerStatus()
        {
            if (_isBuilding)
                return Json("server is busy with building process");
            if (_isCleaning)
                return Json("server is busy with cleaning process");

            return Json("server is wating the next Ops");
        }

        public ActionResult Download()
        {
            try
            {
                byte[] fileBytes = GetFile(ProjectPath + apkPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "app.apk");
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

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
            catch (Exception)
            {

                throw;
            }

        }

    }
}