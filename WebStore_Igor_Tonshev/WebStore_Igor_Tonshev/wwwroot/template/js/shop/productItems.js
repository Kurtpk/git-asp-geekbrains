ProductItems = {
    _options: {
        getUrl: ''
    },
    init: function (options) {
        $.extend(ProductItems._options, options);
        $('.pagination li a').on('click', ProductItems.clickOnPage);
        $('#itemsContainer').LoadingOverlay('show');// Показываем overlay
        var page = ProductItems.getHash('page');
        if (page === null) {
            page = 1;
        }
        ProductItems.getItems(window.location.search, page);
    },
    clickOnPage: function (event) {
        event.preventDefault();
        if ($(this).prop('href').length > 0) {

            var page = $(this).data('page');
            window.location.hash = 'page=' + page;

            $('#itemsContainer').LoadingOverlay('show');// Показываем overlay

            var data = $(this).data();// Получаем все атрибуты
            // Строим строку запроса
            var query = '';
            for (var key in data) {
                if (key !== 'page' && data.hasOwnProperty(key)) {
                    query += key + '=' + data[key] + '&';
                }
            }

            // Делаем запрос на сервер
            ProductItems.getItems(query, page);
        }
    },
    getItems: function (query, page) {
        $.get(ProductItems._options.getUrl + '?' + query + "&page=" + page).done(
            function (result) {
                // Заполняем результат и убираем overlay
                $('#itemsContainer').html(result);
                $('#itemsContainer').LoadingOverlay('hide');

                // Обновляем пейджинг
                $('.pagination li').removeClass('active');
                $('.pagination li a').prop('href', '#');
                $('.pagination li a[data-page=' + page + ']').removeAttr('href').parent().addClass('active');

            }).fail(function () {
                console.log('clickOnPage getItems error');
                $('#itemsContainer').LoadingOverlay('hide');
            });
    },

    getHash: function (key) {
        var matches = location.hash.match(new RegExp(key + '=([^&]*)'));
        return matches ? matches[1] : null;
    }
}
