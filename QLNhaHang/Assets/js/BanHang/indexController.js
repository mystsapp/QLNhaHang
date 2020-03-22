var indexController = {
    init: function () {
        indexController.registerEvent();
    },

    registerEvent: function () {

        ///////// change table image
        //$.ajax({
        //        url: '/CapThes/Index',
        //        data: {
        //            maCT: id
        //        },
        //        dataType: 'json',
        //        type: 'GET',
        //        success: function (response) {

        //        }
        //    });
        //$.each($('.cursor-pointer'), function (i, item) {
        //    if ($(this).prop('checked')) {
        //        idList.push({
        //            Id: $(item).data('id')
        //        });
        //    }

        //});

        //if (idList.length !== 0) {
        //    $('#stringId').val(JSON.stringify(idList));
        //    $('#frmExportAll').submit();
        //}
        //else {
        //    bootbox.alert({
        //        size: "small",
        //        title: "Information",
        //        message: "Bạn chưa chọn bàn giao!",
        //        callback: function () {
        //            //e.preventDefault();

        //        }
        //    });
        //}
        ///////// change table image
        
        $('.cursor-pointer').off('click').on('click', function () {
            id = $(this).data('id');
            
            $('#hidMaBan').val(id);

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

            $('#hidSubmit').click();
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

        $('#btnTinhTien').off('click').on('click', function () {

            var id = $('#hidMaBanTinhTien').val();
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
                $.ajax({
                    url: '/BanHangs/MonInBan',
                    data: {
                        maBan: id
                    },
                    dataType: 'json',
                    type: 'POST',
                    success: function (response) {
                        if (response.status) {
                            $('#frmTinhTien').submit();
                        }
                        else {
                            bootbox.alert({
                                size: "small",
                                title: "Information",
                                message: "Bàn này <b> chưa gọi món <b/> nào !",
                                callback: function () {
                                    //e.preventDefault();
                                }
                            });
                        }
                    }
                });
                
            }

               
        });
    }
};
indexController.init();