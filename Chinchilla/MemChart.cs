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
    class MemChart : Basechart
    {
        public String charttype = "内存";
        private double avgData;

        public double AvgData
        {
            get
            {
                return this.avgData;
            }
        }

        public override double getData(string package)
        {
            double memdata = 0;
            string meminfo;
            //Executecmd.ExecuteCommandSync("adb shell dumpsys meminfo " + package, out meminfo);
            Executecmd.ExecuteCommandSync("adb shell procrank", out meminfo);
            //return 100.0;
            Regex memReg = new Regex(@"\s*(\d*)K\s*\d*K\s*"+package+@"\s*");
            Match memM = memReg.Match(meminfo);
            if (memM.Groups.Count > 1)
            {
                memdata = Convert.ToDouble(memM.Groups[1].ToString())/1024;
            }

            this.currentData = memdata;

            if (this.datalist.Count != 1)
            {
                this.avgData = -1;
            }
            else
            {
                foreach (var value in this.datalist.Values)
                {
                    int count = value.Collection.Count;
                    this.avgData = (this.avgData * count + memdata) / (count + 1);
                }
            }

            return memdata;
        }

        public MemChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist)
        {
        }
    }
}
