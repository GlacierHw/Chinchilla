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

namespace Chinchilla {
    class KpiChart : Basechart {
        public String charttype = "屏幕变化率";
        private Process proc = null;
        private ThreadStart ts;
        private bool runable = false;
        private bool isRunning = false;
        private Thread getDiffThread;
        private List<VerticalLine> vlines = new List<VerticalLine>();
        public delegate void DeleFunc(double value);
        public delegate void DeleRect(double value, double width);
        ObservableDataSource<Point> markerPoints = new ObservableDataSource<Point>();
        ObservableDataSource<Point> framePoints = new ObservableDataSource<Point>();

        public KpiChart(Dispatcher p, ChartPlotter newchart, Dictionary<string, string> packagelist)
            : base(p, newchart, packagelist) {

        }

        public override void asyncProcData() {
            return;
        }

        private void clearVerticalLine() {
            List<VerticalLine> vlines = new List<VerticalLine>();
            foreach (var child in this.chart.Children) {
                if (child is VerticalLine) {
                    vlines.Add(child as VerticalLine);
                }
            }

            foreach (var child in vlines) {
                this.chart.Children.Remove(child);
            }

            
            List<RectangleHighlight> rectangleHighlight = new List<RectangleHighlight>();
            foreach (var child in this.chart.Children) {
                if (child is RectangleHighlight)
                {
                    rectangleHighlight.Add(child as RectangleHighlight);
                }
            }

            foreach (var child in rectangleHighlight)
            {
                this.chart.Children.Remove(child);
            }

        }

        public override void updatechart(Dictionary<string, string> packagelist) {
            isRunning = true;
            pkglist = new Dictionary<string, string>();
            clearLines();
            //Rect rec = (t-20>0?t-20:0,t-20>0?t+10:30,t,chart.Viewport.Visible.YMax);
            datalist.Clear();
            listgraph.Clear();

            this.msr.Width = 30;
            datalist.Add("屏幕变化率", new ObservableDataSource<Point>());

            MarkerPointsGraph mpg = new MarkerPointsGraph(markerPoints);
            XValueTextMarker ctm = new XValueTextMarker(this.chart.Viewport);
            mpg.Marker = ctm;
            this.chart.Children.Add(mpg);

            MarkerPointsGraph fmpg = new MarkerPointsGraph(framePoints);
            FrameValueTextMarker ftm = new FrameValueTextMarker(this.chart.Viewport);
            fmpg.Marker = ftm;
            this.chart.Children.Add(fmpg);

            listgraph.Add(chart.AddLineGraph(datalist["屏幕变化率"], Colors.Blue, 2, "屏幕变化率"));//Color.FromRgb(72, 118, 255)

            ts = new ThreadStart(getScreenDiff);
            if (getDiffThread == null) {
                if (!runable) {
                    return;
                }
                getDiffThread = new Thread(ts);
                getDiffThread.SetApartmentState(ApartmentState.STA);
                getDiffThread.Start();
            } else {
                getDiffThread.Abort();
                if (!proc.HasExited)
                    proc.Kill();
                getDiffThread = new Thread(ts);
                getDiffThread.SetApartmentState(ApartmentState.STA);
                getDiffThread.Start();
            }
        }

        public override void stop() {
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

        public override void restart() {
            runable = true;
            if (!isRunning) {
                return;
            }

            if (proc != null && !proc.HasExited)
                proc.Kill();
            clearKpiInfo();
            getDiffThread = new Thread(ts);
            getDiffThread.SetApartmentState(ApartmentState.STA);
            getDiffThread.Start();
        }

        private void getScreenDiff() {

            try {
                //if (proc != null)
                //    proc.Kill();
                string commandarg = "shell /data/local/tmp/save 15";
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

                int startIndex = 0;
                int endIndex = 0;
                int safeDistance = 100;
                int safeDistanceCount = 0;
                int pointShowStep = 4;
                int pointCurrentStep = 0;

                while ((line = proc.StandardOutput.ReadLine()) != null) {

                    if (line.Contains("time")) {
                        if (linecount == 0) {
                            linecount++;
                            continue;
                        }

                        Match diffM = cpuReg.Match(line);
                        double timex = Convert.ToDouble(diffM.Groups[1].ToString()) / 1000;
                        int diffi = Math.Abs(Convert.ToInt32(diffM.Groups[2].ToString()) << 25);
                        double diffy = diffi==0?diffi:66;
                        int pointcount = this.datalist["屏幕变化率"].Collection.Count;

                        if (pointcount > 10) {
                            double lastvaluey = this.datalist["屏幕变化率"].Collection[pointcount - 1].Y;
                            double lastvaluex = this.datalist["屏幕变化率"].Collection[pointcount - 1].X;
                            if (lastvaluey > 0 && diffy == 0) {
                                if (endIndex == 0 || (pointcount - 1 - endIndex) < safeDistance) {
                                    endIndex = pointcount;
                                }
                                safeDistanceCount = 0;
                            } else if (lastvaluey == 0 && diffy > 0) {
                                if (startIndex == 0) {
                                    startIndex = pointcount - 1;
                                } else if (endIndex != 0 && (pointcount - 1 - endIndex) < safeDistance) {
                                    endIndex = 0;
                                }
                                safeDistanceCount = 0;
                            } else {
                                if (startIndex != 0 && endIndex != 0) {
                                    safeDistanceCount++;
                                }

                                if (safeDistanceCount >= safeDistance) {
                                    double endx = this.datalist["屏幕变化率"].Collection[endIndex].X;
                                    double startx = this.datalist["屏幕变化率"].Collection[startIndex].X;
                                    Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                         new DeleFunc(addVerticalLine), endx);
                                    Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                         new DeleFunc(addVerticalLine), startx);
                                    this.markerPoints.AppendAsync(disp, new Point(this.datalist["屏幕变化率"].Collection[startIndex].X, endx - startx));
                                    Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                         new DeleRect(addRect), endx, endx - startx);
                                    
                                    int frame = 0;
                                    for (int i = startIndex; i <= endIndex; i++)
                                    {
                                        if (this.datalist["屏幕变化率"].Collection[i].Y > 0)
                                        {
                                            frame++;
                                        }
                                    }
                                    this.framePoints.AppendAsync(disp, new Point(this.datalist["屏幕变化率"].Collection[startIndex].X, frame / (endx - startx)));

                                    startIndex = 0;
                                    endIndex = 0;
                                    safeDistanceCount = 0;
                                    //this.datalist["屏幕变化率"].AppendAsync(disp, new Point(startx, 0));
                                    //this.datalist["屏幕变化率"].AppendAsync(disp, new Point(endx, 0));
                                }
                            }
                        }

                        pointCurrentStep++;
                        if (pointShowStep < pointCurrentStep)
                        {
                            this.datalist["屏幕变化率"].AppendAsync(disp, new Point(timex, diffy));
                            pointCurrentStep = 0;
                        }
                        else
                        {
                            if (pointcount >= 1)
                            {
                                double lastvaluey = this.datalist["屏幕变化率"].Collection[pointcount - 1].Y;
                                if (diffy != lastvaluey)
                                {
                                    this.datalist["屏幕变化率"].AppendAsync(disp, new Point(timex, diffy));
                                }
                            }
                        }
                    }
                }

                //proc.Dispose();
                // Display the command output.
                //Console.WriteLine(result);
            } catch (Exception objException) {
                // we eat the exception
                Console.WriteLine("Get error when reading screen diff,please contact hewei03@baidu.com,error:" + objException.Message);
            }
        }

        public override void dispose() {
            //getDiffThread.Abort();
            if (proc != null) {
                try {
                    proc.Kill();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
            if (getDiffThread != null)
                getDiffThread.Abort();

            base.dispose();
        }

        private void addVerticalLine(double value) {
            VerticalLine vl = new VerticalLine();
            vl.Value = value;
            vl.Stroke = new SolidColorBrush(Colors.Green);
            vl.StrokeThickness = 1;
            vl.ToolTip = value.ToString();
            this.chart.Children.Add(vl);
        }

        private void addRect(double value, double width) {
            RectangleHighlight rh = new RectangleHighlight();
            rh.Bounds = new Rect(new Point(value - width, 0), new Point(value, 0));
            rh.Stroke = new SolidColorBrush(Colors.OrangeRed);
            rh.StrokeThickness = 2;
            this.chart.Children.Add(rh);
        }

        private void clearKpiInfo()
        {
            this.framePoints.Collection.Clear();
            this.markerPoints.Collection.Clear();
            this.datalist["屏幕变化率"].Collection.Clear();
            clearVerticalLine();
        }
    }
}