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