<%@ page title="Home Page" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="Default.aspx.cs" inherits="Airbrake.Web.HttpException._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-4">
        <div class="form-group">
            <asp:Label AssociatedControlID="BookTitle" runat="server">Title</asp:Label>
            <asp:TextBox ID="BookTitle" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookAuthor" runat="server">Author</asp:Label>
            <asp:TextBox ID="BookAuthor" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookPageCount" runat="server">Page Count</asp:Label>
            <asp:TextBox ID="BookPageCount" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookPublicationDate" runat="server">Publication Date</asp:Label>
            <asp:Calendar ID="BookPublicationDate" runat="server"></asp:Calendar>
        </div>
        <asp:Button ID="BookSubmit" CssClass="btn btn-primary" Text="Submit" OnClick="BookSubmit_Click" runat="server" />
        <br />
        <asp:Label ID="BookLabel" runat="server"></asp:Label>
    </div>
</asp:Content>
