﻿using System.Linq;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata.Helpers;
using Xpand.Persistent.Base.General;

namespace Xpand.ExpressApp.AuditTrail {
    public class NoAuditProccesor : ObjectAuditProcessor {
        public NoAuditProccesor(Session session, AuditTrailSettings settings)
            : base(session, settings) {
        }

        public override bool IsObjectAudited(object obj) {
            return false;
        }
    }

    public class XpandObjectAuditProcessorsFactory : ObjectAuditProcessorsFactory {
        public override bool IsSuitableAuditProcessor(ObjectAuditProcessor processor, ObjectAuditingMode mode) {
            var isNoAuditMode = mode == (ObjectAuditingMode)Persistent.Base.AuditTrail.ObjectAuditingMode.None;
            if (isNoAuditMode) {
                return processor is NoAuditProccesor;
            }
            return base.IsSuitableAuditProcessor(processor, mode);
        }

        public override ObjectAuditProcessor CreateAuditProcessor(ObjectAuditingMode mode, Session session, AuditTrailSettings settings) {
            var auditTrailSettings = new AuditTrailSettings();
            auditTrailSettings.SetXPDictionary(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary);
            foreach (var auditTrailClassInfo in settings.TypesToAudit) {
                var auditTrailMemberInfos = auditTrailClassInfo.Properties;
                if (!(auditTrailClassInfo.ClassInfo is IntermediateClassInfo))
                    auditTrailSettings.AddType(auditTrailClassInfo.ClassInfo.ClassType, auditTrailMemberInfos.Select(info => info.Name).ToArray());
            }
            return mode == (ObjectAuditingMode)Persistent.Base.AuditTrail.ObjectAuditingMode.None ? new NoAuditProccesor(session, auditTrailSettings) : base.CreateAuditProcessor(mode, session, auditTrailSettings);
        }
    }

}
