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

        //$('#btnExportAll').off('click').on('click', function () {

            //indexController.exportList();

        //});

        $('#btnSaveHDModal').off('click').on('click', function () {

            indexController.exportList();

        });

        
    },
    exportList: function () {
        var idList = [];
        $.each($('.ckId'), function (i, item) {
            if ($(this).prop('checked')) {
                idList.push({
                    Id: $(item).data('id')
                });
            }

        });

        if (idList.length !== 0) {
            $('#stringId').val(JSON.stringify(idList));
            $('#frmExportAll').submit();
        }
        else {
            bootbox.alert({
                size: "small",
                title: "Information",
                message: "Bạn chưa chọn bàn giao!",
                callback: function () {
                    //e.preventDefault();

                }
            });
        }


        //if (idList.length === 1) {
        //    $.ajax({
        //        url: '/BanGiaos/ExportToWord',
        //        type: 'GET',
        //        data: {
        //            idDataList: JSON.stringify(idList)
        //        },
        //        dataType: 'json',
        //        success: function (response) {
        //            if (response.status) {
        //                bootbox.alert({
        //                    size: "small",
        //                    title: "Information",
        //                    message: "OK Men!",
        //                    callback: function () {
        //                        //e.preventDefault();

        //                    }
        //                });


        //            }
        //        }
        //    });
        //}
        //else {
        //    $.ajax({
        //        url: '/BanGiaos/ExportList',
        //        type: 'GET',
        //        data: {
        //            idDataList: JSON.stringify(idList)
        //        },
        //        dataType: 'json',
        //        success: function (response) {
        //            if (response.status) {
        //                bootbox.alert({
        //                    size: "small",
        //                    title: "Information",
        //                    message: "OK Men!",
        //                    callback: function () {
        //                        //e.preventDefault();

        //                    }
        //                });


        //            }
        //        }
        //    });
        //}

    }
};
indexController.init();