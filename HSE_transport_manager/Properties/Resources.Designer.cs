﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HSE_transport_manager.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HSE_transport_manager.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Error saving file!.
        /// </summary>
        internal static string SettingsViewModel_Reset_Error_saving_file_message {
            get {
                return ResourceManager.GetString("SettingsViewModel_Reset_Error_saving_file_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot is inactive.
        /// </summary>
        internal static string StatusViewModel__botStatus_Bot_is_inactive_message {
            get {
                return ResourceManager.GetString("StatusViewModel__botStatus_Bot_is_inactive_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /get_route.
        /// </summary>
        internal static string StatusViewModel_BotWork__check_message {
            get {
                return ResourceManager.GetString("StatusViewModel_BotWork__check_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Неизвестная комманда..
        /// </summary>
        internal static string StatusViewModel_BotWork_Unknown_input_message {
            get {
                return ResourceManager.GetString("StatusViewModel_BotWork_Unknown_input_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot is active.
        /// </summary>
        internal static string StatusViewModel_Start_Bot_is_active_message {
            get {
                return ResourceManager.GetString("StatusViewModel_Start_Bot_is_active_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error contacting bot!.
        /// </summary>
        internal static string StatusViewModel_Start_Error_contacting_bot_message {
            get {
                return ResourceManager.GetString("StatusViewModel_Start_Error_contacting_bot_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bot is offline.
        /// </summary>
        internal static string StatusViewModel_Stop_Bot_is_offline_message {
            get {
                return ResourceManager.GetString("StatusViewModel_Stop_Bot_is_offline_message", resourceCulture);
            }
        }
    }
}
