using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using WixSharp;

namespace WixInstaller
{
    [System.Runtime.InteropServices.Guid("1230653A-0966-4829-9E0D-C6508B1277EA")]
    class Program
    {
        static void Main()
        {
            string productMsi = BuildMsi();
        }

        static string BuildMsi()
        {
            //Source for file to be installed
            string sRootDir = @"C:\Immucor\Beispiel Utility\Beispiel Export\WixInstaller\PublishedFiles";
            // Install Path
            string installPath = @"%ProgramFiles%\LIFECODES\MATCH IT Koln Export Utility";

            WixEntity[] installFiles = new WixEntity[0];
            installFiles = BuildDirInfo(sRootDir, installFiles);

            /* Install with one install directory shown below. If more than one directory, such as needing to install to the programdata folder, 
            * then add another new Dir({putInstallFolderPathHere},{putNewWixEntityHereForFiles}).
            */

            //string commonPath = @"C:\TFSSource\Wix Installer Projects\WixInstaller\PublishedFiles\CommonApp";
            //// Install Path
            //string commonInstallPath = @"%CommonAppDataFolder%\LIFECODES";

            //WixEntity[] commonFiles = new WixEntity[0];
            //commonFiles = BuildDirInfo(commonPath, commonFiles);

            var project = new Project("MATCH IT Koln Export Utility", new Dir(installPath, installFiles))
            {
                Version = new Version(1, 0, 0),
                UpgradeCode = new Guid("{74605FEA-7C18-46AD-B54C-6A9181988A85}"),
                Id = "MATCH_IT_Koln_Export_Utility", // this is for internal application use, if more than one application is going to be install with this package.
                ControlPanelInfo = new ProductInfo() { Manufacturer = "Immucor, Inc." },
                InstallScope = InstallScope.perMachine,
                UI = WUI.WixUI_ProgressOnly,
                Platform = Platform.x86
            };

            project.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;

            project.ResolveWildCards();

            // Edit Files
            var exeFile = project.AllFiles.Single(f => f.Name.EndsWith("MATCHITKolnExportUtility.exe"));

            exeFile.Shortcuts = new[]
            {
                new FileShortcut("MATCH IT Koln Export Utility", @"%ProgramMenu%\Lifecodes") { WorkingDirectory = installPath },
                new FileShortcut("MATCH IT Koln Export Utility", @"%Desktop%") { WorkingDirectory = installPath }
            };

            Compiler.PreserveTempFiles = true;

            // Output path for MSI file.
            return project.BuildMsi(@"C:\Immucor\Beispiel Utility\Beispiel Export\WixSetupUI\Resources\MATCH_IT_Beispiel_Export_UtilitySetup.msi");
        }

        /// <summary>
        /// This method will find all files and directories in the source file location
        /// 
        private static WixEntity[] BuildDirInfo(string sRootDir, WixEntity[] weDir)
        {
            DirectoryInfo RootDirInfo = new DirectoryInfo(sRootDir);
            if (RootDirInfo.Exists)
            {
                DirectoryInfo[] DirInfo = RootDirInfo.GetDirectories();
                List<string> lMainDirs = new List<string>();
                foreach (DirectoryInfo DirInfoSub in DirInfo)
                    lMainDirs.Add(DirInfoSub.FullName);
                int cnt = lMainDirs.Count;
                weDir = new WixEntity[cnt + 1];
                if (cnt == 0)
                    weDir[0] = new DirFiles(RootDirInfo.FullName + @"\*.*");
                else
                {
                    weDir[cnt] = new DirFiles(RootDirInfo.FullName + @"\*.*");
                    for (int i = 0; i < cnt; i++)
                    {
                        DirectoryInfo RootSubDirInfo = new DirectoryInfo(lMainDirs[i]);
                        if (!RootSubDirInfo.Exists)
                            continue;
                        WixEntity[] weSubDir = new WixEntity[0];
                        weSubDir = BuildDirInfo(RootSubDirInfo.FullName, weSubDir);
                        weDir[i] = new Dir(RootSubDirInfo.Name, weSubDir);
                    }
                }
            }
            return weDir;
        }
    }
}