<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Request.aspx.cs" Inherits="CourseRequest__.Net_Framework_._Request" %>

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
                    <a class="btn btn-primary" runat="server" href="~/ListRequest">Перейти к списку</a>
                </div>
            </div>
        </div>

        <hr />

        <section class="container-fluid">
            <div class="row mt-2 mb-4 justify-content-center">
                <section class="col-md-4" aria-labelledby="FullName">
                    <label for="FullName">ФИО обучаемого</label>
                    <input type="text" class="form-control col-md-12" id="Full_Name" name="Full_Name" placeholder="Введите значение" required />
                    <div id="similarNamesList" class="list-group" style="position: absolute; z-index: 1; display: none; max-height: calc(100vh / 3); overflow-y: auto; border: 2px solid #e2e2e2;"></div>
                </section>

                <section class="col-md-4" aria-labelledby="CourseName">
                    <label for="CourseName">Наименование курса</label>
                    <textarea class="form-control" id="Course_Name" name="Course_Name" rows="1" placeholder="Введите текст" required></textarea>
                </section>

                <section class="col-md-4" aria-labelledby="Status">
                    <label for="Status">Статус</label>
                    <select class="form-control" id="Status" name="Status" disabled>
                        <option value="1">Новая</option>
                        <option value="2">В работе</option>
                        <option value="3">Заявлен на обучение</option>
                        <option value="4">Курс пройден</option>
                        <option value="5">Курс не пройден</option>
                    </select>
                </section>
            </div>

            <div class="row mb-4 justify-content-between">
                <section class="col-md-4" aria-labelledby="Department">
                    <label for="Department">Отдел</label>
                    <input type="text" class="form-control" id="Department" name="Department" placeholder="Введите значение" required />
                </section>
                <section class="col-md-4" aria-labelledby="CourseType">
                    <label for="CourseType">Тип курса</label>
                    <select class="form-control" id="Course_Type" name="Course_Type">
                        <option value="1">Базовый</option>
                        <option value="2">Продвинутый</option>
                        <option value="3">Для администраторов</option>
                    </select>
                </section>
                <section class="col-md-4" aria-labelledby="CourseDate">
                    <label for="Course_Start">Период проведения курса</label>
                    <div class="row">
                        <div class="col-md-6">
                            <input type="text" class="form-control" id="Course_Start" name="Course_Start" placeholder="от" required />
                        </div>
                        <div class="col-md-6">
                            <input type="text" class="form-control" id="Course_End" name="Course_End" placeholder="до" required />
                        </div>
                    </div>
                </section>
            </div>

            <div class="row mb-4">

                <section class="col-md-4" aria-labelledby="Position">
                    <label for="Position">Должность</label>
                    <input type="text" class="form-control" id="Position" name="Position" placeholder="Введите значение" required />
                </section>
                <section class="col-md-4" aria-labelledby="Notation">
                    <label for="Notation">Примечание</label>
                    <textarea class="form-control" id="Notation" name="Notation" rows="1" placeholder="Введите текст"></textarea>
                </section>

            </div>

            <div class="row">
                <div class="col-auto">
                    <span class="countApp">Количество ролей: <%= requestCount %> </span>
                </div>
            </div>
            <div class="row">

                <div class="col text-center">
                    <input type="hidden" id="User" name="User" value="">
                    <button type="submit" class="btn btn-primary btn-custom-outline-orange text-white">Создать заявку</button>
                </div>
            </div>


            <div class="row mb-4 mt-2">

                <hr class="mt-2 mb-0" style="color: grey" />

                <asp:Repeater ID="RepeaterRequests" runat="server" ItemType="CourseRequest__.Net_Framework_.Models.Request">
                    <HeaderTemplate>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th class="text-center">ФИО обучаемого</th>
                                    <th class="text-center">Должность</th>
                                    <th class="text-center">Наименование курса</th>
                                    <th class="text-center">Тип курса</th>
                                    <th class="text-center">Примечание</th>
                                    <th class="text-center"></th>
                                </tr>
                            </thead>
                    </HeaderTemplate>


                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td class="text-center"><%# Eval("Full_Name") %></td>
                                <td class="text-center"><%# Eval("Position") %></td>
                                <td class="text-center"><%# Eval("Course_Name") %></td>
                                <td class="text-center"><%# Eval("Course_Type") %></td>
                                <td class="text-center"><%# Eval("Notation") %></td>
                                <td class="text-center"><a class="text-decoration-none" runat="server" href="~/ListRequest">Содержание</a></td>
                            </tr>
                        </tbody>
                    </ItemTemplate>

                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>







        </section>
    </main>

</asp:Content>
