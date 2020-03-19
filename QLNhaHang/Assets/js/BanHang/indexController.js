var indexController = {
    init: function () {
        indexController.registerEvent();
    },

    registerEvent: function () {
        $('.cursor-pointer').click(function () {
            id = $(this).data('id');
            $('#hidMaBan').val(id);
            $('#hidMaBanGoiMon').val(id);

            //var page = $('.active span').text();
            //$('#hidPage').val(page);
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

        $('#btnGoiMon').off('click').on('click', function () {
            var id = $('#hidMaBanGoiMon').val();
            if (id === '') {
                bootbox.alert({
                    size: "small",
                    title: "Information",
                    message: "Bạn chưa chọn <b> bàn </b> nào !",
                    callback: function () {
                        //e.preventDefault();
                    }
                });

            }
            else {
                $('#frmGoiMon').submit();
            }
        });
    }
};
indexController.init();