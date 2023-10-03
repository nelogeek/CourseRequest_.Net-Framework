<%@ Page Title="Список заявок" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListRequest.aspx.cs" Inherits="CourseRequest__.Net_Framework_._ListRequest" %>

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
                    <a class="btn btn-primary" id="NewRequestBtn" runat="server" href="~/Request">Новая заявка</a>
                </div>
            </div>
        </div>

        <hr />

        <!--начало фильтров-->
        <div class="container-fluid">
            <div class="row mt-3">
                <div class="col-md-4">
                    <label for="Year">Год</label>
                    <select id="yearSelect" name="yearSelect" class="form-control" runat="server"></select>
                </div>
                <div class="col-md-4">
                    <label for="Stat">Статус заявки</label>
                    <select class="form-control" id="Stat" name="Stat" runat="server">
                        <option value=""></option>
                        <option value="1">Новая</option>
                        <option value="2">В работе</option>
                        <option value="3">Заявлен на обучение</option>
                        <option value="4">Курс пройден</option>
                        <option value="5">Курс не пройден</option>
                    </select>
                </div>
                <div class="col-md-4">
                    <label for="Dep">Отдел</label>
                    <select class="form-control" id="Department" name="Department" runat="server" ></select>
                </div>
            </div>

            <div class="row mt-3 mb-4">
                <div class="col-md-4">
                    <label for="CourseBegin">Период проведения курса</label>
                    <div class="row">
                        <div class="col-md-6">
                            <input type="text" class="form-control date-input" id="CourseBegin_list" name="CourseBegin_list" placeholder="от" runat="server" autocomplete="off" />
                        </div>
                        <div class="col-md-6">
                            <input type="text" class="form-control date-input" id="CourseEnd_list" name="CourseEnd_list" placeholder="до" runat="server" autocomplete="off" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <label for="Full_Name">ФИО обучаемого</label>
                    <select class="form-control" id="Full_Name" name="Full_Name" runat="server" ></select>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-6">
                            <label>Тип курса</label>
                            <select class="form-control" id="Course_Type_list" name="Course_Type_list" runat="server"></select>
                        </div>
                        <div class="col-md-6">
                            <label>Показать</label>
                            <select class="form-control" id="OnlyMyReq" name="OnlyMyReq" runat="server">
                                <option value="">Все заявки</option>
                                <option value="1">Только мои</option>
                            </select>
                        </div>
                    </div>

                </div>
            </div>

            <div class="row mt-3 mb-4">
                <div class="col-md-4">
                </div>
                <div class="col-md-4 ">
                    <label>Наименование курса</label>
                    <select class="form-control" id="Course_Name_list" name="Course_Name_list" runat="server" required></select>
                </div>
                <div class="col-md-4">
                </div>

            </div>
            <div class="row mt-3 ">

                <div class="col text-center">
                    <button type="button" id="applyFiltersBtn" class="btn btn-success" runat="server" onserverclick="FilterRequestButton_Click">Применить фильтры</button>
                </div>
            </div>
            <!--конец фильтров-->

            <div class="row">
                <div class="col-auto">
                    <span class="countApp">Кол-во заявок: <%= requestCount %> </span>
                </div>
            </div>
        </div>

        <div class="row mb-4 mt-2">

            <hr class="mt-2 mb-0" style="color: grey" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Repeater ID="RepeaterRequests" runat="server" ItemType="CourseRequest__.Net_Framework_.Models.Request">
                        <HeaderTemplate>
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th class="text-center">Номер заявки</th>
                                        <th class="text-center">Статус</th>
                                        <th class="text-center">ФИО обучаемого</th>
                                        <th class="text-center">Наименование курса</th>
                                        <th class="text-center">Тип курса</th>
                                        <th class="text-center">Дата обучения</th>
                                        <th class="text-center"></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>


                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td class="text-center"><%# Eval("Id") %></td>
                                    <td class="text-center"><%# Eval("Status") %></td>
                                    <td class="text-center"><%# Eval("Full_Name") %></td>
                                    <td class="text-center"><%# Eval("Course_Name") %></td>
                                    <td class="text-center"><%# Eval("Course_Type") %></td>
                                    <td class="text-center"><%# Convert.ToDateTime(Eval("Course_Start")).ToString("dd.MM.yyyy") %> - <%# Convert.ToDateTime(Eval("Course_End")).ToString("dd.MM.yyyy") %></td>
                                    <td class="text-center">
                                        <a class="text-decoration-none" runat="server" href='<%# "~/Details.aspx?requestId=" + Eval("Id") %>'>Содержание</a>
                                    </td>

                                </tr>
                            </tbody>
                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </main>





</asp:Content>