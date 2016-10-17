<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Main.master" AutoEventWireup="true"
    CodeFile="Earning.aspx.cs" Inherits="Pages.PagesEarning" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadComboBoxOptions">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridExpenses" />
                    <telerik:AjaxUpdatedControl ControlID="RadGridEarning" />
                    <telerik:AjaxUpdatedControl ControlID="RadChartExpenses" />
                    <telerik:AjaxUpdatedControl ControlID="RadChartEarning" />
                    <telerik:AjaxUpdatedControl ControlID="rcbYears" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbYears">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChartEarning" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbReport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbYears" />
                    <telerik:AjaxUpdatedControl ControlID="RadChartEarning" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="EarningSourcePieChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="EarningClientPieChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
    <table>
        <tr>
            <td>
                <telerik:RadComboBox ID="rcbReport" runat="server" AutoPostBack="true" Width="100px"
                    Label="Earning report by:" Skin="Office2010Silver" OnSelectedIndexChanged="RcbReport_SelectedIndexChanged">
                    <Items>
                        <telerik:RadComboBoxItem Text="Year" Value="Year" Selected="true" />
                        <telerik:RadComboBoxItem Text="Source" Value="Source" />
                        <telerik:RadComboBoxItem Text="Clients" Value="Clients" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td>
                <telerik:RadComboBox ID="rcbYears" runat="server" AutoPostBack="true" Width="100px"
                    Skin="Office2010Silver" OnSelectedIndexChanged="RcbYears_SelectedIndexChanged">
                    <Items>
					    <telerik:RadComboBoxItem Text="2015" Value="2015" Selected="true"/>
                        <telerik:RadComboBoxItem Text="2014" Value="2014" />
                        <telerik:RadComboBoxItem Text="2013" Value="2013" />
                        <telerik:RadComboBoxItem Text="2012" Value="2012" />
                        <telerik:RadComboBoxItem Text="2011" Value="2011" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
    <br />
    <%--EARNING CHART PER MONTH--%>
    <telerik:RadChart ID="RadChartEarning" runat="server" Width="1202px" Height="450px" Skin="DeepGray"
        DataSourceID="sdsEarningChartData" IntelligentLabelsEnabled="True">
        <PlotArea>
            <XAxis Step="1" DataLabelsColumn="oDate" AutoScale="false" LayoutMode="Inside" />
            <YAxis Step="2000" AxisMode="Extended" />
        </PlotArea>
        <ChartTitle>
            <TextBlock Text="No data available!" />
        </ChartTitle>
        <Series>
            <telerik:ChartSeries Name="Salary per Month" Type="Line" DataXColumn="oDate" DataYColumn="amount">
                <Appearance LegendDisplayMode="Nothing">
                    <PointMark Visible="True" Border-Width="1" Border-Color="#ffc501" Dimensions-AutoSize="False"
                        Dimensions-Height="6px" Dimensions-Width="6px">
                        <FillStyle MainColor="225,254,88" FillType="Solid" />
                    </PointMark>
                </Appearance>
            </telerik:ChartSeries>
        </Series>
    </telerik:RadChart>
    <asp:SqlDataSource ID="sdsEarningChartData" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString%>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT DATEPART(mm, OnDate) AS oDate, SUM(Amount) AS amount FROM Earning WHERE DATEPART(yy, OnDate) = @YearValue GROUP BY DATEPART(mm, Earning.OnDate)">
        <SelectParameters>
            <asp:ControlParameter ControlID="rcbYears" DefaultValue="2015" Name="YearValue" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%--EARNING CHART PER SOURCE--%>
    <telerik:RadChart ID="EarningSourcePieChart" runat="server" DefaultType="Pie" Width="960px"
        AutoTextWrap="true" OnItemDataBound="EarningSourcePieChart_ItemDataBound" DataSourceID="sdsPieChartSourceData"
        Skin="LightBlue" Visible="false" OnPreRender="EarningSourcePieChart_PreRender">
        <ChartTitle>
            <TextBlock Text="Lifetime Earning - Source wise" />
        </ChartTitle>
        <Series>
            <telerik:ChartSeries Name="Items" Type="Pie" DataYColumn="Amount">
                <Appearance LegendDisplayMode="ItemLabels" />
            </telerik:ChartSeries>
        </Series>
    </telerik:RadChart>
    <asp:SqlDataSource ID="sdsPieChartSourceData" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString%>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT SUM(Amount) AS spentOn, E.Source FROM Earning AS E WHERE E.Source <> 'NA' GROUP BY E.Source" />
    
     <%--EARNING CHART PER CLIENT--%>
    <telerik:RadChart ID="EarningClientPieChart" runat="server" DefaultType="Pie" Width="960px"
        AutoTextWrap="true" OnItemDataBound="EarningClientPieChart_ItemDataBound" DataSourceID="sdsPieChartClientData"
        Skin="LightBlue" Visible="false" OnPreRender="EarningClientPieChart_PreRender">
        <ChartTitle>
            <TextBlock Text="Lifetime Earning - Client wise" />
        </ChartTitle>
        <Series>
            <telerik:ChartSeries Name="Items" Type="Pie" DataYColumn="Amount">
                <Appearance LegendDisplayMode="ItemLabels" />
            </telerik:ChartSeries>
        </Series>
    </telerik:RadChart>
    <asp:SqlDataSource ID="sdsPieChartClientData" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString%>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT SUM(Amount) AS spentOn, E.Client FROM Earning AS E WHERE E.Client <> ' ' GROUP BY E.Client" />
    

    <br />
    <%--EARNING DATA GRID--%>
    <telerik:RadGrid ID="RadGridEarning" runat="server" Width="100%" PageSize="10" AllowPaging="True"
        AutoGenerateColumns="False" DataSourceID="sdsEarningData" Skin="Office2010Blue"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridEarning_ItemDataBound">
        <MasterTableView DataSourceID="sdsEarningData" CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="Source" HeaderText="Earning Source" />
                <telerik:GridBoundColumn DataField="Client" HeaderText="From Client" />
                <telerik:GridBoundColumn DataField="Salary_USD" HeaderText="Salary (USD)" DataFormatString="${0:#.00}" />
                <telerik:GridBoundColumn DataField="Amount" HeaderText="Salary (INR)" DataFormatString="{0:C2}" />
                <telerik:GridBoundColumn DataField="ER" HeaderText="ER (INR/USD)" DataFormatString="{0:C2}" />
                <telerik:GridDateTimeColumn DataField="OnDate" HeaderText="On Date" DataFormatString="{0:dd MMMM yyyy}" />
                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="sdsEarningData" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString %>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT * FROM Earning ORDER BY OnDate DESC" />

    <%--RADNOTIFICATION--%>
    <!--AutoCloseDelay is in milli-seconds-->
    <telerik:RadNotification ID="goalNotification" runat="server" Width="320" Height="190"
        VisibleOnPageLoad="true" Animation="Fade" EnableRoundedCorners="true" VisibleTitlebar="true"
        Title="Financial Goal Tracker" Text="Target for Year:" OffsetX="-10" OffsetY="-10" AutoCloseDelay="60000" LoadContentOn="PageLoad" TitleIcon="../Style/Img/exclamation.png"
        Skin="Hay" Visible="True">
        <ContentTemplate>
            <div class="outerContainer">
                <div class="divLeft">
                    <img src="../Style/Img/BigFinance.png" alt="" />
                </div>
                <div class="divRight">
                    <strong>
                        <asp:Label ID="lblCurrYrGoal" runat="server" />
                    </strong>
                    <br />
                    <asp:Label ID="lblCurrYrTillLastMonthGoal" runat="server" Visible="false" />
                    <asp:Label ID="lblCurrYrEarned" runat="server" /><br />
                    <asp:Label ID="lblCurrYrEarnLeft" runat="server" /><br />

                    <br />
                    <strong>
                        <asp:Label ID="lblCurrMonth" runat="server" /></strong><br />
                    <asp:Label ID="lblCurrMonthEarned" runat="server" /><br />
                    <asp:Label ID="lblCurrMonthEarnLeft" runat="server"/>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadNotification>
</asp:Content>
