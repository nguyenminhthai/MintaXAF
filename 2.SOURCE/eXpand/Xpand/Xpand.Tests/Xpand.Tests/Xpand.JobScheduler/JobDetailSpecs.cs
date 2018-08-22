﻿using System.Linq;
using DevExpress.ExpressApp;
using Machine.Specifications;
using Quartz;
using Xpand.Persistent.BaseImpl.JobScheduler;
using Xpand.Persistent.BaseImpl.JobScheduler.Triggers;
using Xpand.ExpressApp.JobScheduler.QuartzExtensions;

namespace Xpand.Tests.Xpand.JobScheduler {
    [Ignore("")]
    public class When_new_Job_detail_saved : With_Job_Scheduler_XpandJobDetail_Application<When_new_Job_detail_saved> {
        static IJobDetail _jobDetail;
        Because of = () => XPObjectSpace.CommitChanges();

        It should_add_a_new_job_detail_to_the_scheduler =
            () => {
                _jobDetail = Scheduler.GetJobDetail(Object);
                _jobDetail.ShouldNotBeNull();
            };

        It should_have_as_group_the_jobtype_plus_the_name_of_the_job_Detail = () => _jobDetail.Key.Group.ShouldEqual(Object.Job.JobType.FullName);


        It should_shutdown_the_scheduler = () => Scheduler.Shutdown(false);
    }
    [Ignore("")]
    public class When_new_Job_detail_with_group_assigned_saved : With_Job_Scheduler_XpandJobDetail_Application<When_new_Job_detail_with_group_assigned_saved> {
        Establish context = () => {
            var jobSchedulerGroup = XPObjectSpace.FindObject<JobSchedulerGroup>(null);
            Object.Group = jobSchedulerGroup;
        };

        protected override void ViewCreated(DetailView detailView) {
            base.ViewCreated(detailView);
            var XPObjectSpace = Application.CreateObjectSpace();
            var jobSchedulerGroup = XPObjectSpace.CreateObject<JobSchedulerGroup>();
            jobSchedulerGroup.Name = "gr";
            var xpandSimpleTrigger = XPObjectSpace.CreateObject<XpandSimpleTrigger>();
            xpandSimpleTrigger.JobSchedulerGroups.Add(jobSchedulerGroup);
            XPObjectSpace.CommitChanges();
        }
        Because of = () => XPObjectSpace.CommitChanges();

        It should_create_triggers_for_that_group = () => Scheduler.GetTriggersOfJob(Object).Count().ShouldEqual(1);

        It should_shutdown_the_scheduler = () => Scheduler.Shutdown(false);
    }
    [Ignore("")]
    public class When_Job_detail_Deleted : With_Job_Scheduler_XpandJobDetail_Application<When_Job_detail_Deleted> {

        Establish context = () => {
            var xpandSimpleTrigger = XPObjectSpace.CreateObject<XpandSimpleTrigger>();
            xpandSimpleTrigger.JobDetails.Add(Object);
            XPObjectSpace.CommitChanges();
            XPObjectSpace.Delete(Object);
        };

        Because of = () => XPObjectSpace.CommitChanges();

        It should_remove_it_from_the_scheduler =
            () => Scheduler.GetJobDetail(Object).ShouldBeNull();

        It should_remove_the_listener_from_the_scheduler =
            () => Scheduler.ListenerManager.GetTriggerListener("DummyJobListener").ShouldBeNull();
        It should_shutdown_the_scheduler = () => Scheduler.Shutdown(false);
    }
    [Ignore("")]
    public class When_Job_detail_updated : With_Job_Scheduler_XpandJobDetail_Application<When_Job_detail_updated> {

        Establish context = () => {
            XPObjectSpace.CommitChanges();
            Object.Description = "new_description";
        };

        Because of = () => XPObjectSpace.CommitChanges();
        protected override void ViewCreated(DetailView detailView) {
            base.ViewCreated(detailView);
            var xpandJobTrigger = XPObjectSpace.CreateObject<XpandSimpleTrigger>();
            xpandJobTrigger.Name = "trigger";
            Object.JobTriggers.Add(xpandJobTrigger);
        }

        It should_change_the_value_in_the_scheduler =
            () => Scheduler.GetJobDetail(Object).Description.ShouldEqual("new_description");

        It should_have_the_same_number_of_triggers = () => Scheduler.GetTriggersOfJob(Object).Count().ShouldEqual(1);

        It should_shutdown_the_scheduler = () => Scheduler.Shutdown(false);
    }
    [Ignore("")]
    public class When_Job_Detail_is_linked_with_triggers : With_Job_Scheduler_XpandJobDetail_Application<When_Job_Detail_is_linked_with_triggers> {
        Establish context = () => {
            var xpandSimpleTrigger = XPObjectSpace.CreateObject<XpandSimpleTrigger>();
            xpandSimpleTrigger.Name = "trigger";
            Object.JobTriggers.Add(xpandSimpleTrigger);
        };


        Because of = () => XPObjectSpace.CommitChanges();

        It should_add_one_trigger_to_the_Schedule_job = () => Scheduler.GetTriggersOfJob(Object).Count.ShouldEqual(1);

        It should_shutdown_the_scheduler = () => Scheduler.Shutdown(false);
    }
    [Ignore("")]
    public class When_Job_Detail_is_unlinked_with_triggers : With_Job_Scheduler_XpandJobDetail_Application<When_Job_Detail_is_unlinked_with_triggers> {

        Establish context = () => {
            var xpandSimpleTrigger = XPObjectSpace.CreateObject<XpandSimpleTrigger>();
            xpandSimpleTrigger.Name = "trigger";
            Object.JobTriggers.Add(xpandSimpleTrigger);
            XPObjectSpace.CommitChanges();
            xpandSimpleTrigger.Delete();
        };


        Because of = () => XPObjectSpace.CommitChanges();

        It should_remove_the_trigger_from_the_schedule_job = () => Scheduler.GetTriggersOfJob(Object).Count.ShouldEqual(0);

        It should_shutdown_the_scheduler = () => Scheduler.Shutdown(false);
    }
}
