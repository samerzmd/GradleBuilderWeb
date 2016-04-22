﻿using System;
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

        public new bool Start()
        {
            var result =base.Start();

            StandardInput.WriteLine(@"gradlew.bat assembleDebug");
            StandardInput.WriteLine("exit");

            return result;
        }
        //for next releases 
        public bool Equals(BuildingProcess other)
        {
            return StartInfo.WorkingDirectory.Equals(other.StartInfo.WorkingDirectory);
        }
    }
}
