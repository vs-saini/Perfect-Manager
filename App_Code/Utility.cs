using System;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Globalization;
using Telerik.Web.UI;
using Telerik.Charting.Styles;

public class Utility
{
    static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["sdfConString"].ConnectionString;

    public static double ExecuteDoubleScalar(string query)
    {
        double result = 0;
        var con = new SqlCeConnection(ConnectionString);

        using (var cmd = new SqlCeCommand(query, con))
        {
            con.Open();
            var queryResult = cmd.ExecuteScalar();

            if (queryResult != DBNull.Value)
                result = Convert.ToDouble(queryResult);
            con.Close();
        }
        return result;
    }

    /// <summary>
    /// Execute query and return data table.
    /// </summary>
    public static DataTable ExecuteQuery(string query)
    {
        var dataTable = new DataTable();

        using (var con = new SqlCeConnection(ConnectionString))
        {
            using (var cmd = new SqlCeCommand(query, con))
            {
                con.Open();
                var adapter = new SqlCeDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
        }

        return dataTable;
    }

    /// <summary>
    /// Get total amount as per query.
    /// </summary>
    public static double TotalAmount(string query)
    {
        return ExecuteDoubleScalar(query);
    }

    /// <summary>
    /// Convert amount value into currency format. By default INR.
    /// </summary>
    public static string AmountToString(double amount, string culture = "hi-IN")
    {
        return amount.ToString("C2", CultureInfo.CreateSpecificCulture(culture));
    }

    public static void SetSeriesColor(RadChart radChart, string categoryName)
    {
        var areaColors = new[]{Color.FromArgb(225,254,88),
       Color.FromArgb(61,157,86),
       Color.FromArgb(30,152,229),
       Color.FromArgb(0,254,156),
       Color.FromArgb(255,197,1),
       Color.FromArgb(227,136,57),
       Color.FromArgb(178,216,252),
       Color.FromArgb(62,77,92),
       Color.FromArgb(161,191,190),
       Color.FromArgb(104,109,103),
       Color.FromArgb(218,222,232),
	   Color.FromArgb(100,100,100),
	   Color.FromArgb(80,70,60)
   };
        var i = 0;
        radChart.Series[0].Name = categoryName;
        foreach (var item in radChart.Series[0].Items)
        {
            item.Appearance.FillStyle.MainColor = areaColors[i++];
            item.Appearance.FillStyle.FillType = FillType.Solid;
        }
    }

}

