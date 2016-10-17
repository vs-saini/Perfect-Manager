<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Main.master" AutoEventWireup="true"
    CodeFile="MutualFund.aspx.cs" Inherits="Pages.PagesMutualFund " %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />
    
    <asp:Literal runat="server" ID="lblTotalInvestment"></asp:Literal>
    <br/><br/>
    
     <%--ICICI MF GROWTH--%>
    <label><i><b>Financial freedom and Retrirement planning :</b> Equity Balanced - ICICI Prudential Balanced Fund (Regular Plan - Growth)</i> <b>7377198</b></label>
    <telerik:RadGrid ID="RadGridIciciGrowth" runat="server" Width="50%" PageSize="12" AllowPaging="True"
        AutoGenerateColumns="False" Skin="Office2010Blue" ShowFooter="True"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridBirla_OnItemDataBound">
        <MasterTableView CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="Date" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                <telerik:GridBoundColumn DataField="TransactionType" HeaderText="Transaction Type" />
                <telerik:GridNumericColumn Aggregate="Sum" DataField="Amount" HeaderText="Amount (INR)"/>
				<telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    
     <%--ICICI MF TAX--%>
    <br/>
    <label><i><b>Gunjan college education :</b> Equity ELSS - ICICI Prudential Long Term Equity Fund (Tax Saving, Regular Plan - Growth)</i> <b>7377197</b></label>
    <telerik:RadGrid ID="RadGridIciciTax" runat="server" Width="50%" PageSize="12" AllowPaging="True"
        AutoGenerateColumns="False" Skin="Office2010Silver" ShowFooter="True"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridBirla_OnItemDataBound">
        <MasterTableView CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="Date" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                <telerik:GridBoundColumn DataField="TransactionType" HeaderText="Transaction Type" />
                <telerik:GridNumericColumn Aggregate="Sum" DataField="Amount" HeaderText="Amount (INR)"/>
				<telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>

    <%--BIRLA MF--%>
    <br/>
    <label><i><b>Gunjan marriage : </b> Equity Large Cap - Birla Sunlife Frontline Equity Fund (Growth Regular Plan)</i> <b>1016942614</b></label>
    <telerik:RadGrid ID="RadGridBirla" runat="server" Width="50%" PageSize="12" AllowPaging="True"
        AutoGenerateColumns="False" Skin="Hay" ShowFooter="True"
        AllowSorting="true" HorizontalAlign="Center" OnItemDataBound="RadGridBirla_OnItemDataBound">
        <MasterTableView CommandItemDisplay="None" ItemStyle-Wrap="true"
            Width="100%">
            <Columns>
                <telerik:GridBoundColumn DataField="Date" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}"/>
                <telerik:GridBoundColumn DataField="TransactionType" HeaderText="Transaction Type" />
                <telerik:GridNumericColumn Aggregate="Sum" DataField="Amount" HeaderText="Amount (INR)"/>
				<telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
