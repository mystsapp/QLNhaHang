var modalTTHDController = {
    init: function () {
        //modalTTHDController.loadThanhTienVAT();
        modalTTHDController.registerEvent();
    },

    registerEvent: function () {
        //$('.tdValTTHD').click(function () {
        //    var id = $(this).data('id');
        //    $.ajax({
        //        url: '/HoaDons/GetByTTHDId',
        //        data: {
        //            id: id
        //        },
        //        dataType: 'json',
        //        type: 'GET',
        //        success: function (response) {
        //            var data = JSON.parse(response);
        //            console.log(data);
        //            $('.txtSTT').val(data.SoThuTu);
        //            $('.txtSoHD').val(data.So);
        //            $('.txtKyHieu').val(data.KyHieu);
        //            $('.txtQuyenSo').val(data.QuyenSo);
        //            $('.txtMauSo').val(data.MauSo);
        //            $('#hidThongTinHDId').val(data.Id);

        //            $('#modal-ThongTinHD').modal('hide');
        //        }
        //    });

        //});

       
    }

    //loadThanhTienVAT: function () {
    //    var vat = $('.txtVAT').val();
    //    var thanhTienHD = parseFloat($('#hidThanhTienHD').val());
    //    var thanhTienVAT = thanhTienHD * vat / 100 + thanhTienHD;

    //    $('.txtThanhTienVAT').val(addCommas(thanhTienVAT));
    //}



};
modalTTHDController.init();