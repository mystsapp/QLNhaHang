function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var editController = {
    init: function () {
        var optionValue = $('.ddlVanPhong').val();
        //editController.loadMaBan(optionValue);
        editController.registerEvent();
    },

    registerEvent: function () {
        
        $('.ddlVanPhong').off('change').on('change', function () {
            var optionValue = $(this).val();
            //$('#hidMaTD').val(optionValue);
            editController.loadMaBan(optionValue);
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
    },
    
    loadMaBan: function (optionValue) {
        $.ajax({
            url: '/Bans/GetNextMaBan',
            type: 'GET',
            data: {
                vanPhongId: optionValue
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    $('.txtMaBan').val(response.data);
                }
            }
        });
    }
};
editController.init();