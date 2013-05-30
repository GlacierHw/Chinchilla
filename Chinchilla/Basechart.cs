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
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;


namespace Chinchilla {
    class Basechart  {
        private String charttype = "";
        protected bool running = true;
        protected ChartPlotter chart;
        private DispatcherTimer timerSine;
        protected double t = 0;
        private bool testlock = false;
        protected Dispatcher disp;
        private String package;
        private String uid = "";
        protected Dictionary<string, string> pkglist = new Dictionary<string, string>();
        protected Dictionary<string, ObservableDataSource<Point>> datalist = new Dictionary<string, ObservableDataSource<Point>>();
        protected double currentData;
        protected List<LineGraph> listgraph = new List<LineGraph>();
        public delegate void DeleFunc();
        protected System.Timers.Timer aTimer;
        protected List<Color> colorpool = new List<Color>();
        private int linenum = 0;
        protected FollowWidthRestriction msr = new FollowWidthRestriction();
        protected Thread newThread;
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
            msr.Width = 300;
            this.chart.Viewport.Restrictions.Add(msr);
            chart.FitToView();
            chart.Legend.LegendLeft = 10.0;
            chart.MouseMove += new MouseEventHandler(chart_MouseMove);
            chart.MouseLeave += new MouseEventHandler(chart_MouseLeave);
            //chart.Legend.Visibility = auto;
            //pkglist = packagelist;
            //datalist = new Dictionary<string, ObservableDataSource<Point>>();
            ThreadStart ts = new ThreadStart(asyncProcData);
            newThread = new Thread(ts);
            newThread.Start();
        }

        public virtual void restart() {
            running = true;
        }

        public virtual void stop() {
            running = false;
        }

        public virtual void updatechart(Dictionary<string, string> packagelist) {
            //pkglist.Clear();
            aTimer.Stop();
            pkglist = new Dictionary<string,string>(packagelist);
            initChart();
            aTimer.Start();
        }
        public virtual void asyncProcData() {
         
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(timerSine_Tick);
            aTimer.Interval = 5000; 
  
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                     new DeleFunc(initChart));
            aTimer.Start();       
        }

        public void initChart() {
            clearLines();
            datalist.Clear();
            listgraph.Clear();
            testlock = false;
            t = 0;
            foreach (KeyValuePair<string, string> pkg in pkglist) {
                datalist.Add(pkg.Key, new ObservableDataSource<Point>());
                listgraph.Add(chart.AddLineGraph(datalist[pkg.Key], colorpool[linenum++], 2, pkg.Key));//Color.FromRgb(72, 118, 255)
            }
        }

        private void timerSine_Tick(object sender, EventArgs e) {
            if (!running) {
                return;
            }
            while(testlock) {
                Thread.Sleep(50);
            }
            testlock = true;
            foreach (KeyValuePair<string, ObservableDataSource<Point>> pkg in datalist) {
                Double value = getData(pkg.Key);
                if (value >= 0) {
                    string test = charttype;
                    pkg.Value.AppendAsync(disp, new Point(t, value));
                }
            }
            t += aTimer.Interval / 1000;
            testlock = false;
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
            chart.LegendVisibility = Visibility.Hidden;
        }

        private void chart_MouseMove(object sender, MouseEventArgs e) {
            chart.LegendVisibility = Visibility.Visible;
        }

        public virtual void dispose()
        {
            if (newThread != null)
                newThread.Abort();
        }
    }
}
