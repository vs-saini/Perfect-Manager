<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Main.master" AutoEventWireup="true"
    CodeFile="Expenses.aspx.cs" Inherits="Pages.PagesExpenses" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
		 <telerik:AjaxSetting AjaxControlID="RadGridExpenses">
                <UpdatedControls>                    
                    <telerik:AjaxUpdatedControl ControlID="RadGridExpenses" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbReport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbYears" />
                    <telerik:AjaxUpdatedControl ControlID="rcbMonths" />
                    <telerik:AjaxUpdatedControl ControlID="lblExtraSpent"/>
                    <telerik:AjaxUpdatedControl ControlID="RadChartExpenses" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbYears">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblExtraSpent"/>
                    <telerik:AjaxUpdatedControl ControlID="RadChartExpenses" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbMonths">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChartExpenses" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Blue" />
   <table>
        <tr>
            <td>
                <telerik:RadComboBox ID="rcbReport" runat="server" AutoPostBack="true" Width="130px"
                    Label="Expenses report by:" Skin="Office2010Blue" OnSelectedIndexChanged="RcbReport_SelectedIndexChanged">
                    <Items>
                        <telerik:RadComboBoxItem Text="Year and Month" Value="YearAndMonth" Selected="true" />
                        <telerik:RadComboBoxItem Text="Only Year" Value="OnlyYear" />
                        <telerik:RadComboBoxItem Text="Combined Years" Value="CombinedYears" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td>
                <telerik:RadComboBox ID="rcbYears" runat="server" AutoPostBack="true" Width="100px"
                    Skin="Office2010Blue" OnSelectedIndexChanged="RcbYears_SelectedIndexChanged">
                    <Items>
						<telerik:RadComboBoxItem Text="2015" Value="2015" />
						<telerik:RadComboBoxItem Text="2014" Value="2014" />
                        <telerik:RadComboBoxItem Text="2013" Value="2013" />
                        <telerik:RadComboBoxItem Text="2012" Value="2012" />
                        <telerik:RadComboBoxItem Text="2011" Value="2011" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td>
                <telerik:RadComboBox ID="rcbMonths" runat="server" AutoPostBack="true" Width="100px"
                    Skin="Office2010Blue" OnSelectedIndexChanged="RcbMonths_SelectedIndexChanged">
                    <Items>
                        <telerik:RadComboBoxItem Text="January" Value="1" />
                        <telerik:RadComboBoxItem Text="February" Value="2" />
                        <telerik:RadComboBoxItem Text="March" Value="3" />
                        <telerik:RadComboBoxItem Text="April" Value="4" />
                        <telerik:RadComboBoxItem Text="May" Value="5" />
                        <telerik:RadComboBoxItem Text="June" Value="6" />
                        <telerik:RadComboBoxItem Text="July" Value="7" />
                        <telerik:RadComboBoxItem Text="August" Value="8" />
                        <telerik:RadComboBoxItem Text="September" Value="9" />
                        <telerik:RadComboBoxItem Text="October" Value="10" />
                        <telerik:RadComboBoxItem Text="November" Value="11" />
                        <telerik:RadComboBoxItem Text="December" Value="12" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
    <br />
    <%--RADCHART STARTS--%>
    <telerik:RadChart ID="RadChartExpenses" runat="server" DefaultType="Pie" RadiusFactor="1"
        Width="1200px" Height="400px" AutoTextWrap="true" OnItemDataBound="RadChartExpenses_ItemDataBound"
        DataSourceID="sdsExpenseChartData" Skin="LightBlue">
        <ChartTitle>
            <TextBlock Text="Expenses in year 2015" />
        </ChartTitle>
        <Series>
            <telerik:ChartSeries Name="Items" Type="Pie" DataYColumn="Amount" Appearance-Border-Color="#b6e0f9">
                <Appearance LegendDisplayMode="ItemLabels" />
            </telerik:ChartSeries>
        </Series>
    </telerik:RadChart>
    <asp:SqlDataSource ID="sdsExpenseChartData" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString%>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" />
    <%--RADCHART ENDS--%>
    <p>
        <i><asp:Label runat="server" ID="lblExtraSpent" /></i>
    </p>
    <br/>
     <%--RADGRID STARTS--%>
     <telerik:RadGrid ID="RadGridExpenses" runat="server" Width="100%" PageSize="10" AllowPaging="True"
        AutoGenerateColumns="False" DataSourceID="sdsExpenseData" Skin="Office2010Blue"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridExpenses_OnItemDataBound">
        <MasterTableView DataSourceID="sdsExpenseData" CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount" />
                <telerik:GridBoundColumn DataField="SpentOn" HeaderText="Spent On" />
                <telerik:GridDateTimeColumn DataField="OnDate" HeaderText="On Date" DataFormatString="{0:dd MMMM yyyy}" />
                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="sdsExpenseData" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString %>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT * FROM Expenses ORDER BY OnDate DESC" />
    <%--RADTOOLTIPMANAGER--%>
    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Skin="Telerik" Animation="Slide"
        Position="TopRight" AutoTooltipify="true" />
</asp:Content>
