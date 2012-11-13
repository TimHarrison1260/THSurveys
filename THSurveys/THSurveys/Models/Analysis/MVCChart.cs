using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;                                    //  Memory stream stuff;
using System.Web.UI.DataVisualization.Charting;     //  Charting stuff;
using System.Drawing;                               //  Font stuff;

namespace THSurveys.Models.Analysis
{
    /// <summary>
    /// This abstract class represents the MVC Chart object.
    /// </summary>
    public abstract class MVCChart
    {
        private ChartData _chartData;
        //private List<Series> ChartSeriesData { get; set; }
        //private string ChartTitle { get; set; }

        public MVCChart(ChartData ChartData)
        {
            if (ChartData == null)
                throw new ArgumentNullException("ChartData", "No data supplied to build a chart with");
            _chartData = ChartData;
        }


        // This is the method to get the chart image
        public MemoryStream GenerateChartImage(int width, int height)
        {
            //  Generate the chart, to be rendered as a BinaryStream;
            var chart = CreateChart(width, height, RenderType.BinaryStreaming);
            //  Create a memory stream to store the image.
            var ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            //  Return the image of the chart.
            return ms;
        }

        /// <summary>
        /// Create and populate the chart with the supplied information
        /// </summary>
        /// <param name="width">The width of the chart</param>
        /// <param name="height">The height of the chart</param>
        /// <param name="renderType">The RenderType of chart which represents how the chart will be rendered to the UI.</param>
        /// <returns>The chart object.</returns>
        private Chart CreateChart(int width, int height, RenderType renderType)
        {
            //  1   create the chart object
            var chart = InitialiseChart(width, height);
            //  2   Add the Title object, if it's defined.
            if (this._chartData.Title != null)
                chart.Titles.Add(CreateTitle());
            //  3   Add the Legend Object
            chart.Legends.Add(CreateLegend());
            //  4   Add the data to the object
            chart.Series.Add(LoadChartData());
            //  5   Add Areas to the chart
            chart.ChartAreas.Add(CreateChartArea());
            //  6   Set the chosen renderType
            chart.RenderType = renderType;

            return chart;
        }

        /// <summary>
        /// Initialise the MVC Chart object
        /// </summary>
        /// <param name="width">The width of the chart.</param>
        /// <param name="height">The height of the chart.</param>
        /// <returns>The initialise MVC Chart</returns>
        private Chart InitialiseChart(int width, int height)
        {
            var chart = new Chart();
            chart.Width = width;
            chart.Height = height;
            chart.BorderSkin.BackColor = System.Drawing.Color.Transparent;
            chart.BorderSkin.PageColor = System.Drawing.Color.Transparent;
            chart.BackColor = System.Drawing.Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = System.Drawing.Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = System.Drawing.Color.FromArgb(26, 59, 105);
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;

            return chart;
        }

        /// <summary>
        /// Create a Title object for the MVC Chart object.
        /// </summary>
        /// <returns>An instance of an MVCChart Title object.</returns>
        private Title CreateTitle()
        {
            return new Title()
            {
                Text = _chartData.Title,
                ShadowColor = System.Drawing.Color.FromArgb(32, 0, 0, 0),
                Font = new System.Drawing.Font("Trebuchet MS", 10, FontStyle.Bold),
                ShadowOffset = 3,
                ForeColor = System.Drawing.Color.FromArgb(26, 59, 105)
            };
        }

        /// <summary>
        /// Create a Legend for the chart.
        /// </summary>
        /// <returns></returns>
        private Legend CreateLegend()
        {
            return new Legend()
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                Font = new System.Drawing.Font(new System.Drawing.FontFamily("Trebuchet MS"), 8),
                LegendStyle = LegendStyle.Row
            };
        }

        /// <summary>
        /// Adds the data to a MVC Chart Series object
        /// </summary>
        private Series LoadChartData()
        {
            var series = new Series()
            {
                Name = "Question Responses",
                ChartType = SeriesChartType.Bar,
                BorderWidth = 1
            };

            foreach (var data in _chartData.Results)
            {
                //  Create a MVC Chart Datapoint to hold the data
                var point = new DataPoint();
                //  
                point.IsValueShownAsLabel = true;
                point.AxisLabel = data.Text;
                point.ToolTip = data.Text + " " + data.responses.ToString("#0.##%");
                point.YValues = new double[] { data.responses };
                point.LabelFormat = "P1";
                series.Points.Add(point);
            }
            return series;
        }

        /// <summary>
        /// Create a ChartArea, which defines the x and y axes and the chart layout.
        /// </summary>
        /// <returns>An instance of the ChartArea object</returns>
        private ChartArea CreateChartArea()
        {
            var area = new ChartArea()
            {
                Name = _chartData.Title,
                BackColor = System.Drawing.Color.Transparent
            };

            area.AxisX.IsLabelAutoFit = true;
            area.AxisX.LabelStyle.Font =
                new System.Drawing.Font("Verdana,Arial,Helvetica,sans-serif",
                                        8F, FontStyle.Regular);
            area.AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            area.AxisX.Interval = 1;


            area.AxisY.LabelStyle.Font =
                new System.Drawing.Font("Verdana,Arial,Helvetica,sans-serif",
                                        8F, FontStyle.Regular);
            area.AxisY.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);

            return area;
        }
    }
}