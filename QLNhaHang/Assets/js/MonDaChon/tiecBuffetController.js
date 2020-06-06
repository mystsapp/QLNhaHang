function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var tiecBuffetController = {
    init: function () {
        
        tiecBuffetController.registerEvent();
    },

    registerEvent: function () {
        $('.ddlVP').off('change').on('change', function () {
            var optionValue = $(this).val();
            $('#hidVPName').val(optionValue);
            $('#hidSubmit').click();
        });

        //$('.tenMon').off('change').on('change', function () {
        //    var optionValue = $(this).val();
        //    $('#hidMaTD').val(optionValue);
        //    $('#hidSubmit').click();
        //});

        // setInterval : time
        $('#dropDown').off('change').on('change', function () {
            
            var optionValue = $(this).val();
            $('#hidMaTD').val(optionValue);
            $('#hidSubmit').click();
        });

        $('.txtSoLuong').off('blur').on('blur', function () {
            var optionValue = $('.tenMon').val();
            $('#hidMaTD').val(optionValue);
            var soLuong = $(this).val();
            $('#hidSoLuong').val(soLuong);
            // lay lun VPName de giu trang thai
            $('#hidVPName').val($('.ddlVP').val());
            $('#hidSubmit').click();
        });

        //var inputNumberVal = $('input.numbers').val();
        //$('input.numbers').val(function (index, value) {
        //    return addCommas(value);
        //});
        // format .numbers
        $('input.numbers').keyup(function (event) {

            // Chỉ cho nhập số
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return addCommas(value);
            });
        });
    }
};
tiecBuffetController.init();