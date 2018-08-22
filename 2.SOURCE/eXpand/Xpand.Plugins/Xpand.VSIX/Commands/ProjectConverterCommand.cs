﻿using System.ComponentModel.Design;
using System.IO;
using EnvDTE80;
using Xpand.VSIX.Extensions;
using Xpand.VSIX.Options;
using Xpand.VSIX.VSPackage;
using Process = System.Diagnostics.Process;

namespace Xpand.VSIX.Commands {
    public class ProjectConverterCommand:VSCommand {
        private ProjectConverterCommand():base((sender, args) => Convert(), new CommandID(PackageGuids.guidVSXpandPackageCmdSet, PackageIds.cmdidProjectConverter)){
            this.EnableForDXSolution();
        }
        private static readonly DTE2 _dte =DteExtensions.DTE;
        private static string GetProjectConverterPath() {
            if (string.IsNullOrWhiteSpace(OptionClass.Instance.ProjectConverterPath)) {
                var version = _dte.Solution.GetDXVersion();
                var dxRootDirectory = _dte.Solution.GetDXRootDirectory();
                return Path.Combine(dxRootDirectory + @"\Tools\Components", "TestExecutor." + version + ".exe");
            }
            return OptionClass.Instance.ProjectConverterPath;
        }
        public static void Convert(){
            _dte.InitOutputCalls("ConvertProject");
            string path = GetProjectConverterPath();
            string token = OptionClass.Instance.Token;
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(token)) {
                var directoryName = Path.GetDirectoryName(_dte.Solution.FileName);
                _dte.WriteToOutput("Project Converter Started !!!");
                var userName = $"/sc /k:{token} \"{directoryName}\"";
                Process.Start(path, userName);
            }
        }

        public static void Init(){
            new ProjectConverterCommand();
        }
    }
}
