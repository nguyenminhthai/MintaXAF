﻿using System.Collections.Generic;
using DevExpress.ExpressApp.Security;
using Xpand.Persistent.Base.StateMachine;

namespace Xpand.ExpressApp.StateMachine.Security.Improved {
    public class StateMachineTransitionPermission : OperationPermissionBase, IStateMachineTransitionPermission {
        public const string OperationName = "StateMachineTransition";
        public StateMachineTransitionPermission(IStateMachineTransitionPermission permission)
            : base(OperationName) {
            StateCaption=permission.StateCaption;
            StateMachineName=permission.StateMachineName;
            Hide=permission.Hide;
        }

        public StateMachineTransitionPermission() : base(OperationName) {
        }

        public bool Hide { get; set; }

        public override IList<string> GetSupportedOperations() {
            return new[] { OperationName };
        }
        
        public string StateMachineName { get; set; }
        public string StateCaption { get; set; }
    }
}
