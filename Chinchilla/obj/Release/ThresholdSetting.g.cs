﻿#pragma checksum "..\..\ThresholdSetting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F4321FA89992D58340439DF6D7613C2C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3643
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Chinchilla {
    
    
    /// <summary>
    /// ThresholdSetting
    /// </summary>
    public partial class ThresholdSetting : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.Label label2;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.Label label3;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.Label label4;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.TextBox textBox1;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.TextBox textBox2;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.TextBox textBox3;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.TextBox textBox4;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\ThresholdSetting.xaml"
        internal System.Windows.Controls.Button button2;
        
        #line default
        #line hidden
        
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
            System.Uri resourceLocater = new System.Uri("/Chinchilla;component/thresholdsetting.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ThresholdSetting.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.label2 = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.label3 = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.label4 = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.textBox1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.textBox2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.textBox3 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.textBox4 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.button1 = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\ThresholdSetting.xaml"
            this.button1.Click += new System.Windows.RoutedEventHandler(this.button1_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.button2 = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\ThresholdSetting.xaml"
            this.button2.Click += new System.Windows.RoutedEventHandler(this.button2_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
