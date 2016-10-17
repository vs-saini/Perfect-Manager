using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using Telerik.Charting;
using Telerik.Web.UI;

namespace Pages
{
    public partial class PagesEarning : System.Web.UI.Page
    {
        // Global variable
        string _chartTitle;
        private double _cyLeftToEarn, _cmLeftToEarn;

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadChartEarning.PlotArea.XAxis.DataLabelsColumn = "OnDate";

            if (IsPostBack) return;
            RadChartEarning.ChartTitle.TextBlock.Text = _chartTitle;
        }

        protected void EarningSourcePieChart_ItemDataBound(object sender, ChartItemDataBoundEventArgs e)
        {
            e.SeriesItem.Name = ((DataRowView)e.DataItem)["Source"].ToString();
        }

        protected void EarningSourcePieChart_PreRender(object sender, EventArgs e)
        {
            Utility.SetSeriesColor(EarningSourcePieChart, "Source");
        }

        protected void EarningClientPieChart_PreRender(object sender, EventArgs e)
        {
            Utility.SetSeriesColor(EarningSourcePieChart, "Client");
        }

        protected void EarningClientPieChart_ItemDataBound(object sender, ChartItemDataBoundEventArgs e)
        {
            e.SeriesItem.Name = ((DataRowView)e.DataItem)["Client"].ToString();
        }

        protected void RcbYears_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadChartEarning.ChartTitle.TextBlock.Text = _chartTitle;
        }

        protected void RcbReport_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            switch (rcbReport.SelectedValue)
            {
                case "Year":
                    ShowOrHideControls(true, false, false);
                    RadChartEarning.ChartTitle.TextBlock.Text = _chartTitle;
                    break;

                case "Source":
                    ShowOrHideControls(false, true, false);

                    var inrAmount = Utility.AmountToString(Utility.TotalAmount("SELECT SUM(Amount) FROM Earning"));
                    var usdAmount = Utility.AmountToString(Utility.TotalAmount("SELECT SUM(Salary_USD) FROM Earning"), "en-US");

                    var totalWorkingYears = (DateTime.Now.Year - 2011) + 1;
                    var builder = new StringBuilder("Lifetime (").Append(totalWorkingYears).Append(" years) Earning :");
                    builder.Append(inrAmount).Append(" (").Append(usdAmount).Append(")");
                    EarningSourcePieChart.ChartTitle.TextBlock.Text = builder.ToString();
                    break;

                case "Clients":
                    ShowOrHideControls(false, false, true);
                    break;
            }
        }

        protected void RadGridEarning_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!(e.Item is GridDataItem)) return;

            //Get the instance of the right type
            var dataBoundItem = e.Item as GridDataItem;

            if (dataBoundItem["Salary_USD"].Text.Equals("&nbsp;"))
                dataBoundItem["Salary_USD"].Text = "NA";

            if (dataBoundItem["ER"].Text.Equals("&nbsp;"))
                dataBoundItem["ER"].Text = "NA";
        }

        #region Helpers

        /// <summary>
        /// Initialize different things for earning part.
        /// </summary>
        private void Initialize()
        {
            var builder = new StringBuilder("Total Earning in Year ");

            // Get total of earning in INR
            var inrQuery = "SELECT SUM(Amount) FROM Earning WHERE DATEPART(yy, OnDate) = " + rcbYears.SelectedValue;
            var totalAmount = Utility.TotalAmount(inrQuery);
            var inrAmount = Utility.AmountToString(totalAmount);
            builder.Append(rcbYears.SelectedValue).Append(" : ").Append(inrAmount);

            // Get total of earning in USD
            var usdQuery = "SELECT SUM(Salary_USD) FROM Earning WHERE DATEPART(yy, OnDate) = " + rcbYears.SelectedValue;
            var usdAmount = Utility.AmountToString(Utility.TotalAmount(usdQuery), "en-US");
            if (!usdAmount.Equals("$0.00"))
            {
                builder.Append(" (").Append(usdAmount).Append(")\n");
            }

            _chartTitle = builder.ToString();

            #region Set Financial Goal Tracker value

            // Keys: cy = current year, cm = current month
            var cmSalary = Convert.ToDouble(ConfigurationManager.AppSettings["TargetSalary"]);
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var lastMonth = month > 1 ? month - 1 : month;

            // Query to retrieve current month earning
            var cmeQueryBuilder = new StringBuilder("SELECT SUM(Amount) FROM Earning WHERE DATEPART(yy, OnDate) = ");
            cmeQueryBuilder.Append(year).Append(" AND DATEPART(mm,OnDate) = ").Append(month);

            // Note - Set year goals
            var cyGoal = cmSalary * 12;
            var cyLastMonthGoal = cmSalary * lastMonth;

            // If amount greater, then vice-versa
            if (cyGoal >= totalAmount)
                _cyLeftToEarn = cyGoal - totalAmount;
            else
                _cyLeftToEarn = totalAmount - cyGoal;

            lblCurrYrGoal.Text = string.Format("Goal for Year : {0} ({1})", year, Utility.AmountToString(cyGoal));

            if (cyLastMonthGoal > totalAmount)
            {
                lblCurrYrTillLastMonthGoal.Visible = true;
                lblCurrYrTillLastMonthGoal.Text = string.Format("Goal till last month : {0} {1}",
                    Utility.AmountToString(cyLastMonthGoal), "</br>");
            }

            lblCurrYrEarned.Text = string.Format("Earned : {0}", inrAmount);
            lblCurrYrEarnLeft.Text = string.Format(cyGoal >= totalAmount ? "Left to earn : {0}" : "Earned extra : {0}", Utility.AmountToString(_cyLeftToEarn));

            // Note - Set month goals
            var cmEarned = Utility.TotalAmount(cmeQueryBuilder.ToString());

            // If amount greater, then vice-versa
            if (cmSalary >= cmEarned)
                _cmLeftToEarn = cmSalary - cmEarned;
            else
                _cmLeftToEarn = cmEarned - cmSalary;

            lblCurrMonth.Text = string.Format("Goal for Month : {0} ({1})", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), Utility.AmountToString(cmSalary));
            lblCurrMonthEarned.Text = string.Format("Earned : {0}", Utility.AmountToString(cmEarned));
            lblCurrMonthEarnLeft.Text = string.Format(cmSalary >= cmEarned ? "Left to earn : {0}" : "Earned extra : {0}", Utility.AmountToString(_cmLeftToEarn));

            #endregion
        }

        /// <summary>
        /// Enable or disable visibility of controls.
        /// </summary>
        /// <param name="showEarningChart">Set true to show 'Earning Chart'</param>
        /// <param name="showSourceChart">Set true to show 'Earning Source Pie Chart'</param>
        /// <param name="showClientChart">Set true to show 'Earning Client Pie Chart'</param>
        private void ShowOrHideControls(bool showEarningChart, bool showSourceChart, bool showClientChart)
        {
            RadChartEarning.Visible = showEarningChart;
            EarningSourcePieChart.Visible = showSourceChart;
            EarningClientPieChart.Visible = showClientChart;

            rcbYears.Visible = showEarningChart;
        }

        #endregion
    }
}