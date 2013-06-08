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
    class CpuChart : Basechart {
        public String charttype = "cpu";
        private double avgData;

        public double AvgData {
            get {
                return this.avgData;
            }
        }

        private double maxData;

        public double MaxData {
            get {
                return this.maxData;
            }
        }

        public override double getData(string package) {
            double cpudata = 0;
            string cpuinfo;
            Executecmd.ExecuteCommandSync("adb shell top -m 15 -d 1 -n 1", out cpuinfo);

            Regex cpuReg = new Regex(@"\s*(\d*)%.*" + package + @"\s*");
            Match cpuM = cpuReg.Match(cpuinfo);
            if (cpuM.Groups.Count > 1) {
                cpudata = Convert.ToDouble(cpuM.Groups[1].ToString());
            } else if (!cpuinfo.Contains("CPU")) {
                return -1;
            }

            this.currentData = cpudata;

            if (this.datalist.Count != 1) {
                this.avgData = 0.0;
                this.maxData = 0.0;
            } else {
                foreach (var value in this.datalist.Values) {
                    int count = value.Collection.Count;
                    this.avgData = (this.avgData * count + cpudata) / (count + 1);
                }

                if (cpudata >= this.maxData) {
                    this.maxData = cpudata;
                }
            }

            return cpudata;
        }

        public CpuChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist) {
        }
    }
}