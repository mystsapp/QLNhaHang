var indexController = {
    init: function () {
        
        indexController.registerEvent();
    },

    registerEvent: function () {

        $('.ddlKhuVuc').off('change').on('change', function () {
            var optionValue = $(this).val();
            $('#hidDdlLoai').val(optionValue);
            $('#btnSubmit').click();
        });

        shortcut.add("F1", function () {
            indexController.goiMon();
        });
        shortcut.add("F2", function () {
            indexController.tinhTien();
        });
        shortcut.add("F3", function () {
            $('#modal-MayTinh').modal('show');
            $('#modal-MayTinh').on('shown.bs.modal', function () {
                $('#txtTongTien').focus();
            });
            
        });
        
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

            indexController.goiMon();
        });

        $('#btnTinhTien').off('click').on('click', function () {

            indexController.tinhTien();
        });

        $('#btnInHoaDon').off('click').on('click', function () {

            var id = $('#hidMaBanInHoaDon').val();
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
                            $('#frmInHoaDon').submit();
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

    },

    goiMon: function () {
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
    },

    tinhTien: function () {
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
                type: 'GET',
                success: function (response) {
                    var data = JSON.parse(response.data);

                    if (response.status) {
                        var j = 0;
                        var tenMonChuaLam = '';
                        $.each(data, function (i, item) {
                            if (!item.DaLam) {
                                j++;
                                tenMonChuaLam += item.ThucDon.TenMon + ', '
                            }
                        });
                        
                        var tenMonString = tenMonChuaLam.substring(0, tenMonChuaLam.length - 2);
                        if (j !== 0) {
                            bootbox.alert({
                                size: "small",
                                title: "Information",
                                message: tenMonString + " <b> chưa làm <b/> !",
                                callback: function () {
                                    //e.preventDefault();

                                }
                            });
                        }
                        else {
                            $('#frmTinhTien').submit();
                        }
                        
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
    }
};
indexController.init();