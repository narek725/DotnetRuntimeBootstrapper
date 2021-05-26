﻿using System;
using DotnetRuntimeBootstrapper.Env;
using DotnetRuntimeBootstrapper.Utils;
using OperatingSystem = DotnetRuntimeBootstrapper.Env.OperatingSystem;

namespace DotnetRuntimeBootstrapper.RuntimeComponents
{
    public class WindowsUpdate3154518RuntimeComponent : IRuntimeComponent
    {
        public string DisplayName => "Windows Update KB3154518";

        public bool IsRebootRequired => true;

        public bool CheckIfInstalled() =>
            OperatingSystem.Version >= OperatingSystemVersion.Windows8 ||
            OperatingSystem.IsUpdateInstalled("KB3154518");

        private string GetInstallerDownloadUrl() =>
            OperatingSystem.IsProcessor64Bit
                ? "https://download.microsoft.com/download/6/8/0/680ee424-358c-4fdf-a0de-b45dee07b711/windows6.1-kb3154518-x64.msu"
                : "https://download.microsoft.com/download/6/8/0/680ee424-358c-4fdf-a0de-b45dee07b711/windows6.1-kb3154518-x86.msu";

        public DownloadedRuntimeComponentInstaller DownloadInstaller(Action<double> handleProgress)
        {
            var filePath = FileEx.GetTempFileName("KB3154518", "msu");
            Http.DownloadFile(GetInstallerDownloadUrl(), filePath, handleProgress);

            return new DownloadedRuntimeComponentInstaller(this, filePath);
        }
    }
}