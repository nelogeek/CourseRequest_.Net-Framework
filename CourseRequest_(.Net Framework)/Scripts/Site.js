//Вывод текущей даты
$(document).ready(function () {
    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1; // Месяцы в JavaScript начинаются с 0
    var year = currentDate.getFullYear();

    // Добавляем ведущий ноль для дней и месяцев, если они состоят из одной цифры
    if (day < 10) {
        day = '0' + day;
    }
    if (month < 10) {
        month = '0' + month;
    }

    var formattedDate = day + '.' + month + '.' + year;
    $('.date-start').val(formattedDate);
});




 // Маска для полей с датой
$(document).ready(function () {
    $('.date-input').mask('99.99.9999');
});

// Фильтрация заявок
$(document).ready(function () {
    $('#applyFiltersBtn').click(function () {
        // Получите значения фильтров из полей формы
        var year = $('#yearSelect').val();
        var status = $('#Stat').val();
        var department = $('#Dep').val();
        var courseBegin = $('#CourseBegin_list').val();
        var courseEnd = $('#CourseEnd_list').val();
        var fullName = $('#FullName').val();
        var requestNumber = $('#RequestNumber').val();

        // Отправьте запрос на сервер для применения фильтров
        $.ajax({
            type: "POST",
            url: "ListRequest.aspx/ApplyFilters", // Путь к методу на сервере
            data: JSON.stringify({
                year: year,
                status: status,
                department: department,
                courseBegin: courseBegin,
                courseEnd: courseEnd,
                fullName: fullName,
                requestNumber: requestNumber
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                // Обновите таблицу с данными
                // data содержит результаты после применения фильтров
            },
            error: function () {
                alert('Произошла ошибка при отправке запроса на сервер.');
            }
        });
    });
});




