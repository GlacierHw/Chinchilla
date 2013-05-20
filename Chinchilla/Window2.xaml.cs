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
        public List<string> threValue = new List<string>();
        //private DispatcherTimer timerSine;

        private ThresholdSetting thresholdSettingWindow;

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
                foreach (Basechart chart in testchart) {
                    chart.clearLines();
                }
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
            timerSine.Interval = new TimeSpan(0, 0, 3);
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
                this.status_datausage.Content = "流量:"+this.testchart[0].CurrentData.ToString("f2")+"KB";
                this.status_cpu.Content = "CPU:" + this.testchart[1].CurrentData+"%";
                this.status_mem.Content = "内存:" + this.testchart[2].CurrentData.ToString("f2") + "MB";
            }
            if (this.threValue.Count > 0)
            {
                this.status_datausage_thre.Content = "阈值:" + this.threValue[0] + "KB";
                this.status_cpu_thre.Content = "阈值:" + this.threValue[1] + "%";
                this.status_mem_thre.Content = "阈值:" + this.threValue[2] + "MB";

                if (this.testchart.Count > 0)
                {
                    if (this.testchart[0].CurrentData > Convert.ToDouble(this.threValue[0]))
                    {
                        this.status_datausage.Background = new SolidColorBrush(Colors.Red);
                    }

                    if (this.testchart[1].CurrentData > Convert.ToDouble(this.threValue[1]))
                    {
                        this.status_cpu.Background = new SolidColorBrush(Colors.Red);
                    }

                    if (this.testchart[2].CurrentData > Convert.ToDouble(this.threValue[2]))
                    {
                        this.status_mem.Background = new SolidColorBrush(Colors.Red);
                    }
                }
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.thresholdSettingWindow == null)
            {
                this.thresholdSettingWindow = new ThresholdSetting();
                this.thresholdSettingWindow.Owner = this;
                this.thresholdSettingWindow.Show();
            }
            else
            {
                this.thresholdSettingWindow.Show();
            }
        }
    }
}