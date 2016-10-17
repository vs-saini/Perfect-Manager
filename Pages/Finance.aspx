<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Main.master" AutoEventWireup="true"
    CodeFile="Finance.aspx.cs" Inherits="Pages.PagesFinance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />
    <div align="center">
        <div class="infoPanel">
            <div class="lblPadding">
                <asp:Literal ID="lblSnapshot" runat="server"/>
            </div>
        </div>
    </div>
    <br />
    <%--RADGRID STARTS--%>
    <telerik:RadGrid ID="RadGridEarning" runat="server" Width="100%" PageSize="10" AllowPaging="True"
        AutoGenerateColumns="False" DataSourceID="sdsPotentialMoney" Skin="Office2007" ShowFooter="True"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridEaring_OnItemDataBound">
        <MasterTableView DataSourceID="sdsPotentialMoney" CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="From" HeaderText="From Source" />
                <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount (INR)" />
                <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount (USD)" Aggregate="Sum" DataFormatString="${0:#.00}"/>
				<telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" />
                <telerik:GridDateTimeColumn DataField="SDate" HeaderText="Start Date" DataFormatString="{0:dd MMMM yyyy}" />
                <telerik:GridDateTimeColumn DataField="EDate" HeaderText="By Date" DataFormatString="{0:dd MMMM yyyy}" />
                <telerik:GridBoundColumn DataField="TimeSpent" HeaderText="Time spent" />                
                <telerik:GridBoundColumn DataField="Currency" Visible="False" />
                <telerik:GridBoundColumn DataField="Active" Visible="False" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <p>
        *<i>Amount is calculated considering last exchange rate of 1 USD = INR
            <%=ConfigurationManager.AppSettings["ExchangeRate"]%></i>
    </p>
    <p><asp:Literal ID="lblNetWorthStatus" runat="server"/></p>    
    <p><asp:Literal ID="lblLiquidMoney" runat="server"/></p>    
   
      <%--RADGRID STARTS--%>
    <telerik:RadGrid ID="RadGridUpcomingExpenses" runat="server" Width="100%" PageSize="10" AllowPaging="True"
        AutoGenerateColumns="False" DataSourceID="sdsUpcomingExpenses" Skin="Office2010Silver" ShowFooter="True"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridUpcomingExpenses_OnItemDataBound">
        <MasterTableView DataSourceID="sdsUpcomingExpenses" CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="Purpose" HeaderText="Purpose"/>
                <telerik:GridBoundColumn DataField="Amount" Aggregate="Sum" HeaderText="Amount (INR)" />
				<telerik:GridDateTimeColumn DataField="LastDate" HeaderText="Last Date" DataFormatString="{0:dd MMMM yyyy}" />
                <telerik:GridBoundColumn DataField="TimeLeft" HeaderText="Time left" />
                <telerik:GridBoundColumn DataField="Category" HeaderText="Category"/>
                <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks"/>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    
     <asp:SqlDataSource ID="sdsPotentialMoney" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString %>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT * FROM IncomePotential ORDER BY SDate" />
    

    <asp:SqlDataSource ID="sdsUpcomingExpenses" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString %>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT * FROM UpcomingExpenses WHERE DATEPART(yy, LastDate) = 2016 ORDER BY LastDate" />
</asp:Content>
