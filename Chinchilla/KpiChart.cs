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
    class KpiChart : Basechart
    {
        public String charttype = "屏幕变化率";
        private Process proc = null;
        private Thread getDiffThread;
        private List<VerticalLine> vlines = new List<VerticalLine>();
        public delegate void DeleFunc(double value);
        ViewportRectPanel vp = new ViewportRectPanel();

        public KpiChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist)
        {

        }

        public override void asyncProcData()
        {
            return;
        }

        private void clearVerticalLine(ChartPlotter chart)
        {
            List<VerticalLine> vlines = new List<VerticalLine>();
            foreach (var child in chart.Children)
            {
                if (child is VerticalLine)
                {
                    vlines.Add(child as VerticalLine);
                }
            }

            foreach (var child in vlines)
            {
                chart.Children.Remove(child);
            }

        }

        public override void updatechart(Dictionary<string, string> packagelist)
        {
            pkglist = new Dictionary<string, string>();
            clearLines();
            //Rect rec = (t-20>0?t-20:0,t-20>0?t+10:30,t,chart.Viewport.Visible.YMax);
            datalist.Clear();
            listgraph.Clear();

            clearVerticalLine(chart);

            this.msr.Width = 30;
            datalist.Add("屏幕变化率", new ObservableDataSource<Point>());
            listgraph.Add(chart.AddLineGraph(datalist["屏幕变化率"], Colors.Blue, 2, "屏幕变化率"));//Color.FromRgb(72, 118, 255)

            ThreadStart ts = new ThreadStart(getScreenDiff);
            getDiffThread = new Thread(ts);
            getDiffThread.SetApartmentState(ApartmentState.STA);
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
                long linecount = 0;

                while ((line = proc.StandardOutput.ReadLine()) != null)
                {                    
                    if (line.Contains("time"))
                    {
                        if (linecount == 0)
                        {
                            linecount++;
                            continue;
                        }

                        Match diffM = cpuReg.Match(line);
                        double timex = Convert.ToDouble(diffM.Groups[1].ToString()) / 1000;
                        double diffy = Math.Abs(Convert.ToDouble(diffM.Groups[2].ToString()));
                        int pointcount = this.datalist["屏幕变化率"].Collection.Count;
                        if (pointcount > 10)
                        {
                            double lastvaluey = this.datalist["屏幕变化率"].Collection[pointcount - 1].Y;
                            double lastvaluex = this.datalist["屏幕变化率"].Collection[pointcount - 1].X;
                            if (lastvaluey > 0 && diffy == 0)
                            {
                                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                     new DeleFunc(addVerticalLine), timex);
                                //this.addVerticalLine(diffy);
                            }
                            else if (lastvaluey == 0 && diffy > 0)
                            {
                                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                     new DeleFunc(addVerticalLine), lastvaluex);
                                //this.addVerticalLine(lastvalue);
                            }
                        }
                        this.datalist["屏幕变化率"].AppendAsync(disp, new Point(timex, diffy > 66 ? 66:diffy));
                        /*
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                     (Action)(() =>
                                                     { 
                                                         //this.chart.Viewport.Visible = new Rect(timex-10,-10,11,100); 
                                                         //this.chart.Viewport.Visible.Width = 10; 
                                                     }));
                         */
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

        private void addVerticalLine(double value)
        {
            VerticalLine vl = new VerticalLine();
            vl.Value = value;
            vl.Stroke = new SolidColorBrush(Colors.IndianRed);
            vl.StrokeThickness = 2;
            vl.ToolTip = value.ToString();
            this.chart.Children.Add(vl);
        }
    }
}
