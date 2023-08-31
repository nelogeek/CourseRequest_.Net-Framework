<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListRequest.aspx.cs" Inherits="CourseRequest__.Net_Framework_._ListRequest" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="container-fluid">
            <div class="row mt-3">
                <div class="col">
                    <div>
                        <b>Новая заявка</b>
                    </div>
                    <div>
                        <span class="user">Пользователь:
                            <asp:LoginName runat="server" />
                        </span>

                    </div>
                </div>
                <div class="col-auto text-center">
                    <a class="btn btn-primary" runat="server" href="~/Request">Перейти к заявке</a>
                </div>
            </div>
        </div>

        <hr />

        <div class="container-fluid">Перенести фильтры и таблицу</div>
    </main>

</asp:Content>
