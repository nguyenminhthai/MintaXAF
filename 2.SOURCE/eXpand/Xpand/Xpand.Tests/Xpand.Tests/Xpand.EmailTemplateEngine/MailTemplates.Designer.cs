﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Xpand.Tests.Xpand.EmailTemplateEngine {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MailTemplates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MailTemplates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Xpand.Tests.Xpand.EmailTemplateEngine.MailTemplates", typeof(MailTemplates).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///	&lt;head&gt;
        ///		&lt;title&gt;@Model.Subject&lt;/title&gt;
        ///	&lt;/head&gt;
        ///	&lt;body&gt;
        ///		&lt;p&gt;Dear @Model.Name, &lt;/p&gt;
        ///		&lt;p&gt;An account has been created for you.&lt;/p&gt;
        ///		&lt;p&gt;Your account is FREE and allows you to perform bla bla features.&lt;/p&gt;
        ///		&lt;p&gt;To login and complete your profile, please go to:&lt;/p&gt;
        ///		&lt;p&gt;&lt;a href=&quot;@Model.LogOnUrl&quot;&gt;@Model.LogOnUrl&lt;/a&gt;&lt;/p&gt;
        ///		&lt;p&gt;Your User ID is your email address and password is: @Model.Password&lt;/p&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string HtmlTemplate {
            get {
                return ResourceManager.GetString("HtmlTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @{
        ///	From = Model.From;
        ///	To.Add(Model.To);
        ///	CC.Add(Model.CC);
        ///	Bcc.Add(Model.Bcc);
        ///	Headers.Add(&quot;key&quot;, &quot;value&quot;);
        ///	Subject = Model.Subject;
        ///}.
        /// </summary>
        internal static string SharedTemplate {
            get {
                return ResourceManager.GetString("SharedTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dear @Model.Name,
        ///
        ///An account has been created for you.
        ///
        ///Your account is FREE and allows you to perform bla bla features.
        ///
        ///To login and complete your profile, please go to:
        ///
        ///@Model.LogOnUrl
        ///
        ///Your User ID is your email address and password is: @Model.Password.
        /// </summary>
        internal static string TextTemplate {
            get {
                return ResourceManager.GetString("TextTemplate", resourceCulture);
            }
        }
    }
}
