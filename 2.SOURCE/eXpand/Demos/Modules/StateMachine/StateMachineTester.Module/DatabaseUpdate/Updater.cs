using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine.Utils;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using StateMachineTester.Module.BusinessObjects;
using Xpand.ExpressApp.Security.Core;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.BaseImpl.Security;

namespace StateMachineTester.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            var defaultRole = (XpandPermissionPolicyRole)ObjectSpace.GetDefaultRole();

            var adminRole =  ObjectSpace.GetAdminRole("Admin");
            adminRole.GetUser("Admin");
            

            var userRole = (XpandPermissionPolicyRole) ObjectSpace.GetRole("User");
            var user = (XpandPermissionPolicyUser)userRole.GetUser("User");
            user.Roles.Add(defaultRole);
            userRole.SetTypePermission<XpoStateMachine>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
            userRole.SetTypePermission<XpoState>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
            userRole.SetTypePermission<XpoTransition>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
            userRole.SetTypePermission<PaymentTask>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
            userRole.SetTypePermission<Status>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
            
            CreateStatuses();
            ObjectSpace.CommitChanges();
            CreateStateMachines();
            ObjectSpace.CommitChanges();
        }

        private void CreateStatuses(){
            if (ObjectSpace.FindObject<Status>(null)==null){
                var captions = new[]{"Paid","Canceled","Pending","New"};
                foreach (var caption in captions){
                    var status = ObjectSpace.CreateObject<Status>();
                    status.Caption = caption;
                }
            }
        }

        private void CreateStateMachines(){
            if (ObjectSpace.QueryObject<XpoStateMachine>(machine => machine.Name=="Payment")==null){
                var stateMachine = ObjectSpace.CreateObject<XpoStateMachine>();
                stateMachine.Name = "Payment";
                stateMachine.Active = true;
                stateMachine.TargetObjectType = typeof (PaymentTask);
                stateMachine.StatePropertyName = new StringObject("PaymentStatus");
                stateMachine.States.AddRange(CreatePaymentStatusStates());
                stateMachine.StartState = stateMachine.States.First(state => state.Caption=="New");
                
                stateMachine = ObjectSpace.CreateObject<XpoStateMachine>();
                stateMachine.Name = "Bill";
                stateMachine.Active = true;
                stateMachine.TargetObjectType = typeof (PaymentTask);
                stateMachine.StatePropertyName = new StringObject("BillStatus");
                stateMachine.States.AddRange(CreateBillStatusStates());
                stateMachine.StartState = stateMachine.States.First(state => state.Caption=="Active");
            }
        }

        private IEnumerable<XpoState> CreateBillStatusStates(){
            var states = new List<XpoState>();

            var activeState = CreateState("Active", BillStatus.Active);
            states.Add(activeState);
            var inDisputeState = CreateState("InDispute", BillStatus.InDispute);
            states.Add(inDisputeState);
            var overdueState = CreateState("Overdue", BillStatus.Overdue);
            states.Add(overdueState);

            activeState.AddTransition(inDisputeState);
            inDisputeState.AddTransition(overdueState);

            return states;
        }

        private List<XpoState> CreatePaymentStatusStates(){
            var states = new List<XpoState>();
            var canceled = CreateState("Canceled", ObjectSpace.QueryObject<Status>(status => status.Caption == "Canceled"));
            states.Add(canceled);
            var pending = CreateState("Pending", ObjectSpace.QueryObject<Status>(status => status.Caption == "Pending"));
            states.Add(pending);
            var paid = CreateState("Paid", ObjectSpace.QueryObject<Status>(status => status.Caption == "Paid"));
            states.Add(paid);
            var newState = CreateState("New", ObjectSpace.QueryObject<Status>(status => status.Caption == "New"));
            states.Add(newState);

            newState.AddTransition(canceled);
            newState.AddTransition(paid);
            newState.AddTransition(pending);

            pending.AddTransition(paid);
            pending.AddTransition(canceled);

            return states;
        }

        private XpoState CreateState(string caption, object marker){
            var state = ObjectSpace.CreateObject<XpoState>();
            state.Caption = caption;
            state.Marker=new MarkerObject(marker);
            return state;
        }

    }
}
