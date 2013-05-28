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

namespace Chinchilla
{
    class KpiChart : Basechart
    {
        public String charttype = "屏幕变化率";
        private Process proc = null;
        private Thread getDiffThread;

        public KpiChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist)
        {
        }

        public override void asyncProcData()
        {
            return;
        }

        public override void updatechart(Dictionary<string, string> packagelist)
        {
            pkglist = new Dictionary<string, string>();
            clearLines();
            datalist.Clear();
            listgraph.Clear();
            datalist.Add("屏幕变化率", new ObservableDataSource<Point>());
            listgraph.Add(chart.AddLineGraph(datalist["屏幕变化率"], Colors.Blue, 2, "屏幕变化率"));//Color.FromRgb(72, 118, 255)

            ThreadStart ts = new ThreadStart(getScreenDiff);
            getDiffThread = new Thread(ts);
            getDiffThread.Start();
        }

        private void getScreenDiff()
        {
            try
            {
                if (proc != null)
                    proc.Kill();

                Executecmd.ExecuteCommandSync("adb root",0);
                Executecmd.ExecuteCommandSync("adb remount",0);
                Executecmd.ExecuteCommandSync("adb push save /data/local/save",0);
                Executecmd.ExecuteCommandSync("adb shell chmod 777 /data/local/save",0);

                string commandarg = "shell /data/local/save 50";
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("adb", commandarg);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;

                procStartInfo.CreateNoWindow = true;

                proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                //result = proc.StandardOutput.ReadToEnd();
                string line;
                Regex cpuReg = new Regex(@"time:\s*(\d*)\s*\S*diff:\s*-*(\d*)\s*");

                while ((line = proc.StandardOutput.ReadLine()) != null)
                {
                    if (line.Contains("time"))
                    {
                        Match diffM = cpuReg.Match(line);
                        double timex = Convert.ToDouble(diffM.Groups[1].ToString()) / 1000;
                        double diffy = Math.Abs(Convert.ToDouble(diffM.Groups[2].ToString()));
                        this.datalist["屏幕变化率"].AppendAsync(disp, new Point(timex, diffy));
                    }
                }

                //proc.Dispose();
                // Display the command output.
                //Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // we eat the exception
                Console.WriteLine("Get error when reading screen diff,please contact hewei03@baidu.com,error:" + objException.Message);
            }
        }

        public override void dispose()
        {
            //getDiffThread.Abort();
            if (proc != null)
            {
                try
                {
                    proc.Kill();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (getDiffThread != null)
                getDiffThread.Abort();

            base.dispose();
        }
    }
}
