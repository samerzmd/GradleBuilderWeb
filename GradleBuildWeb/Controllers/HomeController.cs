using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace GradleBuildWeb.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            var process = new Process
            {
                StartInfo =
                            {
                                FileName = "cmd.exe",
                                WorkingDirectory = @"C:\Projects\TheApplication",
                                UseShellExecute = false,
                                RedirectStandardInput = true,
                            },
                EnableRaisingEvents = true

            };
            process.Exited += (sender, e) =>
            {
                Console.WriteLine("shiiiit");
            };
            process.Start();

            process.StandardInput.WriteLine(@"gradlew.bat assembleDebug");
            process.StandardInput.WriteLine("exit");

            return View();
        }
    }
}