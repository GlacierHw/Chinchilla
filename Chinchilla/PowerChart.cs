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
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows.Threading;
using System.Threading;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.Numeric;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

namespace Chinchilla
{
    class PowerChart : Basechart
    {
        public String charttype = "电量消耗";
        private Process proc = null;
        private ThreadStart ts;
        private bool runable = false;
        private bool isRunning = false;
        private Thread getPowerThread;
        private double avgData;

        public double AvgData
        {
            get
            {
                return this.avgData;
            }
        }

        private double maxData;

        public double MaxData
        {
            get
            {
                return this.maxData;
            }
        }

        public PowerChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist)
        {

        }

        public override void asyncProcData()
        {
            return;
        }

        public override void updatechart(Dictionary<string, string> packagelist)
        {
            isRunning = true;
            pkglist = new Dictionary<string, string>();
            clearLines();
            datalist.Clear();
            listgraph.Clear();

            this.avgData = 0;
            this.maxData = 0;

            datalist.Add(charttype, new ObservableDataSource<Point>());
            listgraph.Add(chart.AddLineGraph(datalist[charttype], Colors.Blue, 2, charttype));

            ts = new ThreadStart(getPower);
            if (getPowerThread == null)
            {
                if (!runable)
                {
                    return;
                }
                getPowerThread = new Thread(ts);
                getPowerThread.SetApartmentState(ApartmentState.STA);
                getPowerThread.Start();
            }
            else
            {
                getPowerThread.Abort();
                if (!proc.HasExited)
                    proc.Kill();
                getPowerThread = new Thread(ts);
                getPowerThread.SetApartmentState(ApartmentState.STA);
                getPowerThread.Start();
            }
        }

        public override void stop()
        {
            if (!isRunning)
                return;
            if (getPowerThread != null)
                getPowerThread.Abort();
            if (proc != null && !proc.HasExited)
                proc.Kill();
            stopPowerMoniter();
        }

        public override void restart()
        {
            runable = true;
            if (!isRunning)
            {
                return;
            }

            if (proc != null && !proc.HasExited)
                proc.Kill();

            this.avgData = 0;
            this.maxData = 0;
            startPowerMoniter();
            datalist[charttype].Collection.Clear();
            getPowerThread = new Thread(ts);
            getPowerThread.SetApartmentState(ApartmentState.STA);
            getPowerThread.Start();
        }

        private void getPower()
        {

            try
            {
                this.startPowerMoniter();

                string commandarg = "shell logcat -s ChinPower";
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("adb", commandarg);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;

                procStartInfo.CreateNoWindow = true;

                proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                
                string line;
                Regex powerReg = new Regex(@"Current:*(\d*.*\d*)mA:*\s*");

                int time = 0;

                /*
                DateTime dt = DateTime.Now
                while ((proc.StandardOutput.ReadLine()) != null && (DateTime.Now - dt).Milliseconds>2000)
                {
                }
                 * */

                while ((line = proc.StandardOutput.ReadLine()) != null)
                {
                    if (line.Contains("Current"))
                    {
                        Match powerM = powerReg.Match(line);
                        double power = Convert.ToDouble(powerM.Groups[1].ToString());

                        this.datalist[charttype].AppendAsync(disp, new Point(time, power));

                        this.currentData = power;


                        int count = this.datalist[charttype].Collection.Count;
                        this.avgData = (this.avgData * count + power) / (count + 1);
                        if (power >= this.maxData)
                        {
                            this.maxData = power;
                        }

                        time++;
                    }
                }

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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            stopPowerMoniter();
            if (getPowerThread != null)
                getPowerThread.Abort();

            base.dispose();
        }

        private void startPowerMoniter()
        {
            Executecmd.ExecuteCommandSync("adb shell logcat -c ", 0);
            Executecmd.ExecuteCommandSync("adb shell am startservice com.android.Chinpower/.service.UMLoggerService", 0);
        }

        private void stopPowerMoniter()
        {
            Executecmd.ExecuteCommandSync("adb shell am force-stop com.android.Chinpower", 0);
        }
    }
}