var hoaDonTuDongController = {
    init: function () {
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

    }



};
hoaDonTuDongController.init();