<%@ Page Async="true" Title="Создать заявку" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Request.aspx.cs" Inherits="CourseRequest__.Net_Framework_._Request" %>


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
                            <asp:LoginName ID="UserLoginName" runat="server" />
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
                    <label>ФИО обучаемого</label>
                    <select class="form-control" id="Full_Name" name="Full_Name" runat="server"></select>
                </section>

               <section class="col-md-4" aria-labelledby="CourseName">
                    <label>Наименование курса</label>
                    <div class="input-group">
                        <select class="form-control" id="Course_Name" name="Course_Name" runat="server"></select>
                        <div class="input-group-append">
                            <button type="button" id="showModalButton" class="btn btn-secondary">Добавить</button>
                        </div>
                    </div>
                </section>

                <!-- Модальное окно для добавления курса -->
                <div class="modal fade" id="addCourseModal" tabindex="-1" role="dialog" aria-labelledby="addCourseModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="addCourseModalLabel">Добавить новый курс</h5>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label for="newCourseNameInput">Название нового курса</label>
                                    <input type="text" class="form-control" id="newCourseNameInput" runat="server"/>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" id="hideModalButton">Закрыть</button>
                                <button type="button" class="btn btn-secondary" runat="server" id="createNewCourseNameButton" onserverclick="AddNewCourse_Click">Добавить</button>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-md-4" aria-labelledby="Status">
                    <label>Статус</label>
                    <select class="form-control" id="Status" name="Status" runat="server" disabled></select>
                </section>
            </div>

            <div class="row mb-4 justify-content-between">
                <section class="col-md-4" aria-labelledby="Department">
                    <label>Отдел</label>
                    <input type="text" class="form-control " id="Department" name="Department" placeholder="Введите значение" runat="server" autocomplete="off" readonly="readonly"/>
                </section>
                <section class="col-md-4" aria-labelledby="CourseType">
                    <label>Тип курса</label>
                    <select class="form-control " id="Course_Type" name="Course_Type" runat="server"></select>
                </section>
                <section class="col-md-4" aria-labelledby="CourseDate">
                    <label>Период проведения курса</label>
                    <div class="row">
                        <div class="col-md-6">
                            <input type="text" class="form-control date-start date-input " id="Course_Start" name="Course_Start" placeholder="от" runat="server"  autocomplete="off" />
                        </div>
                        <div class="col-md-6">
                            <input type="text" class="form-control date-input " id="Course_End" name="Course_End" placeholder="до" runat="server"  autocomplete="off" />
                        </div>
                    </div>
                </section>
            </div>

            <div class="row mb-4">

                <section class="col-md-4" aria-labelledby="Position">
                    <label>Должность</label>
                    <input type="text" class="form-control " id="Position" name="Position" placeholder="Введите значение" runat="server"  autocomplete="off" readonly="readonly"/>
                </section>
                <section class="col-md-4" aria-labelledby="Notation">
                    <label>Примечание</label>
                    <textarea class="form-control " id="Notation" name="Notation" rows="1" placeholder="Введите текст" runat="server"></textarea>
                </section>

            </div>


            <div class="row">

                <div class="col text-center">
                    <input type="hidden" id="User" name="User" value="" runat="server">
                    <button type="submit" id="createRequestButton" class="btn btn-primary text-white" runat="server" onserverclick="CreateRequestButton_Click" >Создать заявку</button>
                </div>
            </div>
            <div class="row">
                <div class="col-auto">
                    <span class="countApp">Кол-во заявок: <%= requestCount %> </span>
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
                                <td class="text-center"><a class="text-decoration-none" runat="server" href='<%# "~/Details.aspx?requestId=" + Eval("Id") %>'>Содержание</a></td>
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

    
    <script>
        $(document).ready(function () {
            $(document).ready(function () {
                $('#showModalButton').on('click', function () {
                    $('#MainContent_createNewCourseNameButton').prop('disabled', true);
                    $('#addCourseModal').modal('show');
                });

                $('#hideModalButton').on('click', function () {
                    $('#MainContent_newCourseNameInput').val(""); // Обратите внимание на изменение этой строки
                    $('#addCourseModal').modal('hide');
                });
            });

            // Начально кнопка "Добавить" должна быть выключена
            $('#MainContent_createNewCourseNameButton').prop('disabled', true);

            // Обработчик события на изменение поля ввода
            $('#MainContent_newCourseNameInput').on('input', function () {
                // Проверяем, есть ли текст в поле
                if ($(this).val().trim() !== '') {
                    // Если текст есть, включаем кнопку "Добавить"
                    $('#MainContent_createNewCourseNameButton').prop('disabled', false);
                } else {
                    // Если текст отсутствует, выключаем кнопку "Добавить"
                    $('#MainContent_createNewCourseNameButton').prop('disabled', true);
                }
            });

            // Обработчик события на клик по кнопке "Добавить"
            $('#MainContent_createNewCourseNameButton').on('click', function () {
                // Добавьте здесь код для добавления нового курса
            });
        });
    </script>

    



</asp:Content>






