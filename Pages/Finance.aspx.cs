using System;
using System.Configuration;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

namespace Pages
{
    public partial class PagesFinance : Page
    {
        private double _rate, _netWorth, _totalActiveMoney, _totalInactiveMoney;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            ShowNetWorth();
        }

        protected void RadGridEaring_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            double money;
            var item = e.Item as GridDataItem;
            if (item == null) return;

            // Set amount accordingly
            var amount = Convert.ToDouble(item["Amount"].Text);

            if (item["Currency"].Text.Equals("USD"))
                money = amount * _rate;
            else
                money = amount;

            // Mark active rows
            if (item["Active"].Text.Equals("Y"))
            {
                _totalActiveMoney = _totalActiveMoney + money;
                item.CssClass = "rgRow Orange";
            }
            else
                _totalInactiveMoney = _totalInactiveMoney + money;

            // Format amount as per currency
            item["Amount"].Text = Utility.AmountToString(money);

            // Get time spent
            var sDate = Convert.ToDateTime(item["SDate"].Text);
            if (sDate <= DateTime.Now)
            {
                var dateDifference = new DateDifference(sDate, DateTime.Now);
                item["TimeSpent"].Text = dateDifference.ToString();
            }

            // Set other values
            var builder = new StringBuilder();
            builder.AppendFormat("<span class='infoImg'>Total Potential : {0}</span>", Utility.AmountToString(_totalActiveMoney + _totalInactiveMoney));
            if (_totalInactiveMoney > 0)
            {
                builder.AppendFormat("<br/><span class='infoImg' style='color:yellow'>Active Money : {0}</span>", Utility.AmountToString(_totalActiveMoney));
                builder.AppendFormat("<br/><span class='infoImg' style='color:orange'>Inactive Money : {0}</span>", Utility.AmountToString(_totalInactiveMoney));
            }
            builder.AppendFormat("<br/><span class='infoImg'>Net Worth : {0}</span>", Utility.AmountToString(_netWorth));
            lblSnapshot.Text = builder.ToString();
        }

        protected void RadGridUpcomingExpenses_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridDataItem;
            if (item == null) return;

            // Format amount as per currency
            var amount = Convert.ToDouble(item["Amount"].Text);
            item["Amount"].Text = Utility.AmountToString(amount);

            // Get time left
            var lDate = Convert.ToDateTime(item["LastDate"].Text);
            if (lDate >= DateTime.Now)
            {
                var dateDifference = new DateDifference(DateTime.Now, lDate);
                item["TimeLeft"].Text = dateDifference.ToString();
            }
        }

        /// <summary>
        /// Show the networth considering all financial vehicles.
        /// </summary>
        private void ShowNetWorth()
        {
            // Read from web.config different values
            var sbbjBankBal = Convert.ToDouble(ConfigurationManager.AppSettings["SBBJ"]);
            var sbiBankBal = Convert.ToDouble(ConfigurationManager.AppSettings["SBI"]);
            var sbbjCurrentBal = Convert.ToDouble(ConfigurationManager.AppSettings["SBBJCurrent"]);
            var elanceBal = Convert.ToDouble(ConfigurationManager.AppSettings["Elance"]);
            var payPalBal = Convert.ToDouble(ConfigurationManager.AppSettings["PayPal"]);
            var transferWise = Convert.ToDouble(ConfigurationManager.AppSettings["TransferWise"]);
            var cashAmount = Convert.ToDouble(ConfigurationManager.AppSettings["Cash"]);

            // Investment stuff
            var showInvGainLoss = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowInvGainLoss"]);
            var ppf = Convert.ToDouble(ConfigurationManager.AppSettings["PPF"]);
            var pFd = Convert.ToDouble(ConfigurationManager.AppSettings["P-FD"]);
            var pFd2 = Convert.ToDouble(ConfigurationManager.AppSettings["P-FD2"]);
            var eFd = Convert.ToDouble(ConfigurationManager.AppSettings["E-FD"]);
            var eRd = Convert.ToDouble(ConfigurationManager.AppSettings["E-RD"]);
            var pRd = Convert.ToDouble(ConfigurationManager.AppSettings["P-RD"]);

            _rate = Convert.ToDouble(ConfigurationManager.AppSettings["ExchangeRate"]);

            // Get MF investment
            const string investQuery = "SELECT SUM(Amount) FROM MutualFunds"; // include MF
            var investAmount = Utility.TotalAmount(investQuery);

            if (showInvGainLoss)
            {
                var invGainLoss = Convert.ToDouble(ConfigurationManager.AppSettings["InvGainLoss"]);
                investAmount = investAmount + invGainLoss;
            }

            // Get gold investment
            const string goldQuery = "SELECT SUM(Amount) FROM Expenses WHERE Category='Gold'";
            var goldAmount = Utility.TotalAmount(goldQuery);

            // Get total earning from debtor in INR
            const string debtQuery = "SELECT SUM(Amount) - SUM(Deposited) FROM Debtors";
            var debtAmount = Utility.TotalAmount(debtQuery);

            var elanceBalInr = elanceBal * _rate;
            var payPalBalInr = payPalBal * _rate;

            _netWorth = sbbjBankBal + sbiBankBal + sbbjCurrentBal + elanceBalInr + payPalBalInr + transferWise + debtAmount + ppf + pFd + pFd2 + eFd + investAmount + goldAmount + eRd + pRd;

            // Net worth line
            var builder = new StringBuilder("<b>Net Worth = Sum of all financial instruments- </b>");

            // Bank balance
            builder.Append("<ol><li>Bank balance: ");
            builder.Append(Utility.AmountToString(sbbjBankBal + sbiBankBal)); // + sbbjCurrentBal
            builder.AppendFormat("<i> (<b>SBI:</b> {0},", Utility.AmountToString(sbiBankBal))
                .AppendFormat(" <b>SBBJ:</b> {0},", Utility.AmountToString(sbbjBankBal))
                .AppendFormat(" <b>CA:</b> {0})</i>", Utility.AmountToString(sbbjCurrentBal)).Append(" </li> ");

            // Show Elance balance
            if (elanceBalInr > 0)
            {
                builder.Append("<li>Elance balance: ");
                builder.Append(Utility.AmountToString(elanceBalInr));
                builder.Append(" ($").Append(elanceBal).Append(")");
                builder.Append(" </li> ");
            }

            // Show Cash amount
            if (cashAmount > 0)
            {
                builder.Append("<li>Cash amount: ");
                builder.Append(Utility.AmountToString(cashAmount));
                builder.Append(" </li> ");
            }

            // Show PayPal balance
            if (payPalBalInr > 0)
            {
                builder.Append("<li>PayPal balance: ");
                builder.Append(Utility.AmountToString(payPalBalInr));
                builder.Append(" ($").Append(payPalBal).Append(")");
                builder.Append(" </li> ");
            }

            // Show TransferWise balance
            if (transferWise > 0)
            {
                builder.Append("<li>TransferWise balance: ");
                builder.Append(Utility.AmountToString(transferWise));
                builder.Append(" </li> ");
            }

            // Show debtor amount
            if (debtAmount > 0)
            {
                builder.Append("<li>Debtor amount: ");
                builder.Append(Utility.AmountToString(debtAmount));
                builder.Append(" </li> ");
            }

            // Show PPF
            if (ppf > 0)
            {
                builder.Append("<li>PPF: ");
                builder.Append(Utility.AmountToString(ppf));
                builder.Append(" </li> ");
            }

            // Show plot FD
            if (pFd > 0)
            {
                builder.Append("<li>P-FD: ");
                builder.Append(Utility.AmountToString(pFd));
                builder.Append(" </li> ");
            }

            // Show plot FD
            if (pFd2 > 0)
            {
                builder.Append("<li>P-FD2: ");
                builder.Append(Utility.AmountToString(pFd2));
                builder.Append(" </li> ");
            }

            if (eFd > 0)
            {
                builder.Append("<li>E-FD: ");
                builder.Append(Utility.AmountToString(eFd));
                builder.Append(" </li> ");
            }

            // Show invest amount
            if (investAmount > 0)
            {
                builder.Append("<li>MF Investment: ");
                builder.Append(Utility.AmountToString(investAmount));
                builder.Append(" </li> ");
            }

            // Show gold amount
            if (goldAmount > 0)
            {
                builder.Append("<li>Gold Investment: ");
                builder.Append(Utility.AmountToString(goldAmount));
                builder.Append(" </li> ");
            }

            // Show emergency RD
            if (eRd > 0)
            {
                builder.Append("<li>E-RD: ");
                builder.Append(Utility.AmountToString(eRd));
                builder.Append(" </li> ");
            }

            if (pRd > 0)
            {
                builder.Append("<li>P-RD: ");
                builder.Append(Utility.AmountToString(pRd));
                builder.Append(" </li> ");
            }

            builder.Append("</ol>");

            var finalNetText = builder.ToString();
            lblNetWorthStatus.Text = finalNetText;

            // Liquid cash
            var liquidMoney = Utility.AmountToString(sbbjBankBal + sbiBankBal + sbbjCurrentBal + transferWise + cashAmount + pFd + pFd2 + eRd + pRd);
            lblLiquidMoney.Text = string.Format("<b>Total liquidable money:</b> {0}", liquidMoney);
        }


    }
}