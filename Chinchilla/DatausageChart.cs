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
        private String rcv = "0";
        private String snd = "0";
        public override double getData(string package)
        {            
            String tmp;
            double datausage=0;
            Executecmd.ExecuteCommandSync("adb shell cat proc/uid_stat/" + pkglist[package] + "/tcp_rcv", out tmp);
            if (tmp != String.Empty)
                rcv = tmp;
            Executecmd.ExecuteCommandSync("adb shell cat proc/uid_stat/" + pkglist[package] + "/tcp_rcv", out tmp);
            if (tmp != String.Empty)
                snd = tmp;
            if (rcv.IndexOf("such") > 0 || rcv == string.Empty)
            {
                rcv = "0";
            }
            if (snd.IndexOf("such") > 0 || snd == string.Empty)
            {
                snd = "0";
            }
            datausage = Convert.ToDouble(rcv) + Convert.ToDouble(snd);
            datausage = datausage / 1024;

            this.currentData = datausage;
            return datausage;
        }

        public DatausageChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist)
        {
        }
    }
}
