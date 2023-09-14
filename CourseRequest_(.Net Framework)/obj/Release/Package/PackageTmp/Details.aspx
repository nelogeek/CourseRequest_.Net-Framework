<%@ Page Title="Содержание заявки" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CourseRequest__.Net_Framework_._Details" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">
        <div class="row mt-3">
            <div class="col">
                <div>
                    <b>Содержание заявки</b>
                </div>
                <div>
                    <span class="user">Создатель заявки: <%= UserName %></span>
                </div>
                <div>
                    <span class="user">Номер заявки: <%= requestId %></span>
                </div>
            </div>
            <div class="col-auto text-center">
                <a class="btn btn-primary" runat="server" href="~/ListRequest">Вернуться к списку</a>
            </div>
        </div>
    </div>

    <hr />


    <section class="container-fluid">
        <div class="row mt-2 mb-4 justify-content-center">
            <section class="col-md-4" aria-labelledby="FullName">
                <label>ФИО обучаемого</label>
                <input type="text" class="form-control col-md-12" id="Full_Name" name="Full_Name" placeholder="Введите значение" runat="server" required />
                <div id="similarNamesList" class="list-group" style="position: absolute; z-index: 1; display: none; max-height: calc(100vh / 3); overflow-y: auto; border: 2px solid #e2e2e2;"></div>
            </section>

            <section class="col-md-4" aria-labelledby="CourseName">
                <label>Наименование курса</label>
                <textarea class="form-control" id="Course_Name" name="Course_Name" rows="1" placeholder="Введите текст" runat="server" required></textarea>
            </section>

            <section class="col-md-4" aria-labelledby="Status">
                <label>Статус</label>
                <select class="form-control" id="Status" name="Status" runat="server">
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
                <label>Отдел</label>
                <input type="text" class="form-control" id="Department" name="Department" placeholder="Введите значение" runat="server" required />
            </section>
            <section class="col-md-4" aria-labelledby="CourseType">
                <label>Тип курса</label>
                <select class="form-control" id="Course_Type" name="Course_Type" runat="server">
                    <option value="1">Базовый</option>
                    <option value="2">Продвинутый</option>
                    <option value="3">Для администраторов</option>
                </select>
            </section>
            <section class="col-md-4" aria-labelledby="CourseDate">
                <label>Период проведения курса</label>
                <div class="row">
                    <div class="col-md-6">
                        <input type="text" class="form-control date-start date-input" id="Course_Start" name="Course_Start" placeholder="от" runat="server" required />
                    </div>
                    <div class="col-md-6">
                        <input type="text" class="form-control date-input" id="Course_End" name="Course_End" placeholder="до" runat="server" required />
                    </div>
                </div>
            </section>
        </div>

        <div class="row mb-4">

            <section class="col-md-4" aria-labelledby="Position">
                <label>Должность</label>
                <input type="text" class="form-control" id="Position" name="Position" placeholder="Введите значение" runat="server" required />
            </section>
            <section class="col-md-4" aria-labelledby="Notation">
                <label>Примечание</label>
                <textarea class="form-control" id="Notation" name="Notation" rows="1" placeholder="Введите текст" runat="server"></textarea>
            </section>

        </div>
        <div class="row">

                <div class="col text-center">
                    <input type="hidden" id="User" name="User" value="" runat="server">
                    <button type="submit" id="createRequestButton" class="btn btn-primary btn-custom-outline-orange text-white" runat="server" onserverclick="UpdateRequestButton_Click">Сохранить изменения</button>

                </div>
            </div>

    </section>



</asp:Content>


