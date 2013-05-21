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
using System.Threading;
using System.Timers;


namespace Chinchilla {
    class Basechart {

        private ChartPlotter chart;
        private DispatcherTimer timerSine;
        private double t = 0;
        private Dispatcher disp;
        private String package;
        private String uid = "";
        protected Dictionary<string, string> pkglist = new Dictionary<string, string>();
        private Dictionary<string, ObservableDataSource<Point>> datalist;
        protected double currentData;
        protected List<LineGraph> listgraph = new List<LineGraph>();
        public delegate void DeleFunc();
        private System.Timers.Timer aTimer;
        protected List<Color> colorpool = new List<Color>();
        private int linenum = 0;
        public double CurrentData
        {
            get
            {
                return this.currentData;
            }
        }

        public Basechart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
        {
            //Init variables
            
            colorpool.Add(Colors.Blue);
            colorpool.Add(Colors.Red);
            colorpool.Add(Colors.Green);
            colorpool.Add(Colors.Yellow);
            colorpool.Add(Colors.Pink);
            disp = p;
            chart = newchart;
            chart.FitToView();
            chart.Legend.LegendLeft = 10.0;
            chart.MouseMove += new MouseEventHandler(chart_MouseMove);
            chart.MouseLeave += new MouseEventHandler(chart_MouseLeave);
            //chart.Legend.Visibility = auto;
            pkglist = packagelist;
            datalist = new Dictionary<string, ObservableDataSource<Point>>();
            ThreadStart ts = new ThreadStart(asyncProcData);
            Thread newThread = new Thread(ts);
            newThread.Start();

        }

    
        public void asyncProcData() {
             //Set timer
            //TimerCallback timerDelegate = new TimerCallback(timerSine_Tick);
            //Timer timer = new Timer(timerDelegate, null, 0, 3000);
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(timerSine_Tick);
            aTimer.Interval = 2000; 
            /*timerSine = new DispatcherTimer();
            timerSine.Tick += new EventHandler(timerSine_Tick);
            timerSine.Interval = new TimeSpan(0, 0, 3);
            timerSine.Start();*/
            //Start draw chart
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                     new DeleFunc(initChart));
            aTimer.Start();
            
        }

        public void initChart() {
            foreach (KeyValuePair<string, string> pkg in pkglist) {
                datalist.Add(pkg.Key, new ObservableDataSource<Point>());
                listgraph.Add(chart.AddLineGraph(datalist[pkg.Key], colorpool[linenum++], 2, pkg.Key));//Color.FromRgb(72, 118, 255)
            }
        }

        public void timerSine_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, ObservableDataSource<Point>> pkg in datalist) {
                Double value = getData(pkg.Key);
                if (value >= 0) {
                    pkg.Value.AppendAsync(disp, new Point(t, value));
                }
                t += aTimer.Interval/1000;
            }
        }

        public virtual double getData(String package)
        {
            Random rd = new Random();
            return (double)rd.Next(100);
        }

        public void clearLines() {
            foreach(LineGraph lg in listgraph){
                chart.Children.Remove(lg);
            }
            linenum = 0;
        }

        private void chart_MouseLeave(object sender, MouseEventArgs e) {
            chart.LegendVisible = false;
        }

        private void chart_MouseMove(object sender, MouseEventArgs e) {
            chart.LegendVisible = true;
        }
    }
}
