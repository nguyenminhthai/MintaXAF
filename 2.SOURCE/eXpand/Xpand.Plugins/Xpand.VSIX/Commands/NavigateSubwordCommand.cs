﻿using System;
using System.ComponentModel.Design;
using System.Linq;
using EnvDTE;
using Xpand.VSIX.Extensions;
using Xpand.VSIX.Options;
using Xpand.VSIX.VSPackage;

namespace Xpand.VSIX.Commands {
    public abstract class SubwordNavigateCommand: VSCommand {
        protected SubwordNavigateCommand(EventHandler invokeHandler, CommandID commandID) : base(invokeHandler, commandID){
//            this.EnableForActiveFile();
        }

        protected static void Navigate(bool previous){
            if (DteExtensions.DTE.ActiveDocument.Selection is TextSelection textSelection && textSelection.IsEmpty){
                int column = 0;
                if (previous){
                    textSelection.WordLeft(true);
                    for (int i = textSelection.Text.Length - 1; i >= 0; i--) {
                        var substring = textSelection.Text.Substring(i, 1);
                        if (IsEndPoint(substring)) {
                            column = textSelection.AnchorColumn - (textSelection.Text.Length - i);
                            break;
                        }
                    }
                }
                else{
                    textSelection.WordRight(true);
                    column = textSelection.AnchorColumn + textSelection.Text.Length;
                    for (int i = 1; i < textSelection.Text.Length ; i++) {
                        var substring = textSelection.Text.Substring(i, 1);
                        if (IsEndPoint(substring)) {
                            column = textSelection.AnchorColumn + i;
                            break;
                        }
                    }
                }
                textSelection.MoveToDisplayColumn(textSelection.CurrentLine, column);
            }
        }


        private static bool IsEndPoint(string substring){
            return char.IsUpper(Convert.ToChar(substring));
        }
    }

    public class NavigateNextSubwordCommand : SubwordNavigateCommand{
        private NavigateNextSubwordCommand() : base(
             (sender, args) => Navigate( false),
            new CommandID(PackageGuids.guidVSXpandPackageCmdSet, PackageIds.cmdidNextSubword)){
            var dteCommand = OptionClass.Instance.DteCommands.FirstOrDefault(command => command.Command == GetType().Name);
            BindCommand(dteCommand);
        }

        public static void Init() {
            new NavigateNextSubwordCommand();
        }
    }

    public class NavigatePreviousSubwordCommand : SubwordNavigateCommand{
        private NavigatePreviousSubwordCommand() : base(
             (sender, args) => Navigate( true),
            new CommandID(PackageGuids.guidVSXpandPackageCmdSet, PackageIds.cmdidPreviousSubword)){
            var dteCommand = OptionClass.Instance.DteCommands.FirstOrDefault(command => command.Command == GetType().Name);
            BindCommand(dteCommand);
        }

        public static void Init() {
            new NavigatePreviousSubwordCommand();
        }    
    }

}
