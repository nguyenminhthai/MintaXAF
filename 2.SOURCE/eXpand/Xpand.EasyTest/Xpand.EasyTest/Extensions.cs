using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;
using DevExpress.EasyTest.Framework.Loggers;
using DevExpress.Xpo.DB.Helpers;
using Fasterflect;
using Xpand.EasyTest.Commands;
using Xpand.EasyTest.Commands.InputSimulator;
using Xpand.EasyTest.Commands.Window;
using Xpand.Utils.Helpers;
using Xpand.Utils.Win32;

namespace Xpand.EasyTest {
    public enum ApplicationParams {
        PhysicalPath,
        UseIISExpress,
        UseModel,
        DefaultWindowSize,
        Url,
        DontRunWebDev,
        SingleWebDev,
        WaitDebuggerAttached,
        DontKillWebDev,
        DontRestartIIS,
        WebBrowserType,
        FileName,
        Model,
        UseIIS,
        DontRunIISExpress,
        UserName,
        DropDatabase
    }

    public interface IXpandEasyTestCommandAdapter {
        IntPtr MainWindowHandle { get;  }
    }

    public interface IXpandTestWinAdapter : IXpandTestAdapter {
         
    }

    public interface IXpandTestAdapter: IApplicationAdapter, ICommandsRegistrator {
        
    }

    public static class Extensions {
        private static readonly string[] NavigationControlPossibleNames = { "ViewsNavigation.Navigation", "Navigation" };

        public static IntPtr GetMainWindowHandle(this ICommandAdapter adapter){
            var mainWindowHandle = ((IXpandEasyTestCommandAdapter)adapter).MainWindowHandle;
            return adapter.IsWinAdapter() ? Process.GetProcessById(mainWindowHandle.ToInt32()).MainWindowHandle : mainWindowHandle;
        }

        public static IntPtr GetApplicationWindowHandle(this ICommandAdapter adapter) {
            ITestControl testControl = adapter.CreateTestControl(TestControlType.Dialog, "");
            if (testControl == null)
                throw new InvalidOperationException("activeWindowControl is null");
            ITestWindow testWindow = testControl.GetInterface<ITestWindow>();
            var caption = testWindow.Caption;
            if (!adapter.IsWinAdapter()){
                caption += " - Internet Explorer";
                return Win32Declares.WindowHandles.FindWindowByCaption(IntPtr.Zero, caption);
            }
            return testWindow.GetActiveWindowHandle();
        }

        public static ITestControl GetNavigationTestControl(this ICommandAdapter adapter) {
            string controlNames = "";
            for (int i = 0; i < NavigationControlPossibleNames.Length; i++) {
                if (adapter.IsControlExist(TestControlType.Action, NavigationControlPossibleNames[i])) {
                    try {
                        var testControl = adapter.CreateTestControl(TestControlType.Action,
                            NavigationControlPossibleNames[i]);
                        var gridBaseInterface = testControl.GetInterface<IGridBase>();
                        int itemsCount = gridBaseInterface.GetRowCount();
                        if (itemsCount > 0) {
                            return testControl;
                        }
                    }
                    catch (WarningException) {
                    }
                }
                controlNames += (i <= NavigationControlPossibleNames.Length)
                    ? NavigationControlPossibleNames[i] + " or "
                    : NavigationControlPossibleNames[i];
            }
            throw new WarningException($"Cannot find the '{controlNames}' control");
        }

        public static void Execute(this CopyFileCommand copyFileCommand,ICommandAdapter adapter,TestParameters testParameters,string sourceFile,string destinationFile){
            var sourceParameter = new Parameter("Source", sourceFile, true, new PositionInScript(0));
            var destinationParameter = new Parameter("Destination", destinationFile, true, new PositionInScript(0));
            copyFileCommand.ParseCommand(testParameters, sourceParameter, destinationParameter);
            copyFileCommand.Execute(adapter);
        }

        public static string GetXpandPath(string directory) {
            var directoryInfo = new DirectoryInfo(directory);
            while (!File.Exists(Path.Combine(directoryInfo.FullName, "Xpand.build"))) {
                directoryInfo = directoryInfo.Parent;
                if (directoryInfo == null)
                    throw new ArgumentNullException();
            }
            return directoryInfo.FullName;
        }

        public static string GetPlatformSuffixedPath(this ICommandAdapter adapter,string fileName){
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName) + "";
            var suffix = adapter.IsWinAdapter() ? ".win" : ".web";
            if (!fileNameWithoutExtension.ToLower().EndsWith(suffix)) {
                var directoryName = Path.GetDirectoryName(fileName) + "";
                fileName = $"{fileNameWithoutExtension}{suffix}{Path.GetExtension(fileName)}";
                fileName =Path.Combine(directoryName, fileName);
            }
            return fileName;
        }

        public static bool IsWinCommand(this Command instance){
            return Adapter is IXpandTestWinAdapter;
        }

        public static bool IsWinAdapter(this ICommandAdapter instance){
            return Adapter is IXpandTestWinAdapter;
        }

        public static string GetBinPath(this Command command) {
            return _application.ParameterValue<string>(ApplicationParams.PhysicalPath) ??
                   Path.GetDirectoryName(_application.ParameterValue<string>(ApplicationParams.FileName));
        }

        public static TestAlias GetAlias(string scriptsPath, string name) {
            var options = LoadOptions(scriptsPath);
            return options.Aliases.Cast<TestAlias>().First(alias => alias.Name == name);
        }

        public static Options LoadOptions(string scriptsPath) {
            var configPath = Path.Combine(scriptsPath, "config.xml");
            var optionsStream = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var optionsLoader = new OptionsLoader();
            return optionsLoader.Load(optionsStream, null, null, Path.GetDirectoryName(configPath));
        }

        public static Command ParseCommand(this Command command,TestParameters testParameters,params Parameter[] parameters){
            string[] strings = parameters.Select(parameter =>" "+ parameter.Name + " = " + parameter.Value).ToArray();
            var commandName = "*"+command.GetType().Name.Replace("Command","");
            var scriptLines = new ScriptStringList(new[] { commandName }.Concat(strings).ToArray());
            var commandCreationParam = new CommandCreationParam(scriptLines, 0, testParameters);
            command.ParseCommand(commandCreationParam);
            return command;
        }

        public static Command SynchWith(this Command instance, Command command) {
            instance.Parameters.AddRange(command.Parameters);
            instance.Parameters.MainParameter = command.Parameters.MainParameter;
            instance.Parameters.ExtraParameter = command.Parameters.ExtraParameter;
            return instance;
        }

        public static void DeleteUserModel(this TestApplication testApplication) {
            var appPath = testApplication.ParameterValue<string>(ApplicationParams.FileName);
            var directoryName = Directory.Exists(Path.GetDirectoryName(appPath)) ? Path.GetDirectoryName(appPath) + "" : testApplication.ParameterValue<string>(ApplicationParams.PhysicalPath);
            foreach (var file in Directory.GetFiles(directoryName, "Model.user*.xafml").ToArray()) {
                File.Delete(file);
            }
        }

        public static void SetParameterValue(this TestApplication application, ApplicationParams applicationParams, string value){
            var dictionary = ((StringDictionary) application.GetFieldValue("attributes"));
            dictionary[applicationParams.ToString()]=value;
        }

        public static bool IsUriAvailable(this TestApplication testApplication, Uri uri) {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(uri.DnsSafeHost);
                foreach (IPAddress ipAddress in ipHostEntry.AddressList) {
                    try {
                        socket.Connect(ipAddress, uri.Port);
                        return true;
                    }
                    catch{
                        // ignored
                    }
                }
            }
            return false;
        }

        public static T ParameterValue<T>(this TestApplication application, ApplicationParams applicationParams) {
            return application.ParameterValue(applicationParams, default(T));
        }

        public static T ParameterValue<T>(this TestApplication application, ApplicationParams applicationParams, T defaultValue) {
            var parameterValue = application.ParameterValue<T>(applicationParams.ToString());
            return Equals(default(T), parameterValue) ? defaultValue : parameterValue;
        }

        public static void ClearModel(this TestApplication application){
            var appPath = application.ParameterValue<string>(ApplicationParams.PhysicalPath) ?? Path.GetDirectoryName(application.ParameterValue<string>(ApplicationParams.FileName));
            File.WriteAllText(Path.Combine(appPath+"","Model.xafml"), @"<?xml version=""1.0"" ?><Application />");
        }

        public static void DropDatabases(this TestApplication application){
            var databases = (application.ParameterValue<string>(ApplicationParams.DropDatabase) + "").Split(';').Where(s => !string.IsNullOrEmpty(s));
            foreach (var database in databases){
                SqlDropDatabaseCommand.Dropdatabase(database);
            }
        }

        public static void CopyModel(this TestApplication application){
            application.ClearModel();
            var appPath = application.ParameterValue<string>(ApplicationParams.PhysicalPath) ?? Path.GetDirectoryName(application.ParameterValue<string>(ApplicationParams.FileName));
            var modelFileName = GetModelFileName(application);
            var destFileName = Path.Combine(appPath+"", "Model.xafml");
            if (File.Exists(modelFileName)){
                File.Copy(modelFileName, destFileName, true);
            }
        }

        private static string GetModelFileName(TestApplication application){
            var model = application.ParameterValue<string>(ApplicationParams.Model);
            var logPath = Logger.Instance.GetLogger<FileLogger>().LogPath;
            return model!=null ? Path.Combine(logPath, model + ".xafml") : logPath;
        }

        public static void CreateParametersFile(this TestApplication application){
            application.DeleteParametersFile();
            var paramFile = application.GetParameterFile();
            var paramValue = application.ParameterValue<string>("Parameter");
            if (paramValue != null){
                using (var streamWriter = File.CreateText(paramFile)){
                    foreach (var param in paramValue.Split(';')){
                        streamWriter.WriteLine(param);
                    }
                }
            }
        }

        public static void DeleteParametersFile(this TestApplication application){
            var paramFile = application.GetParameterFile();
            if (File.Exists(paramFile))
                File.Delete(paramFile);
        }

        private static string GetParameterFile(this TestApplication application){
            var path = application.ParameterValue<string>(ApplicationParams.PhysicalPath) ??
                       Path.GetDirectoryName(application.ParameterValue<string>(ApplicationParams.FileName));
            return Path.Combine(path+"", "easytestparameters");
        }

        public static T ParameterValue<T>(this TestApplication application, string parameterName){
            return application.ParameterValue(parameterName, default(T));
        }

        public static T ParameterValue<T>(this TestApplication application, string parameterName, T defaultValue) {
            var paramValue = application.FindParamValue(parameterName);
            T result;
            if (!XpandConvert.TryToChange(paramValue, out result)) {
                throw new EasyTestException(
                    $"Cannot retrieve the '{parameterName}' attribute's value for the '{application.Name}' application");
            }
            return result;
        }

        public static T ParameterValue<T>(this Command command) {
            return command.ParameterValue(default(T));
        }

        public static T ParameterValue<T>(this Command command, T defaultValue) {
            T result = defaultValue;
            var parameter = command.Parameters.MainParameter;
            if (parameter != null) {
                if (!XpandConvert.TryToChange(parameter.Value, out result)) {
                    throw new CommandException("\'MainParameter\' value is incorrect", command.StartPosition);
                }
            }
            return !result.Equals(default(T)) ? result : defaultValue;
        }

        public static T ParameterValue<T>(this Command command, string parameterName){
            return command.ParameterValue(parameterName, default(T));
        }

        public static T ParameterValue<T>(this Command command, string parameterName,T defaultValue) {
            T result = defaultValue;
            Parameter parameter = command.Parameters[parameterName];
            if (parameter != null){
                if (!XpandConvert.TryToChange(parameter.Value, out result)) {
                    throw new CommandException($"'{parameterName}' value is incorrect", command.StartPosition);    
                }
            }
            return result;
        }
        
        private static IXpandTestAdapter _adapter;
        private static TestApplication _application;

        public static IXpandTestAdapter Adapter => _adapter;

        public static void Assign(this TestApplication application) {
            _application=application;    
        }

        public static void RegisterCommands(this IRegisterCommand registerCommand, IXpandTestAdapter applicationAdapter){
            _adapter = applicationAdapter;
            var dictionary = new Dictionary<Type, string>{
                {typeof (XpandCompareScreenshotCommand), XpandCompareScreenshotCommand.Name},
                {typeof (XpandCompareFilesCommand), XpandCompareFilesCommand.Name},
                {typeof (FillDateTimeValueCommand), FillDateTimeValueCommand.Name},
                {typeof (FieldIsVisibleCommand), FieldIsVisibleCommand.Name},
                {typeof (DeleteCommand), DeleteCommand.Name},
                {typeof (DeleteAllObjectsCommand), DeleteAllObjectsCommand.Name},
                {typeof (CreatePermissionCommand), CreatePermissionCommand.Name},
                {typeof (CopyTextCommand), CopyTextCommand.Name},
                {typeof (CheckClipboardValueCommand), CheckClipboardValueCommand.Name},
                {typeof (ChangeUserPasswordCommand), ChangeUserPasswordCommand.Name},
                {typeof (XpandExecuteEditorAction), XpandExecuteEditorAction.Name},
                {typeof (NavigateCommand), NavigateCommand.Name},
                {typeof (SaveAndCloseCommand), SaveAndCloseCommand.Name},
                {typeof (HideCursorCommand), HideCursorCommand.Name},
                {typeof (KillFocusCommand), KillFocusCommand.Name},
                {typeof (XpandDeleteFileCommand), XpandDeleteFileCommand.Name},
                {typeof (KillWindowCommand), KillWindowCommand.Name},
                {typeof (XpandProcessRecordCommand), XpandProcessRecordCommand.Name},
                {typeof (SqlCommand), SqlCommand.Name},
                {typeof (SqlDropDatabaseCommand), SqlDropDatabaseCommand.Name},
                {typeof (SendKeysCommand), SendKeysCommand.Name},
                {typeof (ActivateWindowCommand), ActivateWindowCommand.Name},
                {typeof (MinimizeApplicationWindowCommand), MinimizeApplicationWindowCommand.Name},
                {typeof (MouseCommand), MouseCommand.Name},
                {typeof (LClickCommand), LClickCommand.Name},
                {typeof (RClickCommand), RClickCommand.Name},
                {typeof (DblClickCommand), DblClickCommand.Name},
                {typeof (MouseDragDropCommand), MouseDragDropCommand.Name},
                {typeof (UseModelCommand), UseModelCommand.Name},
                {typeof (SetEnvironmentVariableCommand), SetEnvironmentVariableCommand.Name},
                {typeof (ProcessCommand), ProcessCommand.Name},
                {typeof (XpandHandleDialogCommand), XpandHandleDialogCommand.Name},
                {typeof (XpandFillFormCommand), XpandFillFormCommand.Name},
                {typeof (FocusControlCommand), FocusControlCommand.Name},
                {typeof (XpandFillRecordCommand), XpandFillRecordCommand.Name},
                {typeof (SaveFileDialogCommand), SaveFileDialogCommand.Name},
                {typeof (OpenFileDialogCommand), OpenFileDialogCommand.Name},
                {typeof (XpandAutoTestCommand), XpandAutoTestCommand.Name},
                {typeof (XpandCheckFieldValuesCommand), XpandCheckFieldValuesCommand.Name},
                {typeof (LogOffCommand), LogOffCommand.Name},
                {typeof (XpandCheckFileExistsCommand), XpandCheckFileExistsCommand.Name},
                {typeof (MoveWindowCommand), MoveWindowCommand.Name},
                {typeof (ResizeWindowCommand), ResizeWindowCommand.Name},
                {typeof (ScreenCaptureCommand), ScreenCaptureCommand.Name},
                {typeof (StopCommand), StopCommand.Name},
                {typeof (ToggleNavigationCommand), ToggleNavigationCommand.Name},
            };
            foreach (var keyValuePair in dictionary) {
                registerCommand.RegisterCommand(keyValuePair.Value, keyValuePair.Key);
            }
        }

        public static IDbConnection DbConnection(this TestDatabase database, string connectionString, string assemblyName, string typeName){
            return ReflectConnectionHelper.GetConnection(assemblyName,typeName,connectionString);
        }
    }
}