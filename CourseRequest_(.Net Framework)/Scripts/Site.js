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




$(document).ready(function () {
    var $full_Name = $("#MainContent_Full_Name");
    var $dept = $("#MainContent_Department");
    var $position = $("#MainContent_Position");

    $full_Name.on("change", function () {
        // Получите выбранный элемент в списке
        var selectedOption = $full_Name.find("option:selected");

        // Получите данные "Отдел" и "Должность" из атрибутов элемента option или из другого источника, если они не хранятся в option
        var dept = selectedOption.data("dept"); // Замените "data-dept" на ваш атрибут
        var position = selectedOption.data("position"); // Замените "data-position" на ваш атрибут

        // Заполните поля "Отдел" и "Должность"
        $dept.val(dept);
        $position.val(position);
    });
});



