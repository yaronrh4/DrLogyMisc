﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PDFMailer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.12.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("טסט")]
        public string Connection1Name {
            get {
                return ((string)(this["Connection1Name"]));
            }
            set {
                this["Connection1Name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=mssql2.websitelive.net;Initial Catalog=8156_drlogy_test;Persist Secur" +
            "ity Info=True;User ID=8156_drlogy_test;Password=MaccabiZona!123test")]
        public string Connection1Value {
            get {
                return ((string)(this["Connection1Value"]));
            }
            set {
                this["Connection1Value"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("פרודקשן")]
        public string Connection2Name {
            get {
                return ((string)(this["Connection2Name"]));
            }
            set {
                this["Connection2Name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=mssql2.websitelive.net;Initial Catalog=8156_drlogy_prod;Persist Secur" +
            "ity Info=True;User ID=8156_drlogy_prod;Password=!q1W@xse311!!!!")]
        public string Connection2Value {
            get {
                return ((string)(this["Connection2Value"]));
            }
            set {
                this["Connection2Value"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("פיתוח")]
        public string Connection3Name {
            get {
                return ((string)(this["Connection3Name"]));
            }
            set {
                this["Connection3Name"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=mssql2.websitelive.net;Initial Catalog=8156_drlogy_dev;Persist Securi" +
            "ty Info=True;User ID=8156_drlogy_dev;Password=MaccabiZona!123dev")]
        public string Connection3Value {
            get {
                return ((string)(this["Connection3Value"]));
            }
            set {
                this["Connection3Value"] = value;
            }
        }
    }
}
