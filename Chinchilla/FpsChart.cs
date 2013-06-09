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
    class FpsChart : Basechart
    {
        public String charttype = "帧率";
        private Process proc = null;
        private ThreadStart ts;
        private bool runable = false;
        private bool isRunning = false;
        private Thread getDiffThread;
        private List<VerticalLine> vlines = new List<VerticalLine>();
        public delegate void DeleFunc(double value);
        public delegate void DeleRect(double value, double width);
        ObservableDataSource<Point> markerPoints = new ObservableDataSource<Point>();

        public FpsChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
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
            isRunning = true;
            pkglist = new Dictionary<string, string>();
            clearLines();
            datalist.Clear();
            listgraph.Clear();

            datalist.Add("帧率", new ObservableDataSource<Point>());

            listgraph.Add(chart.AddLineGraph(datalist["帧率"], Colors.Blue, 2, "帧率"));//Color.FromRgb(72, 118, 255)

            ts = new ThreadStart(getScreenFps);
            if (getDiffThread == null)
            {
                if (!runable)
                {
                    return;
                }
                getDiffThread = new Thread(ts);
                getDiffThread.SetApartmentState(ApartmentState.STA);
                getDiffThread.Start();
            }
            else
            {
                getDiffThread.Abort();
                if (!proc.HasExited)
                    proc.Kill();
                getDiffThread = new Thread(ts);
                getDiffThread.SetApartmentState(ApartmentState.STA);
                getDiffThread.Start();
            }
        }

        public override void stop()
        {
            if (!isRunning)
                return;
            if (getDiffThread != null)
                getDiffThread.Abort();
            if (proc != null && !proc.HasExited)
                proc.Kill();
            //getDiffThread = new Thread(ts);
            //getDiffThread.SetApartmentState(ApartmentState.STA);
            //getDiffThread.Start();
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
            datalist["帧率"].Collection.Clear();
            getDiffThread = new Thread(ts);
            getDiffThread.SetApartmentState(ApartmentState.STA);
            getDiffThread.Start();
        }

        private void getScreenFps()
        {

            try
            {
                //if (proc != null)
                //    proc.Kill();
                
                string commandarg = "shell /data/local/save_fps";
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
                Regex cpuReg = new Regex(@"time:\s*(\d*)\s*\S*frames:\s*-*(\d*)\s*");

                while ((line = proc.StandardOutput.ReadLine()) != null)
                {
                    if (line.Contains("time"))
                    {
                        Match diffM = cpuReg.Match(line);
                        double timex = Convert.ToDouble(diffM.Groups[1].ToString()) / 1000;
                        double fpsy = Math.Abs(Convert.ToDouble(diffM.Groups[2].ToString()));

                        this.datalist["帧率"].AppendAsync(disp, new Point(timex, fpsy));
                        this.currentData = fpsy;
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
                catch (Exception e)
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
            vl.Stroke = new SolidColorBrush(Colors.Green);
            vl.StrokeThickness = 1;
            vl.ToolTip = value.ToString();
            this.chart.Children.Add(vl);
        }

        private void addRect(double value, double width)
        {
            RectangleHighlight rh = new RectangleHighlight();
            rh.Bounds = new Rect(new Point(value - width, 0), new Point(value, 0));
            rh.Stroke = new SolidColorBrush(Colors.OrangeRed);
            rh.StrokeThickness = 2;
            this.chart.Children.Add(rh);
        }
    }
}
