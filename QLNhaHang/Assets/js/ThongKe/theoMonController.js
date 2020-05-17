function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var theoMonController = {
    init: function () {
        
        theoMonController.registerEvent();
    },
    registerEvent: function () {

        $('.ddlVanPhong').off('change').on('change', function () {
            $('#hidTuNgay').val($('.txtTuNgay').val());
            $('#hidDenNgay').val($('.txtDenNgay').val());
            $('#hidVanPhongId').val($('.ddlVanPhong').val());
            $('#hidKhuVucId').val($('.ddlKhuVuc').val());

            $('#frmTheoMon').submit();
        })
        
        //$('input.numbers').val(function (index, value) {
        //    return addCommas(value);
        //});
        // format .numbers
        //$('input.numbers').keyup(function (event) {

        //    // Chỉ cho nhập số
        //    if (event.which >= 37 && event.which <= 40) return;

        //    $(this).val(function (index, value) {
        //        return addCommas(value);
        //    });
        //});

     

    }

};
theoMonController.init();