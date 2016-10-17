using System;
using System.IO;
using Telerik.Web.UI;

namespace Template
{
    public partial class TemplateMain : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Use this event for tasks that require that all other controls on the page be loaded.
            var page = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);

            switch (page)
            {
                case "Expenses":
                    RadComboBoxOptions.SelectedIndex = 0;
                    break;
                case "Earning":
                    RadComboBoxOptions.SelectedIndex = 1;
                    break;
                case "Finance":
                    RadComboBoxOptions.SelectedIndex = 2;
                    break;
                case "MutualFund":
                    RadComboBoxOptions.SelectedIndex = 3;
                    break;
            }
        }

        protected void RadComboBoxOptions_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            switch (RadComboBoxOptions.SelectedItem.Text)
            {
                case "Expenses":
                    Response.Redirect("~/Pages/Expenses.aspx");
                    break;
                case "Earning":
                    Response.Redirect("~/Pages/Earning.aspx");
                    break;
                case "Finance":
                    Response.Redirect("~/Pages/Finance.aspx");
                    break;
                case "MutualFund":
                    Response.Redirect("~/Pages/MutualFund.aspx");
                    break;
            }
        }
    }
}
