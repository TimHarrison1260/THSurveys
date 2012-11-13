using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THSurveys.Models.Analysis
{
    /// <summary>
    /// Class <c>ChartInformation</c> contains the information
    /// to be displayed on the data part of the chart.
    /// </summary>
    public class ChartInformation
    {
        /// <summary>
        /// Gets or sets the text of the response
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the analysis value of the
        /// for the response
        /// </summary>
        public double responses { get; set; }

        public string ToolTip
        {
            get
            {
                return string.Format("{0}: {1}%", this.Text, this.responses);
            }
        }
    }

    /// <summary>
    /// Class<c>ChartModel</c> is the object used to plot the MVC Chart
    /// on the browser page.  It is rendered to the page as an image
    /// file, constructed from a memoryStream object.
    /// </summary>
    public class ChartData
    {

        /// <summary>
        /// Gets or sets the collection of results for the question
        /// responses
        /// </summary>
        public IList<ChartInformation> Results { get; set; }

        /// <summary>
        /// Gets or sets the text of the question for which the results
        /// are being analysed.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the width of the chart image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the chart image.
        /// </summary>
        public int Height { get; set; }

        //  Add any other properties that might need to be set for the 
        //  Chart objet

    }
}