using System;
using System.Diagnostics;

namespace GradleBuildWeb.Models
{
    
    public class BuildingProcess: Process,IEquatable<BuildingProcess>
    {

        public BuildingProcess(string srcDirectory)
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
        
        public bool Equals(BuildingProcess other)
        {
            return StartInfo.WorkingDirectory.Equals(other.StartInfo.WorkingDirectory);
        }
    }
}
