function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var hoaDonTuDongController = {
    init: function () {
        //hoaDonTuDongController.loadThanhTienVAT();
        hoaDonTuDongController.registerEvent();
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
            
            id = $(this).val();
            $('#hidVAT').val(id);
            $('#frmVAT').submit();
            
        });

        $('.txtPPV').off('blur').on('blur', function () {
            
            id = $(this).val();
            $('#hidPPV').val(id);
            $('#frmPPV').submit();
        });

        //var inputNumberVal = $('input.numbers').val();
        $('input.numbers').val(function (index, value) {
            return addCommas(value);
        });
        // format .numbers
        $('input.numbers').keyup(function (event) {

            // Chỉ cho nhập số
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return addCommas(value);
            });
        });
    },

    //loadThanhTienVAT: function () {
    //    var vat = $('.txtVAT').val();
    //    var thanhTienHD = parseFloat($('#hidThanhTienHD').val());
    //    var thanhTienVAT = thanhTienHD * vat / 100 + thanhTienHD;

    //    $('.txtThanhTienVAT').val(addCommas(thanhTienVAT));
    //}



};
hoaDonTuDongController.init();