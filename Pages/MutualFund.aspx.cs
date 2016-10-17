using System;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

namespace Pages
{
    public partial class PagesMutualFund : Page
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var birlaFooter = (GridFooterItem)RadGridBirla.MasterTableView.GetItems(GridItemType.Footer)[0];
            var birlaPrice = Convert.ToInt32(birlaFooter["Amount"].Text.Split(':')[1]);

            var iTaxFooter = (GridFooterItem)RadGridIciciTax.MasterTableView.GetItems(GridItemType.Footer)[0];
            var iTaxPrice = Convert.ToInt32(iTaxFooter["Amount"].Text.Split(':')[1]);

            var iGrowthFooter = (GridFooterItem)RadGridIciciGrowth.MasterTableView.GetItems(GridItemType.Footer)[0];
            var iGrowthPrice = Convert.ToInt32(iGrowthFooter["Amount"].Text.Split(':')[1]);

            var totalInvestment = birlaPrice + iTaxPrice + iGrowthPrice + 5000; // 5000 is Debit investment in ICICI

            var builder = new StringBuilder();
            builder.AppendFormat("<br/><span class='infoImg'>Total cumulative investment so far: {0} (including ICICI Debit investment)</span>", Utility.AmountToString(totalInvestment));
            lblTotalInvestment.Text = builder.ToString();
        }

        protected void RadGridBirla_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            //Get the instance of the right type
            var dataItem = e.Item as GridDataItem;

            if (dataItem != null)
            {
                var item = dataItem;

                // Set amount as per Indian currency
                var amount = Convert.ToDouble(item["Amount"].Text);
                item["Amount"].Text = Utility.AmountToString(amount);
            }
        }

        private void Initialize()
        {
            var dataTable = Utility.ExecuteQuery("SELECT * FROM MutualFunds ORDER BY Date DESC");

            var iciciGrowthDt = dataTable.Select("MFId=2");
            RadGridIciciGrowth.DataSource = iciciGrowthDt;
            RadGridIciciGrowth.DataBind();

            var iciciTaxDt = dataTable.Select("MFId=3");
            RadGridIciciTax.DataSource = iciciTaxDt;
            RadGridIciciTax.DataBind();

            var birlaDt = dataTable.Select("MFId=1");
            RadGridBirla.DataSource = birlaDt;
            RadGridBirla.DataBind();
        }
    }
}