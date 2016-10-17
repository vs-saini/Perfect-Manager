<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Main.master" AutoEventWireup="true"
    CodeFile="Potential.aspx.cs" Inherits="Pages.PagesPotentialMoney" %>

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
        AutoGenerateColumns="False" DataSourceID="sdsPotentialMoney" Skin="Office2007"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridEaring_OnItemDataBound">
        <MasterTableView DataSourceID="sdsPotentialMoney" CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="From" HeaderText="From Source" />
                <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount (INR)" />
                <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount (USD)" DataFormatString="${0:#.00}"/>
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
    <asp:SqlDataSource ID="sdsPotentialMoney" runat="server" ConnectionString="<%$ ConnectionStrings:sdfConString %>"
        ProviderName="<%$ ConnectionStrings:sdfConString.ProviderName %>" SelectCommand="SELECT * FROM IncomePotential ORDER BY SDate" />
</asp:Content>
