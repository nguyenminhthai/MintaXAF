﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AForge.Video;
using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;
using DevExpress.EasyTest.Framework.Loggers;
using ScreenCaptureStream = Xpand.EasyTest.ScreenCaptureStream;

namespace Xpand.EasyTest.Commands{
    public class ScreenCaptureCommand:FilesCommand{
        public const string Name = "ScreenCapture";
        private const string ImagesPath = @"\Images\ScreenCapture";
        private string _fileName;
        private Size _size;
        private static ScreenCaptureStream _screenCaptureStream;
        private long _index;
        private Point _topLeft;
        private string _platformSuffix;

        public override void ParseCommand(CommandCreationParam commandCreationParam){
            base.ParseCommand(commandCreationParam);
            _size = this.ParameterValue("Size", new Size(1024, 768));
            _topLeft = this.ParameterValue("TopLeft", new Point(0, 0));
            var videoDir = ScriptsPath + ImagesPath;
            var testName = Logger.Instance.GetLogger<FileLogger>().CurrentTestLog.Name;
            _fileName = Path.Combine(videoDir, testName);
            _platformSuffix = this.IsWinCommand() ? ".Win" : ".Web";
            if (!Directory.Exists(videoDir))
                Directory.CreateDirectory(videoDir);
            else{
                var files = Directory.GetFiles(videoDir, testName + "*"+_platformSuffix+".bmp");
                for (int index = files.Length - 1; index >= 0; index--) {
                    File.Delete(files[index]);
                }
            }
        }

        protected override void InternalExecute(ICommandAdapter adapter){
            var frameInterval = this.ParameterValue("FrameInterval",1000);
            _screenCaptureStream = new ScreenCaptureStream(new Rectangle(_topLeft, _size), frameInterval);
            _screenCaptureStream.NewFrame+=ScreenCaptureStreamOnNewFrame;
//            _screenCaptureStream.Start();
        }

        private void ScreenCaptureStreamOnNewFrame(object sender, NewFrameEventArgs e){
            string suffix = _index + _platformSuffix;
            var filename = _fileName+suffix+".png";
            e.Frame.Save(filename,ImageFormat.Png);
            _index++;
        }

        public static void Stop(bool isWinApp){
            if (_screenCaptureStream != null){
                _screenCaptureStream.Stop();
                var logger = Logger.Instance.GetLogger<FileLogger>();
                if (logger.CurrentTestLog.Result != "Failed")
                    foreach (var file in Directory.GetFiles(logger.LogPath+ ImagesPath, "*.win.png")){
                        File.Delete(file);
                    }
            }
        }
    }
}