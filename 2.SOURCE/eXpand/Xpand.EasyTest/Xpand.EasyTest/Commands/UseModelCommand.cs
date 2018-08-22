﻿using System;
using System.IO;
using System.Linq;
using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;

namespace Xpand.EasyTest.Commands{
    public enum ModelOperation{
        None,
        LoadModel,
        MergeModel
    }
    public class UseModelCommand:Command{
        private const string Operation = "Operation";
        private TestParameters _testParameters;
        public const string Name = "UseModel";


        protected override void InternalExecute(ICommandAdapter adapter){
            EasyTestTracer.Tracer.InProcedure(GetType().Name + ".Execute");
            EasyTestTracer.Tracer.LogText(Environment.NewLine + Text);
            var binPath = this.GetBinPath();
            if (string.IsNullOrEmpty(Parameters.MainParameter.Value)){
                var modelFile = Directory.GetFiles(_testParameters.ScriptsPath, "*.xafml", SearchOption.TopDirectoryOnly).Single();
                CopyModel(adapter, binPath, GetUserXafml(modelFile), modelFile);
            }
            else{
                var modelFiles = Parameters.MainParameter.Value.Split(';');
                for (int i = 0; i < modelFiles.Length; i++){
                    var path = Path.Combine(_testParameters.ScriptsPath, modelFiles[i] + ".xafml");
                    var userXafml = GetUserXafml(path,i);
                    CopyModel(adapter, binPath, userXafml, path);
                }
            }

            EasyTestTracer.Tracer.OutProcedure(GetType().Name + ".Execute");
        }

        private string GetUserXafml(string model, int i = 0){
            if (this.ParameterValue<ModelOperation>(Operation)!=ModelOperation.None)
                return Path.GetFileName(model);
            string userXafml = "Model.User.xafml";
            if (i > 0)
                userXafml = "Model.User" + i + ".xafml";
            return userXafml;
        }

        private void CopyModel(ICommandAdapter adapter, string binPath, string model, string modelFile){
            var copyFileCommand = new CopyFileCommand();
            string destinationFile = Path.Combine(binPath, model);
            var deleteFileCommand = new DeleteFileCommand();
            deleteFileCommand.Parameters.MainParameter = new MainParameter(destinationFile);
            deleteFileCommand.Execute(adapter);
            copyFileCommand.Execute(adapter, _testParameters, modelFile, destinationFile);
            if (this.ParameterValue<ModelOperation>(Operation)!=ModelOperation.None){
                var actionCommand = new ActionCommand();
                actionCommand.DoAction(adapter, "Parameter", Path.GetFileNameWithoutExtension(model));
                actionCommand.DoAction(adapter, "EasyTestAction", this.ParameterValue<ModelOperation>(Operation).ToString());
            }
        }

        public override void ParseCommand(CommandCreationParam commandCreationParam){
            base.ParseCommand(commandCreationParam);
            _testParameters = commandCreationParam.TestParameters;
        }
    }
}