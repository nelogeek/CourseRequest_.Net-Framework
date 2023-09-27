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
    // Ссылка на поле ввода ФИО
    var $input = $("#MainContent_Full_Name");
    // Ссылка на контейнер для списка схожих имен
    var $listGroup = $("#similarNamesList");

    // Обработчик события ввода в поле ФИО
    $input.on("input", function () {
        // Получаем текст из поля ввода
        var input = $(this).val();
        if (input.length >= 1) {
        // Вызываем серверный метод GetSimilarNames для получения схожих имен
        $.ajax({
            type: "POST",
            url: "Request.aspx/GetSimilarNames",
            data: JSON.stringify({ input: input }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                // Очищаем список схожих имен
                $listGroup.empty();

                // Перебираем полученные данные и создаем элементы списка
                data.d.forEach(function (item) {
                    var listItem = $("<a href='#' class='list-group-item list-group-item-action'></a>").text(item);
                    listItem.click(function () {
                        // Заполняем поле ФИО выбранным именем
                        $input.val(item);
                        // Другие действия при выборе из списка
                        $listGroup.hide();
                    });

                    // Добавляем элемент списка в контейнер
                    $listGroup.append(listItem);
                });

                // Отображаем контейнер с схожими именами
                $listGroup.show();
            },
            error: function (error) {
                console.log(error);
            }
        });
        } else {
        // Если поле ввода пустое, скрываем контейнер с схожими именами
        $listGroup.hide();
        }
    });

    // Закрытие списка схожих имен при клике вне списка и поля ввода
    $(document).on("click", function (e) {
        if (!($listGroup.is(e.target) || $input.is(e.target) || $listGroup.has(e.target).length > 0 || $input.has(e.target).length > 0)) {
        $listGroup.hide();
        }
    });
});

