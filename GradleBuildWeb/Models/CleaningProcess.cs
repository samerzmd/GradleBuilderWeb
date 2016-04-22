using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradleBuildWeb.Models
{
    class CleaningProcess : Process, IEquatable<CleaningProcess>
    {

        public CleaningProcess(string srcDirectory)
        {
            EnableRaisingEvents = true;
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = srcDirectory,
                UseShellExecute = false,
                RedirectStandardInput = true,
            };

        }

        public new bool Start()
        {
            var result = base.Start();

            StandardInput.WriteLine(@"gradlew.bat clean");
            StandardInput.WriteLine("exit");

            return result;
        }
        //for nxt releases with threading and single job project
        public bool Equals(CleaningProcess other)
        {
            return StartInfo.WorkingDirectory.Equals(other.StartInfo.WorkingDirectory);
        }
    }
}
