﻿#pragma checksum "C:\Users\OliverBrian\Documents\Visual Studio 2013\Projects\DoNation\Do-Nation\Do-Nation\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9863BACDD30F7FFB5F79960ABD283840"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Do_Nation {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.PasswordBox tf_password;
        
        internal System.Windows.Controls.Button btn_login;
        
        internal Microsoft.Phone.Controls.PhoneTextBox tf_email;
        
        internal System.Windows.Controls.TextBlock errorText;
        
        internal System.Windows.Controls.Button SignUpBtn;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Do-Nation;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.tf_password = ((System.Windows.Controls.PasswordBox)(this.FindName("tf_password")));
            this.btn_login = ((System.Windows.Controls.Button)(this.FindName("btn_login")));
            this.tf_email = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("tf_email")));
            this.errorText = ((System.Windows.Controls.TextBlock)(this.FindName("errorText")));
            this.SignUpBtn = ((System.Windows.Controls.Button)(this.FindName("SignUpBtn")));
        }
    }
}
