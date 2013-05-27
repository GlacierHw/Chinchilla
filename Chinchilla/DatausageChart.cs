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
    class DatausageChart : Basechart
    {
        public String charttype = "流量";
        private Dictionary<string, double> initData = new Dictionary<string, double>();
        public override double getData(string package)
        {            
            String rcv;
            String snd;
            double datausage=0;
            Executecmd.ExecuteCommandSync("adb shell cat proc/uid_stat/" + pkglist[package] + "/tcp_snd", out snd);
            Executecmd.ExecuteCommandSync("adb shell cat proc/uid_stat/" + pkglist[package] + "/tcp_rcv", out rcv);
            if (rcv.IndexOf("such") > 0 || snd.IndexOf("such") > 0) {
                initData[package] = 0;
                return 0;
                
            }

            if (snd == String.Empty || rcv == String.Empty)
                return -1;
            datausage = Convert.ToDouble(rcv) + Convert.ToDouble(snd);
            if (this.t == 0) {
                initData[package] = datausage;
            }
            datausage = (datausage - initData[package]) / 1024;
            this.currentData = datausage;
            return datausage;
        }

        public DatausageChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist)
        {
           
        }
    }
}
