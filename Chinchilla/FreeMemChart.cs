using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using System.Text.RegularExpressions;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows;

namespace Chinchilla {
    class FreeMemChart : Basechart {
        public String charttype = "系统空闲";
        //protected bool running = false;
        private double avgData;
        public double AvgData {
            get {
                return this.avgData;
            }
        }

        private double minData;

        public double MinData {
            get {
                return this.minData;
            }
        }

        public override double getData(string package) {
            double memdata = 0;
            string meminfo;
            //Executecmd.ExecuteCommandSync("adb shell dumpsys meminfo " + package, out meminfo);
            Executecmd.ExecuteCommandSync("adb shell procrank", out meminfo);
            //return 100.0;
            Regex memReg = new Regex(@"\s*(\d*)K free,\s*(\d*)K buffers,\s*(\d*)K cached");
            Match memM = memReg.Match(meminfo);

            if (memM.Groups.Count > 1) {
                memdata = (Convert.ToDouble(memM.Groups[1].ToString()) + Convert.ToDouble(memM.Groups[2].ToString())
                    + Convert.ToDouble(memM.Groups[3].ToString())) / 1024;
            }
            this.currentData = memdata;
            if (this.datalist.Count != 1) {
                this.avgData = 0;
                this.minData = 0;
            } else {
                foreach (var value in this.datalist.Values) {
                    int count = value.Collection.Count;
                    this.avgData = (this.avgData * count + memdata) / (count + 1);
                }
                if (memdata <= this.minData || this.minData == 0) {
                    this.minData = memdata;
                }
            }

            return memdata;
        }

        public override void initChart() {
            
            base.clearLines();
            datalist.Clear();
            listgraph.Clear();
            testlock = false;
            t = 0;
            
        }

        public override void updatechart(Dictionary<string, string> packagelist) {
            
            //pkglist.Clear();
            aTimer.Stop();
            pkglist = new Dictionary<string, string>(packagelist);
            this.initChart();
            string lineName = "系统空闲内存";
            datalist.Add(lineName, new ObservableDataSource<Point>());
            listgraph.Add(chart.AddLineGraph(datalist[lineName], colorpool[linenum++], 2, lineName));//Color.FromRgb(72, 118, 255)
  
            aTimer.Start();
        }

        public FreeMemChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist) {
        }
    }
}
