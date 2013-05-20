using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows.Threading;

namespace Chinchilla
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private Dictionary<string, string> selectedPackage = new Dictionary<string, string>();
        Dictionary<string, string> pkginfo = new Dictionary<string, string>();
        List<Basechart> testchart = new List<Basechart>();
        //private DispatcherTimer timerSine;

        public Window2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateListview();
            updateStausBar();
        }

        void updateListview()
        {
            pkginfo = DeviceInfoHelper.GetPackageInfo();

            foreach (KeyValuePair<string, string> pkg in pkginfo)
            {
                this.listView1.Items.Add(pkg.Key);
            }
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPackage.Clear();
            foreach(var item in this.listView1.SelectedItems)
            {
                selectedPackage[item.ToString()] = pkginfo[item.ToString()];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPackage.Count == 0)
            {
                MessageBox.Show("Please select process");
            }
            else
            {
                testchart.Clear();
                testchart.Add(new DatausageChart(Dispatcher, this.chart_datausage, selectedPackage));
                testchart.Add(new CpuChart(Dispatcher, this.chart_cpu, selectedPackage));
                testchart.Add(new MemChart(Dispatcher, this.chart_mem, selectedPackage));
            }
        }

        private void updateStausBar()
        {
            DispatcherTimer timerSine = new DispatcherTimer();
            timerSine.Tick += new EventHandler(updateStatus);
            timerSine.Interval = new TimeSpan(0, 0, 1);
            timerSine.Start();
        }

        private void updateStatus(object sender, EventArgs e)
        {
            string selectedProc = "";
            foreach(var pkginfo in this.selectedPackage)
            {
                if (selectedProc !="")
                    selectedProc=selectedProc+";";
                selectedProc = selectedProc+ pkginfo.Key;
            }
            this.status_proname.Content = selectedProc;
            if (this.testchart.Count > 0)
            {
                this.status_datausage.Content = this.testchart[0].CurrentData+"(KB)";
                this.status_cpu.Content = this.testchart[1].CurrentData+"%";
                this.status_mem.Content = this.testchart[2].CurrentData+"(KB)";
            }

        }
    }
}