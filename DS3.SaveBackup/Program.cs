using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DS3.SaveBackup
{

    struct StateObjItem
    {
        // Used to hold parameters for calls to TimerTask.
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;
    }

    class Program
    {

        private string saveFileSource = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.SaveFileSource);
        private string saveFileDestination = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.SaveFileDestination);
        private int saveTimeInterval = Properties.Settings.Default.SaveFileTimeFrequency;
        private string processName = Properties.Settings.Default.ProcessToCheck;
            
        static void Main(string[] args)
        {
            
            Program program = new Program();
            program.Initiate();
        }

        private void Initiate()
        {
            while (ShouldProcessFiles())
            {
                ProcessFiles();
                Thread.Sleep(saveTimeInterval * 10000);
            }

        }
        private bool ShouldProcessFiles()
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                return true;
            }

            return false;

        }
        private void ProcessFiles()
        {
            string[] filesToProcess = GetSaveFiles(saveFileSource);

            if (filesToProcess.Length > 0)
            {
                CopySaveFiles(filesToProcess, saveFileDestination);
            }
        }

        private string[] GetSaveFiles(string saveFileLocation)
        {
            if (Directory.Exists(saveFileLocation))
            {
                return Directory.GetFiles(saveFileLocation);
            }
            return new string[] {};
        }

        private void CopySaveFiles(string[] filesToCopy, string folderDestination)
        {
            if (!Directory.Exists(folderDestination))
            {
                Directory.CreateDirectory(folderDestination);
            }

            if (Directory.Exists(folderDestination))
            {
                foreach (var file in filesToCopy)
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file,folderDestination);
                    }
                }
            }
        }

      
    }
}
