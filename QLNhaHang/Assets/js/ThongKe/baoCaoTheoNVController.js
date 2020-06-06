var baoCaoTheoNVController = {
    init: function () {
        
        baoCaoTheoNVController.registerEvent();
    },

    registerEvent: function () {

        $('.ddlKhuVuc').off('change').on('change', function () {
            $('#hidTuNgay').val($('.txtTuNgay').val());
            $('#hidDenNgay').val($('.txtDenNgay').val());
            
            $('#hidKhuVucId').val($('.ddlKhuVuc').val());

            $('#frmTheoNhanVien').submit();
        })
    },
};
baoCaoTheoNVController.init();