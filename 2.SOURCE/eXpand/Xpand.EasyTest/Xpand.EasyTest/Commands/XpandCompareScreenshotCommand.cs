﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;
using Xpand.EasyTest.Commands.InputSimulator;
using Xpand.EasyTest.Commands.Window;
using Xpand.Utils.Automation;
using Xpand.Utils.Win32;
using Image = System.Drawing.Image;

namespace Xpand.EasyTest.Commands {
    public class XpandCompareScreenshotCommand : CompareScreenshotCommand {
        public const string Name = "XpandCompareScreenshot";
        private const string WinMaskRectangle = "WinMaskRectangle";
        private const string WebMaskRectangle = "WebMaskRectangle";
        private const string MaskRectangle = "MaskRectangle";
        private const string ValidDiffPercentage = "ValidDiffPercentage";

        readonly Size _defaultWindowSize = new Size(1024, 768);
        private bool _additionalCommands;

        protected override void InternalExecute(ICommandAdapter adapter){
            EasyTestTracer.Tracer.LogText("MainParameter=" + Parameters.MainParameter.Value);
            var activeWindowControl = adapter.CreateTestControl(TestControlType.Dialog, "");
            var windowHandleInfo = GetWindowHandle(activeWindowControl);
            if (!windowHandleInfo.Value)
                ExecuteAdditionalCommands(adapter);
            GetRootWindow(windowHandleInfo.Key,adapter);
            var testImage = GetTestImage(windowHandleInfo.Key);
            var filename = GetFilename(adapter);
            var path = Path.GetDirectoryName(filename)+"";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            try {
                if (File.Exists(filename)) {
                    CompareAndSave(filename, testImage, adapter);
                } else {
                    SaveActualImage(testImage, filename);
                    throw new CommandException($"'{filename}' master copy was not found", StartPosition);
                }
            } finally {
                if (!windowHandleInfo.Value && this.ParameterValue(ToggleNavigationCommand.Name, true) && _additionalCommands){
                    var toggleNavigationCommand = new ToggleNavigationCommand();
                    toggleNavigationCommand.Execute(adapter);
                }
            }
        }

        private void CompareAndSave(string filename, Image testImage, ICommandAdapter adapter) {
            var threshold = this.ParameterValue<byte>("ImageDiffThreshold", 3);
            var localImage = Image.FromFile(filename);
            var masks = GetMasks(adapter).ToArray();
            var validDiffPercentace = this.ParameterValue(ValidDiffPercentage, 10);
            if (!masks.Any()) {
                var maskRectangle = GetMaskRectangle(adapter);
                if (maskRectangle != Rectangle.Empty){
                    SaveMask(filename, localImage, maskRectangle, ".mask");
                }
                else {
                    maskRectangle=new Rectangle(0,0,testImage.Width-1,testImage.Height-1);
                }
                var isValidPercentage = IsValidPercentage(testImage, maskRectangle, threshold, validDiffPercentace, localImage);
                if (!isValidPercentage)
                    SaveImages(filename, testImage, threshold, localImage, maskRectangle);
            }
            var height = this.ParameterValue("MaskHeight", 0);
            var width = this.ParameterValue("MaskWidth", 0);
            foreach (var mask in masks) {
                var maskImage = mask;
                if (width > 0 && height > 0) {
                    maskImage = (Bitmap)maskImage.ResizeRectangle(width, height);
                }
                var isValidPercentage = IsValidPercentage(testImage, maskImage, threshold, validDiffPercentace, localImage);
                if (!isValidPercentage)
                    SaveImages(filename, testImage, threshold, localImage, maskImage);
            }
        }

        private void SaveMask(string filename, Image localImage, Rectangle maskRectangle, string maskNameSuffix){
            using (var image = CreateMask(localImage, maskRectangle)){
                var path = Path.Combine(Path.GetDirectoryName(filename) + "",
                    Path.GetFileNameWithoutExtension(filename) +maskNameSuffix+ ".png");
                image.Save(path, ImageFormat.Png);
            }
        }

        protected  Image CreateMask(Image image, Rectangle maskRectangle){
            var mask = AForge.Imaging.Image.Clone((Bitmap) image,PixelFormat.Format24bppRgb);
            using (var graphics = Graphics.FromImage(mask)){
                using (var crop = image.Crop(maskRectangle)){
                    graphics.DrawImage(crop, maskRectangle, new Rectangle(0, 0, crop.Width, crop.Height), GraphicsUnit.Pixel);
                    graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 3), maskRectangle);
                }
            }
            return mask;
        }

        private bool IsValidPercentage(Image testImage, Rectangle maskRectangle, byte threshold, int validDiffPercentace, Image localImage) {
            var differences = localImage.Differences(testImage, maskRectangle, threshold);
            return IsValidPercentageCore(validDiffPercentace, differences);
        }

        private bool IsValidPercentage(Image testImage, Bitmap maskImage, byte threshold, int validDiffPercentace, Image localImage) {
            var differences = localImage.Differences(testImage, maskImage, threshold);
            return IsValidPercentageCore(validDiffPercentace, differences);
        }

        private static bool IsValidPercentageCore(int validDiffPercentace, IEnumerable<KeyValuePair<byte[,], float>> differences) {
            return differences.FirstOrDefault(pair => pair.Value < validDiffPercentace).Key != null;
        }

        private void SaveImages(string filename, Image testImage, byte threshold, Image localImage, Rectangle maskRectangle) {
            var differences = localImage.Differences(testImage, maskRectangle, threshold);
            SaveImagesCore(differences, filename, testImage,maskRectangle);
        }

        private void SaveImages(string filename, Image testImage, byte threshold, Image localImage, Bitmap maskImage) {
            var differences = localImage.Differences(testImage, maskImage, threshold);
            SaveImagesCore(differences, filename, testImage,Rectangle.Empty);
        }

        public new void SaveActualImage(Image actualImage,string etalonFileName){
            string filename =
                $"{ Path.Combine(Path.GetDirectoryName(etalonFileName)+"", Path.GetFileNameWithoutExtension(etalonFileName)+"")}.Actual{ Path.GetExtension(etalonFileName)}";
            actualImage.Save(filename, ImageFormat.Png);
        }

        private void SaveImagesCore(IEnumerable<KeyValuePair<byte[,], float>> differences, string filename, Image testImage, Rectangle maskRectangle) {
            var valuePairs = differences.OrderBy(pair => pair.Value);
            var keyValuePair = valuePairs.First();
            var diffImage = keyValuePair.Key.DiffImage(true, true);
            SaveDiffImage(diffImage, filename);
            if (maskRectangle!=Rectangle.Empty)
                SaveMask(filename, testImage, maskRectangle, ".mask.Actual");
            SaveActualImage(testImage, filename);
            throw new CommandException(String.Format("A screenshot of the active window differs {1}% from the '{0}' master copy", filename, Math.Round(keyValuePair.Value)), StartPosition);
        }

        private Rectangle GetMaskRectangle(ICommandAdapter adapter) {
            var parameterName = adapter.IsWinAdapter() ? WinMaskRectangle : WebMaskRectangle;
            var rectangle = this.ParameterValue<Rectangle>(parameterName);
            return rectangle == Rectangle.Empty ? this.ParameterValue<Rectangle>(MaskRectangle) : rectangle;
        }

        private static Image GetTestImage(IntPtr windowHandle) {
            Image testImage;
            try {
                testImage = windowHandle.GetScreenshot();
                EasyTestTracer.Tracer.LogText("Captured image for window with handle {0} and title {1}", windowHandle, windowHandle.WindowText());
            } catch (Exception e) {
                EasyTestTracer.Tracer.LogText("Exception:" + e.Message);
                testImage = new Bitmap(100, 100);
            }
            return testImage;
        }

        private string GetFilename(ICommandAdapter adapter) {
            var filename = ScriptsPath + "\\Images\\" + Parameters.MainParameter.Value;
            return adapter.GetPlatformSuffixedPath(filename);
        }

        private KeyValuePair<IntPtr, bool> GetWindowHandle(ITestControl activeWindowControl) {
            var isCustom = false;
            var screenMainWindow = this.ParameterValue("ScreenMainWindow", false);
            IntPtr windowHandle = activeWindowControl.GetInterface<ITestWindow>().GetActiveWindowHandle();
            if (screenMainWindow){
                windowHandle = GetRootWindow(windowHandle.ToInt32());
            }

            Parameter windowNameParameter = Parameters["WindowTitle"];
            if (windowNameParameter != null) {
                isCustom = true;
                windowHandle = Win32Declares.WindowHandles.FindWindowByCaption(IntPtr.Zero, windowNameParameter.Value);
                if (windowHandle == IntPtr.Zero)
                    throw new CommandException($"Cannot find window {windowNameParameter.Value}", StartPosition);
            }

            return new KeyValuePair<IntPtr, bool>(windowHandle, isCustom);
        }

        protected virtual IntPtr GetRootWindow(IntPtr windowHandle, ICommandAdapter commandAdapter){
            return GetRootWindow(windowHandle.ToInt32());
        }

        private void ExecuteAdditionalCommands(ICommandAdapter adapter) {
            _additionalCommands = this.ParameterValue("AdditionalCommands", true);
            if (!_additionalCommands)
                return;
            if (this.ParameterValue(ToggleNavigationCommand.Name, true)){
                var toggleNavigationCommand = new ToggleNavigationCommand();
                toggleNavigationCommand.Execute(adapter);
            }
            var activeWindowSize = this.ParameterValue("ActiveWindowSize", _defaultWindowSize);
            var resizeWindowCommand = new ResizeWindowCommand();
            resizeWindowCommand.Parameters.MainParameter = new MainParameter(
                $"{activeWindowSize.Width}x{activeWindowSize.Height}");
            resizeWindowCommand.Execute(adapter);

            if (this.ParameterValue(HideCursorCommand.Name, true)) {
                var hideCaretCommand = new HideCursorCommand();
                hideCaretCommand.Execute(adapter);
            }

            if (this.ParameterValue(KillFocusCommand.Name, true)) {
                var helperAutomation = new HelperAutomation();
                Win32Declares.Message.SendMessage(helperAutomation.GetFocusControlHandle(), Win32Declares.Message.EM_SETSEL, -1, 0);
                var hideCaretCommand = new KillFocusCommand();
                hideCaretCommand.Execute(adapter);
            }

            var sendKeys = this.ParameterValue<string>("SendKeys");
            if (!string.IsNullOrEmpty(sendKeys)) {
                var sendKeysCommand = new SendKeysCommand();
                sendKeysCommand.Parameters.MainParameter = new MainParameter(sendKeys);
                sendKeysCommand.Execute(adapter);
            }

            Wait(adapter, 1000);
        }

        private static void Wait(ICommandAdapter adapter, int interval) {
            var sleepCommand = new SleepCommand();
            sleepCommand.Parameters.MainParameter = new MainParameter(interval.ToString(CultureInfo.InvariantCulture));
            sleepCommand.Execute(adapter);
        }

        private IEnumerable<Bitmap> GetMasks(ICommandAdapter adapter) {
            var parameter = Parameters["Mask"];
            if (parameter != null) {
                foreach (var p in GetMaskFileNames(adapter, parameter)) yield return (Bitmap)Image.FromFile(p);
            }
            parameter = Parameters["XMask"];
            if (parameter != null) {
                parameter.Value = string.Join(";", parameter.Value.Split(';').Select(s => "/regX/" + s));
                foreach (var p in GetMaskFileNames(adapter, parameter)) yield return (Bitmap)Image.FromFile(p);
            }
        }

        private IEnumerable<string> GetMaskFileNames(ICommandAdapter adapter, Parameter parameter) {
            foreach (string maskPath in parameter.Value.Split(';')) {
                string maskFileName;
                if (maskPath.StartsWith("/regX/")) {
                    string path = Extensions.GetXpandPath(ScriptsPath);
                    maskFileName = Path.Combine(path,
                        @"Xpand.EasyTest\Resources\Masks\" + maskPath.TrimStart("/regX/".ToCharArray()));
                } else
                    maskFileName = ScriptsPath + "\\Images\\" + maskPath;
                yield return adapter.GetPlatformSuffixedPath(maskFileName);
            }
        }
    }
}