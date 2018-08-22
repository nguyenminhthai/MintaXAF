﻿namespace Xpand.Persistent.Base.Validation {
    public static class RuleDefaultMessageTemplates {
        public const string ValidFileName = @"""{TargetPropertyName}"" must not be empty.";
        public const string TargetProertiesMustNotBeEmpty = "At least one of {TargetProperties} must not be empty.";
        public const string InvalidTargetPropertyName = "Invalid {TargetPropertyName}";
    }
}