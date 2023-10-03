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
                <a class="btn btn-primary" runat="server" id="BackBtn" href="~/ListRequest">Вернуться к списку</a>
            </div>
        </div>
    </div>

    <hr />


    <section class="container-fluid">
        <div class="row mt-2 mb-4 justify-content-center">
            <section class="col-md-4" aria-labelledby="FullName">
                <label>ФИО обучаемого</label>
                <input type="text" class="form-control col-md-12" id="Full_Name_det" name="Full_Name_det" placeholder="Введите значение" runat="server" disabled />
            </section>

            <section class="col-md-4" aria-labelledby="CourseName">
                <label>Наименование курса</label>
                <input type="text" class="form-control col-md-12" id="Course_Name_det" name="Course_Name_det" placeholder="Введите значение" runat="server" disabled />
            </section>

            <section class="col-md-4" aria-labelledby="Status">
                <label>Статус</label>
                <select class="form-control" id="Status_det" name="Status_det" runat="server" disabled></select>
            </section>
        </div>

        <div class="row mb-4 justify-content-between">
            <section class="col-md-4" aria-labelledby="Department">
                <label>Отдел</label>
                <input type="text" class="form-control" id="Department_det" name="Department_det" placeholder="Введите значение" runat="server" disabled />
            </section>
            <section class="col-md-4" aria-labelledby="CourseType">
                <label>Тип курса</label>
                <input type="text" class="form-control" id="Course_Type_det" name="Course_Type_det" placeholder="Введите значение" runat="server" disabled />
            </section>
            <section class="col-md-4" aria-labelledby="CourseDate" >
                <label>Период проведения курса</label>
                <div class="row">
                    <div class="col-md-6">
                        <input type="text" class="form-control date-start date-input" id="Course_Start_det" name="Course_Start" placeholder="от" runat="server" disabled />
                    </div>
                    <div class="col-md-6">
                        <input type="text" class="form-control date-input" id="Course_End_det" name="Course_End_det" placeholder="до" runat="server" disabled />
                    </div>
                </div>
            </section>
        </div>

        <div class="row mb-4">

            <section class="col-md-4" aria-labelledby="Position">
                <label>Должность</label>
                <input type="text" class="form-control" id="Position_det" name="Position_det" placeholder="Введите значение" runat="server" disabled />
            </section>
            <section class="col-md-4" aria-labelledby="Notation">
                <label>Примечание</label>
                <textarea class="form-control" id="Notation_det" name="Notation_det" rows="1" placeholder="Введите текст" runat="server" disabled></textarea>
            </section>

        </div>
        <div class="row">

                <div class="col text-center">
                    <input type="hidden" id="User" name="User" value="" runat="server">
                    <button type="submit" id="updateRequestButton" class="btn btn-primary btn-custom-outline-orange text-white" runat="server" onserverclick="UpdateRequestButton_Click" visible="false">Сохранить изменения</button>

                </div>
            </div>

    </section>



</asp:Content>


