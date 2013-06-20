using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Research.DynamicDataDisplay;

namespace Chinchilla
{
    /// <summary>Renders specified text near the point</summary>
    public class FrameValueTextMarker : PointMarker
    {
        private Viewport2D ct;

        public FrameValueTextMarker(Viewport2D ct)
        {
            this.ct = ct;
        }

        public override void Render(DrawingContext dc, Point screenPoint)
        {
            Point dataPoint = screenPoint.ScreenToData(this.ct.Transform);
            Point dataPointZero = new Point(dataPoint.X, 0.0);
            Point screenPointZero = dataPointZero.DataToScreen(this.ct.Transform);
            //const double verticalShift = 5; // px

            //dc.DrawLine(new Pen(Brushes.Black, 1), Point.Add(screenPoint, new Vector(0, 40)), screenPoint);

            double pointx = screenPointZero.X + 2;
            double pointy = screenPointZero.Y + 13;

            FormattedText textToDraw = new FormattedText(dataPoint.Y.ToString("0.0"), Thread.CurrentThread.CurrentCulture,
                   FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Black);
            dc.DrawText(textToDraw, new Point(pointx, pointy));

            /*
            string svalue = dataPoint.Y.ToString("0.000");
            foreach (var s in svalue)
            {
                if (s.Equals('.'))
                    continue;
                FormattedText textToDraw = new FormattedText(s.ToString(), Thread.CurrentThread.CurrentCulture,
                    FlowDirection.LeftToRight, new Typeface("Arial"), 12, Brushes.Black);
                dc.DrawText(textToDraw, new Point(pointx,pointy));
                pointy = pointy + 10;

            }
             * */
        }
    }
}
