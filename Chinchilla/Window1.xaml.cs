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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows.Threading;
namespace Chinchilla {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        private String pkgfile = "packageInfo.log";
        private String uid = "1001";
        private DispatcherTimer timerSine;
        private Dictionary<string, string> pkginfo;
        private ObservableDataSource<Point> dataSources;
        private ObservableDataSource<Point> dataSources1;
        private double t = 0;
        public Window1() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            /*dataSources = new ObservableDataSource<Point>();
            dataSources1 = new ObservableDataSource<Point>();
            chart0.AddLineGraph(dataSources, Color.FromRgb(178,58,238), 2,"流量消耗KB");
            chart0.AddLineGraph(dataSources1, Color.FromRgb(0, 58, 238), 2, "内存消耗MB");
            chart0.FitToView();*/
            
            pkginfo = new Dictionary<string, string>();
            executeCmd("adb shell dumpsys package > " + pkgfile,false);

            FileStream fi = new FileStream(pkgfile, FileMode.Open);
            StreamReader m_streamReader = new StreamReader(fi);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string strLine = m_streamReader.ReadLine();

            Regex reg = new Regex(@"Package \[(\S+)\]");//Package ['(\S+)']
            while (strLine != null) {
                //Console.WriteLine(strLine);
                Match m = reg.Match(strLine);
                if (m.Groups.Count > 1) {
                    string uid;       
                    string packagename = m.Groups[1].ToString();
                    Regex uidreg = new Regex(@".*userId=(\d*)");
                    m_streamReader.ReadLine();
                    m = uidreg.Match(m_streamReader.ReadLine());
                    if (m.Groups.Count > 1) {
                        uid = m.Groups[1].ToString();
                        pkginfo.Add(packagename, uid);
                    }
                }
                strLine = m_streamReader.ReadLine();
            }

            foreach (KeyValuePair<string, string> pkg in pkginfo) {
                listBox1.Items.Add(pkg.Key);
            }
            listBox1.SelectedIndex = 1;


            /*timerSine = new DispatcherTimer();
            timerSine.Tick += new EventHandler(timerSine_Tick);
            timerSine.Interval = new TimeSpan(0,0,1);*/
            Basechart bc = new Basechart(Dispatcher,chart0,pkginfo);
        
        }

        private void timerSine_Tick(object sender, EventArgs e)
        {   
            String datausage = executeCmd("adb shell cat proc/uid_stat/" + uid + "/tcp_rcv", true);
            if(datausage.IndexOf("such") > 0) {
                datausage = "0";
            }
            double data = Convert.ToDouble(datausage)/1024;
            dataSources.AppendAsync(Dispatcher, new Point(t,data));
            //dataSources1.AppendAsync(Dispatcher, new Point(t, data/10));
            t+= timerSine.Interval.Seconds;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            uid = pkginfo[listBox1.SelectedValue.ToString()];
        }

        public String executeCmd(string command, bool getresult) {
            try {
                //Console.WriteLine(command);
                ProcessStartInfo procStartInfo =
                        new ProcessStartInfo("cmd", "/c " + command);
                procStartInfo.RedirectStandardOutput = getresult;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                if (getresult) {
                    return proc.StandardOutput.ReadToEnd();
                } else {
                    return String.Empty;
                }
            } catch (Exception objException) {
                Console.WriteLine(objException.Message);
                return String.Empty;
            }
        }

        private int getUid(String packageName) {
            //executeCmd("adb shell dumpsys packageInfo")
            return 1;
        }

        private void button1_Click(object sender, RoutedEventArgs e) {
            /*String datausage = executeCmd("adb shell cat proc/uid_stat/" + uid.ToString() + "/tcp_rcv", true);
            textBox1.Text = datausage;*/
            timerSine.Start();
        }
    }
}
