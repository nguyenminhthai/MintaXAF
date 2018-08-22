﻿using DevExpress.EasyTest.Framework;
using DevExpress.EasyTest.Framework.Commands;

namespace Xpand.EasyTest.Commands {
    public class CreatePermissionCommand : Command,IRequireApplicationOptions {
        private TestApplication _applicationOptions;
        public const string Name = "CreatePermission";
        protected override void InternalExecute(ICommandAdapter adapter) {
            NavigateToRole(adapter);
            foreach (var targetType in Parameters.MainParameter.Value.Split(';')) {
                ProccessUserRole(adapter);
                CreateNewTypePermission(adapter, targetType);
            }
            NavigateToRole(adapter);
        }

        private void CreateNewTypePermission(ICommandAdapter adapter, string targetType) {
            var actionCommand = new ActionCommand();
            actionCommand.Parameters.MainParameter = new MainParameter("Type Permissions");
            actionCommand.Parameters.ExtraParameter = new MainParameter();
            actionCommand.Execute(adapter);
            actionCommand.Parameters.MainParameter = new MainParameter("Type Permissions.New");
            actionCommand.Parameters.ExtraParameter = new MainParameter();
            actionCommand.Execute(adapter);

            var fillFormCommand = new FillFormCommand();
            fillFormCommand.Parameters.Add(new Parameter("Target Type", targetType, true, StartPosition));
            fillFormCommand.Parameters.Add(new Parameter("Read", this.ParameterValue("Read", "Allow"), true, StartPosition));
            fillFormCommand.Parameters.Add(new Parameter("Write", this.ParameterValue("Write", "Allow"), true, StartPosition));
            fillFormCommand.Parameters.Add(new Parameter("Delete", this.ParameterValue("Delete", "Allow"), true, StartPosition));
            fillFormCommand.Parameters.Add(new Parameter("Create", this.ParameterValue("Create", "Allow"), true, StartPosition));
            fillFormCommand.Parameters.Add(new Parameter("Navigate", this.ParameterValue("Navigate", "Allow"), true, StartPosition));
            fillFormCommand.Execute(adapter);

            var saveAndCloseCommand = new SaveAndCloseCommand();
            saveAndCloseCommand.Execute(adapter);

            saveAndCloseCommand = new SaveAndCloseCommand();
            saveAndCloseCommand.Execute(adapter);
        }

        private void ProccessUserRole(ICommandAdapter adapter) {
            var processRecordCommand = new XpandProcessRecordCommand();
            processRecordCommand.SetApplicationOptions(_applicationOptions);
            processRecordCommand.Parameters.MainParameter = new MainParameter("");
            processRecordCommand.Parameters.Add(new Parameter("Name", this.ParameterValue("EditRole", "User"), true, StartPosition));
            processRecordCommand.Parameters.Add(new Parameter("Action", "Edit", true, StartPosition));
            processRecordCommand.Execute(adapter);
        }

        private void NavigateToRole(ICommandAdapter adapter) {
            var navigateCommand = new NavigateCommand();
            navigateCommand.Parameters.MainParameter = new MainParameter(this.ParameterValue("RolePath", "Default.Role"));
            navigateCommand.Execute(adapter);
        }

        public void SetApplicationOptions(TestApplication applicationOptions){
            _applicationOptions = applicationOptions;
        }
    }
}
