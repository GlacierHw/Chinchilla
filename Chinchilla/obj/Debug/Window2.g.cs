﻿#pragma checksum "..\..\Window2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B7C2A31E17E61BA600E3AE5A919D3D25"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3643
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using Microsoft.Research.DynamicDataDisplay.Charts.Markers;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.Charts.Shapes;
using Microsoft.Research.DynamicDataDisplay.Common.Palettes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Navigation;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Microsoft.Research.DynamicDataDisplay.VisualDebugging;
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
    /// Window2
    /// </summary>
    public partial class Window2 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\Window2.xaml"
        internal System.Windows.Controls.DockPanel dockPanel1;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\Window2.xaml"
        internal System.Windows.Controls.Menu menu1;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.ToolBarPanel toolBarPanel1;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\Window2.xaml"
        internal System.Windows.Controls.ToolBar toolBar1;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBar statusBar1;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_name;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_proname;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_datausage;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_datausage_thre;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_cpu;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_cpu_thre;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_mem;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\Window2.xaml"
        internal System.Windows.Controls.Primitives.StatusBarItem status_mem_thre;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\Window2.xaml"
        internal System.Windows.Controls.TextBox textBox1;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\Window2.xaml"
        internal System.Windows.Controls.ListView listView1;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.ChartPlotter chart_datausage;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle chart_datausageTitle;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.ChartPlotter chart_cpu;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle chart_cpuTitle;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.ChartPlotter chart_mem;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle chart_memTitle;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.ChartPlotter chart_kpi;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\Window2.xaml"
        internal Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle chart_kpiTitle;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\Window2.xaml"
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\Window2.xaml"
        internal System.Windows.Controls.Grid grid1;
        
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
            System.Uri resourceLocater = new System.Uri("/Chinchilla;component/window2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Window2.xaml"
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
            
            #line 5 "..\..\Window2.xaml"
            ((Chinchilla.Window2)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 5 "..\..\Window2.xaml"
            ((Chinchilla.Window2)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dockPanel1 = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 3:
            this.menu1 = ((System.Windows.Controls.Menu)(target));
            return;
            case 4:
            
            #line 10 "..\..\Window2.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 11 "..\..\Window2.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.toolBarPanel1 = ((System.Windows.Controls.Primitives.ToolBarPanel)(target));
            return;
            case 7:
            this.toolBar1 = ((System.Windows.Controls.ToolBar)(target));
            return;
            case 8:
            
            #line 15 "..\..\Window2.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.statusBar1 = ((System.Windows.Controls.Primitives.StatusBar)(target));
            return;
            case 10:
            this.status_name = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 11:
            this.status_proname = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 12:
            this.status_datausage = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 13:
            this.status_datausage_thre = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 14:
            this.status_cpu = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 15:
            this.status_cpu_thre = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 16:
            this.status_mem = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 17:
            this.status_mem_thre = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 18:
            this.textBox1 = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\Window2.xaml"
            this.textBox1.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox1_TextChanged);
            
            #line default
            #line hidden
            return;
            case 19:
            this.listView1 = ((System.Windows.Controls.ListView)(target));
            
            #line 32 "..\..\Window2.xaml"
            this.listView1.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.listView1_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 20:
            this.chart_datausage = ((Microsoft.Research.DynamicDataDisplay.ChartPlotter)(target));
            return;
            case 21:
            this.chart_datausageTitle = ((Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle)(target));
            return;
            case 22:
            this.chart_cpu = ((Microsoft.Research.DynamicDataDisplay.ChartPlotter)(target));
            return;
            case 23:
            this.chart_cpuTitle = ((Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle)(target));
            return;
            case 24:
            this.chart_mem = ((Microsoft.Research.DynamicDataDisplay.ChartPlotter)(target));
            return;
            case 25:
            this.chart_memTitle = ((Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle)(target));
            return;
            case 26:
            this.chart_kpi = ((Microsoft.Research.DynamicDataDisplay.ChartPlotter)(target));
            return;
            case 27:
            this.chart_kpiTitle = ((Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle)(target));
            return;
            case 28:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 29:
            this.grid1 = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
