using System;
using System.Configuration;
using System.Data;
using System.Text;
using Telerik.Charting;
using Telerik.Web.UI;

namespace Pages
{
    public partial class PagesExpenses : System.Web.UI.Page
    {
        private string _queryForYear, _queryForMonth;
        private bool _includeParentCategory, _includeInvestCategory, _showExtraExpenses;

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            // Read values
            _includeParentCategory = Convert.ToBoolean(ConfigurationManager.AppSettings["IncludeParentCategory"]);
            _includeInvestCategory = Convert.ToBoolean(ConfigurationManager.AppSettings["IncludeInvestCategory"]);
            _showExtraExpenses = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowExtraExpenses"]);

            if (!IsPostBack)
            {
                rcbMonths.SelectedValue = Convert.ToString(DateTime.Now.Month);
                rcbYears.SelectedValue = Convert.ToString(DateTime.Now.Year);
            }

            var qb = new StringBuilder("SELECT SUM(Amount) FROM Expenses WHERE DATEPART(yy, OnDate) = ");
            qb.Append(rcbYears.SelectedValue);

            if(!_includeParentCategory)
                qb.Append(" AND Category <> 'Parents'");

            if (!_includeInvestCategory)
            qb.Append(" AND Category <> 'Investment'");

            _queryForYear = qb.ToString();

            qb.Append(" AND DATEPART(mm, OnDate) = ").Append(rcbMonths.SelectedValue);
            _queryForMonth = qb.ToString();
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {		
            RadToolTipManager1.ToolTipZoneID = RadChartExpenses.ClientID;
            if (IsPostBack) return;
            ExpensesMonthWise();
            ShowExtraExpenses();
            Utility.SetSeriesColor(RadChartExpenses, "Category");
        }

        protected void RadChartExpenses_ItemDataBound(object sender, ChartItemDataBoundEventArgs e)
        {
            e.SeriesItem.Name = ((DataRowView)e.DataItem)["Category"].ToString();

            e.SeriesItem.ActiveRegion.Tooltip += "Amount spent for " + ((DataRowView)e.DataItem)["Category"] + " : Rs. " + e.SeriesItem.YValue;
        }

        protected void RcbReport_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ShowReport();
        }

        protected void RcbYears_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ShowReport();
            ShowExtraExpenses();
        }

        protected void RcbMonths_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ExpensesMonthWise();
        }

        private void ShowReport()
        {
            switch (rcbReport.SelectedValue)
            {
                case "YearAndMonth":
                    rcbMonths.Visible = true;
                    ExpensesMonthWise();
                    break;

                case "OnlyYear":
                    rcbYears.Visible = true;
                    rcbMonths.Visible = false;
                    ExpensesYearWise();
                    break;

                case "CombinedYears":
                    rcbYears.Visible = rcbMonths.Visible = false;
                    ExpensesCombinedYearWise();
                    break;
            }
        }

        private void ExpensesMonthWise()
        {
            var qb = new StringBuilder("SELECT SUM(Amount) AS spentOn, MAX(E.OnDate) as onDate, E.Category FROM Expenses AS E WHERE DATEPART(yy, E.OnDate) = ");
            qb.Append(rcbYears.SelectedValue);
            qb.Append(" AND DATEPART(mm, E.OnDate) = ").Append(rcbMonths.SelectedValue);

            if (!_includeParentCategory)
                qb.Append(" AND Category <> 'Parents'");

            if (!_includeInvestCategory)
                qb.Append(" AND Category <> 'Investment'");

            qb.Append(" GROUP BY E.Category");

            sdsExpenseChartData.SelectCommand = qb.ToString();

            
            RadChartExpenses.DataBind();

            var builder = new StringBuilder("Expenses in ");
            builder.Append(rcbMonths.SelectedItem.Text);
            builder.Append(" ");
            builder.Append(rcbYears.SelectedValue);
            builder.Append(": ");
            builder.Append(Utility.AmountToString(Utility.TotalAmount(_queryForMonth)));

            // Also show total expenses for current year
            builder.Append(" (").Append("Total: ").Append(Utility.AmountToString(Utility.TotalAmount(_queryForYear))).Append(")");

            RadChartExpenses.ChartTitle.TextBlock.Text = builder.ToString();
            Utility.SetSeriesColor(RadChartExpenses, "Category");
        }

        private void ExpensesYearWise()
        {
            sdsExpenseChartData.SelectCommand =
                         "SELECT SUM(Amount) AS spentOn, MAX(E.OnDate) as onDate, E.Category FROM Expenses AS E WHERE DATEPART(yy, E.OnDate) = " + rcbYears.SelectedValue + " GROUP BY E.Category";
            RadChartExpenses.DataBind();

            var builder = new StringBuilder("Expenses in ");
            builder.Append(rcbYears.SelectedValue);
            builder.Append(": ");
            builder.Append(Utility.AmountToString(Utility.TotalAmount(_queryForYear)));

            RadChartExpenses.ChartTitle.TextBlock.Text = builder.ToString();
            Utility.SetSeriesColor(RadChartExpenses, "Category");
        }

        private void ExpensesCombinedYearWise()
        {
            sdsExpenseChartData.SelectCommand =
                        "SELECT SUM(Amount) AS spentOn, MAX(E.OnDate) as onDate, E.Category FROM Expenses AS E GROUP BY E.Category";
            RadChartExpenses.DataBind();

            var builder = new StringBuilder("Combined Year Expenses (");
            builder.Append(Utility.AmountToString(Utility.TotalAmount("SELECT SUM(Amount) FROM Expenses"), null));
            builder.Append(")");

            RadChartExpenses.ChartTitle.TextBlock.Text = builder.ToString();
            Utility.SetSeriesColor(RadChartExpenses, "Category");
        }

        private void ShowExtraExpenses()
        {
            if (!_showExtraExpenses) return;

            var qForParents = "SELECT SUM(Amount) FROM Expenses WHERE Category='Parents' AND DATEPART(yy, OnDate) = " + rcbYears.SelectedValue;
            var qForGift = "SELECT SUM(Amount) FROM Expenses WHERE Category='Gift' AND DATEPART(yy, OnDate) = " + rcbYears.SelectedValue;
            var qForUtility = "SELECT SUM(Amount) FROM Expenses WHERE Category='Utility' AND DATEPART(yy, OnDate) = " + rcbYears.SelectedValue;

            var tParents = Utility.TotalAmount(qForParents);
            var tGift = Utility.TotalAmount(qForGift);
            var tUtility = Utility.TotalAmount(qForUtility);
            
            var totalExtra = tParents + tGift + tUtility;

            var builder = new StringBuilder("Total extra spent = ").Append(Utility.AmountToString(totalExtra));
            builder.Append(" (To Parents: ").Append(Utility.AmountToString(tParents));
            builder.Append(" + As Gift: ").Append(Utility.AmountToString(tGift));
            builder.Append(" + For E.Bill: ").Append(Utility.AmountToString(tUtility));
            builder.Append(")");

            lblExtraSpent.Text = builder.ToString();
        }

        protected void RadGridExpenses_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item == null) return;

            // Set amount as per Indian currency
            var amount = Convert.ToDouble(item["Amount"].Text);
            item["Amount"].Text = Utility.AmountToString(amount);
        }
    }
}