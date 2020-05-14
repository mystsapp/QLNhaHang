function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var editController = {
    init: function () {
        var roleId = $('.ddlRole').val();
        editController.loadKVByRole(roleId);
        editController.registerEvent();
        ////// check role selected
        editController.onOffDdlNoiLamViec(roleId);
    },

    registerEvent: function () {
        $('.ddlRole').off('change').on('change', function () {
            var roleId = $(this).val();
            editController.loadKVByRole(roleId);
            editController.onOffDdlNoiLamViec(roleId);
            
        });
        
        $('.ddlKV').off('change').on('change', function () {
            var optionValue = $(this).val();
            //$('#hidMaTD').val(optionValue);
            editController.loadMaNV(optionValue);
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
    loadKVByRole: function (optionValue) {
        $('.ddlKV').html('');
        var option = '';

        $.ajax({
            url: '/Accounts/GetKVByRole',
            type: 'GET',
            data: {
                roleName: optionValue
            },
            dataType: 'json',
            success: function (response) {
                console.log(response);
                var data = JSON.parse(response.data);

                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.Id + '">' + item.Name + '</option>'; //chinhanh1

                });
                $('.ddlKV').html(option);
                //// load MaNV again
                var optionValue = $('.ddlKV').val();
                editController.loadMaNV(optionValue);
            }
        });
    },
    loadMaNV: function (optionValue) {
        $.ajax({
            url: '/Accounts/GetNextMaNV',
            type: 'GET',
            data: {
                idKV: optionValue
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    $('.txtMaNV').val(response.data);
                }
            }
        });
    },

    onOffDdlNoiLamViec: function (optionValue) {
        if (optionValue !== "Users") {
            
            $('.ddlNoiLamViec').prop('disabled', true);
        }
        else {
            $('.ddlNoiLamViec').prop('disabled', false);
        }
    }
};
editController.init();