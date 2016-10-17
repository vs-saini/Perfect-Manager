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
            string page = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);

            switch (page)
            {
                case "Expenses":
                    RadComboBoxOptions.SelectedIndex = 0;
                    break;

                case "Earning":
                    RadComboBoxOptions.SelectedIndex = 1;
                    break;

                case "Potential":
                    RadComboBoxOptions.SelectedIndex = 2;
                    break;

                case "TimeTracker":
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
                case "Time Spent":
                    Response.Redirect("~/Pages/TimeTracker.aspx");
                    break;
                case "Potential Money":
                    Response.Redirect("~/Pages/Potential.aspx");
                    break;
            }
        }
    }
}
