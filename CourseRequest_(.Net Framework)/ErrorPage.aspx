<%@ Page Title="Ошибка" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="CourseRequest__.Net_Framework_.ErrorPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Произошла ошибка</h2>
        <p>Извините, но произошла ошибка при обработке вашего запроса.</p>
        <p>Пожалуйста, вернитесь на <a href="~/Request.aspx">главную страницу</a> и попробуйте снова.</p>
        <hr />
        <p>Сведения об ошибке:</p>
        <pre>
            <asp:Label ID="ErrorDetails" runat="server" EnableViewState="False" />
        </pre>
    </div>
</asp:Content>
