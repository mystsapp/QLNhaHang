var indexController = {
    init: function () {
        indexController.registerEvent();
    },

    registerEvent: function () {
        $('tr td.tdVal').click(function () {
            id = $(this).data('id');
            $('#hidMaHD').val(id);
            //var page = $('.active .page-link').text();
            var page = $('.active a').text();
            $('#hidPage').val(page);
            //$.ajax({
            //    url: '/CapThes/Index',
            //    data: {
            //        maCT: id
            //    },
            //    dataType: 'json',
            //    type: 'GET',
            //    success: function (response) {

            //    }
            //});

            $('#btnSubmit').click();
        });
    }
};
indexController.init();