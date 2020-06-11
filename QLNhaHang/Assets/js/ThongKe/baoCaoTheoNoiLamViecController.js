var baoCaoTheoNoiLamViecController = {
    init: function () {
        
        baoCaoTheoNoiLamViecController.registerEvent();
    },

    registerEvent: function () {

        $('.ddlKhuVuc').off('change').on('change', function () {
            $('#hidTuNgay').val($('.txtTuNgay').val());
            $('#hidDenNgay').val($('.txtDenNgay').val());
            
            $('#hidKhuVucId').val($('.ddlKhuVuc').val());

            $('#frmTheoNoiLamViec').submit();
        })
    },
};
baoCaoTheoNoiLamViecController.init();