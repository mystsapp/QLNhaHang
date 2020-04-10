function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var hoaDonTayController = {
    init: function () {
        hoaDonTayController.registerEvent();
    },

    registerEvent: function () {
        $('.tdValTT').click(function () {
            id = $(this).data('id');
            $('#hidMaThongTinHDId').val(id);

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

            $('#frmThongTinHD').submit();
        });

        $('.tdValKH').click(function () {
            id = $(this).data('id');
            $('#hidMaKH').val(id);

            $('#frmKhachHang').submit();
        });

        $('.txtVAT').off('blur').on('blur', function () {
            //hoaDonTuDongController.loadThanhTienVAT();
            id = $(this).val();
            //sotien = $('.txtSoTien').val();
            $('#hidVAT').val(id);
            //$('#hidSoTien').val(sotien);
            $('#frmVAT').submit();
        });

        $('.txtPPV').off('blur').on('blur', function () {
            //hoaDonTuDongController.loadThanhTienVAT();
            var ppv = $(this).val();
            var sotien = $('.txtSoTien').val();
            if (sotien === "") {
                bootbox.alert({
                    size: "small",
                    title: "Information",
                    message: "Bạn chưa nhập <b>số tiền</b>!",
                    callback: function () {
                        //e.preventDefault();

                    }
                });
            }
            else {
                $('#hidPPV').val(ppv);
                $('#soTien').val(sotien);
                $('#frmPPV').submit();
            }

        });

        $('.txtSoTien').off('blur').on('blur', function () {
            //hoaDonTuDongController.loadThanhTienVAT();
            var ppv = $('.txtPPV').val();
            var sotien = $('.txtSoTien').val();
            $('#hidPPV').val(ppv);
            $('#soTien').val(sotien);
            $('#frmPPV').submit();
            //if (sotien === "") {
            //    bootbox.alert({
            //        size: "small",
            //        title: "Information",
            //        message: "Bạn chưa nhập <b>số tiền</b>!",
            //        callback: function () {
            //            //e.preventDefault();

            //        }
            //    });
            //}
            //else {
            //    $('#hidPPV').val(ppv);
            //    $('#soTien').val(sotien);
            //    $('#frmPPV').submit();
            //}

        });

        $('input.numbers').val(function (index, value) {
            return addCommas(value);
        });
        $('input.numbers').keyup(function (event) {

            // Chỉ cho nhập số
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return addCommas(value);
            });
        });

    }



};
hoaDonTayController.init();