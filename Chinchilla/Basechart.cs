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

namespace Chinchilla
{
    class Basechart
    {
        private ChartPlotter chart;

        private DispatcherTimer timerSine;
        private double t = 0;
        private Dispatcher disp;
        private String package;
        private String uid = "";
        protected Dictionary<string, string> pkglist;
        private Dictionary<string, ObservableDataSource<Point>> datalist;
        protected double currentData;
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
            disp = p;
            chart = newchart;
            chart.FitToView();
            datalist = new Dictionary<string, ObservableDataSource<Point>>();

            //Set timer
            timerSine = new DispatcherTimer();
            timerSine.Tick += new EventHandler(timerSine_Tick);
            timerSine.Interval = new TimeSpan(0, 0, 1);

            //Start draw chart
            pkglist = new Dictionary<string,string>(packagelist);
            foreach (KeyValuePair<string, string> pkg in pkglist)
            {
                datalist.Add(pkg.Key, new ObservableDataSource<Point>());
                chart.AddLineGraph(datalist[pkg.Key], Color.FromRgb(178, 58, 238), 2, pkg.Key);
            }
            timerSine.Start();
        }

        private void timerSine_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, ObservableDataSource<Point>> pkg in datalist)
            {
                pkg.Value.AppendAsync(disp, new Point(t, getData(pkg.Key)));
                t += timerSine.Interval.Seconds;
            }
        }

        public virtual double getData(String package)
        {
            Random rd = new Random();
            return (double)rd.Next(100);
        }
    }
}
