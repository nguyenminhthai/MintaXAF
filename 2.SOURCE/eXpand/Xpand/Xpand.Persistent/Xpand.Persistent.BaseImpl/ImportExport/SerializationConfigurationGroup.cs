﻿using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Xpand.Persistent.Base;
using Xpand.Persistent.Base.ImportExport;

namespace Xpand.Persistent.BaseImpl.ImportExport {
    [DefaultClassOptions]
    [NavigationItem("ImportExport")]
    [DefaultProperty("Name")]
    public class SerializationConfigurationGroup : XpandBaseCustomObject, ISerializationConfigurationGroup {
        private string _name;

        public SerializationConfigurationGroup(Session session)
            : base(session) {
        }

        [RuleUniqueValue(DefaultContexts.Save)]
        [RuleRequiredField]
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [Association("SerializationConfigurationGroup-SerializationConfigurations"), Aggregated]
        public XPCollection<SerializationConfiguration> SerializationConfigurations => GetCollection<SerializationConfiguration>("SerializationConfigurations");

        IList<ISerializationConfiguration> ISerializationConfigurationGroup.Configurations => new ListConverter<ISerializationConfiguration, SerializationConfiguration>(SerializationConfigurations);
    }
}