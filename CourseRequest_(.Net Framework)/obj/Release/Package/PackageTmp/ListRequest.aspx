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
                    <a class="btn btn-primary" runat="server" href="~/Request">Перейти к заявке</a>
                </div>
            </div>
        </div>

        <hr />

        <!--начало фильтров-->
        <div class="container-fluid">
            <div class="row mt-3">
                <div class="col-md-4">
                    <label for="Year">Год</label>
                    <asp:DropDownList ID="yearSelect" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="YearSelect_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label for="Stat">Статус заявки</label>
                    <select class="form-control" id="Stat" name="Stat">
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
                    <input type="text" class="form-control" id="Dep" name="Dep">
                </div>
            </div>

            <div class="row mt-3 mb-4">
                <div class="col-md-4">
                    <label for="CourseBegin">Период проведения курса</label>
                    <div class="row">
                        <div class="col-md-6">
                            <input type="text" class="form-control date-input" id="CourseBegin_list" name="CourseBegin_list" placeholder="от" />
                        </div>
                        <div class="col-md-6">
                            <input type="text" class="form-control date-input" id="CourseEnd_list" name="CourseEnd_list" placeholder="до">
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <label for="FullName">ФИО обучаемого</label>
                    <input type="text" class="form-control" id="FullName" name="FullName">
                </div>
                <div class="col-md-4">
                    <label for="RequestNumber">Номер заявки</label>
                    <input type="text" class="form-control" id="RequestNumber" name="RequestNumber">
                </div>
            </div>
            <div class="row mt-3">
                <div class="col text-center">
                    <button type="button" id="applyFiltersBtn" class="btn btn-success">Применить фильтры</button>
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
        </div>
    </main>

</asp:Content>
