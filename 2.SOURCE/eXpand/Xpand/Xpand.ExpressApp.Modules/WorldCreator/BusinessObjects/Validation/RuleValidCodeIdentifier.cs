﻿using System.Collections.ObjectModel;
using DevExpress.Persistent.Validation;
using Xpand.Persistent.Base.PersistentMetaData;
using StringExtensions = Xpand.Utils.Helpers.StringExtensions;

namespace Xpand.ExpressApp.WorldCreator.BusinessObjects.Validation{
    [CodeRule]
    public class RuleValidCodeIdentifier : RuleBase<IPersistentTypeInfo> {
        public const string ErrorMessage = "This is not a valid code Identifier";

        public RuleValidCodeIdentifier() : base("", "Save"){
        }

        public RuleValidCodeIdentifier(IRuleBaseProperties properties) : base(properties){
            properties.CustomMessageTemplate = ErrorMessage;
        }

        public override ReadOnlyCollection<string> UsedProperties{
            get{
                IPersistentClassInfo persistentClassInfo;
                return new ReadOnlyCollection<string>(new[]{nameof(persistentClassInfo.Name)});
            }
        }

        protected override bool IsValidInternal(IPersistentTypeInfo target, out string errorMessageTemplate){
            errorMessageTemplate = ErrorMessage;
            return StringExtensions.CleanCodeName(target.Name)==target.Name;
        }
    }
}