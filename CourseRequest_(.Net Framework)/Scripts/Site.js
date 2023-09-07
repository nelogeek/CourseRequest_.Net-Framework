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
    $('#Course_Start').val(formattedDate);
});




//$(document).ready(function () {
//    $('.datepicker').datepicker({
//        changeMonth: true,
//        changeYear: true,
//        todayHighlight: true,
//        format: "dd.mm.yyyy",
//        language: "ru"
//    });
//});


$(document).ready(function () {
    $('#Course_Start').mask('99.99.9999');
    $('#Course_End').mask('99.99.9999');
});