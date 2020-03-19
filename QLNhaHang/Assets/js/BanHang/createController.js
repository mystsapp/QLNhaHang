function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var createController = {
    init: function () {

        createController.registerEvent();
    },
    registerEvent: function () {

        $('.ddlChiNhanh').off('change').on('change', function () {
            var optionValue = $('.ddlChiNhanh').val();
            $('#hidMaCN').val(optionValue);
            $('#hidSubmit').click();
        });

        var seriPrefix = '';
        var seriTuOld = '';
        var seriTu = '';
        // seri tu
        $('#txtSeriTu').off('blur').on('blur', function () {
            var currentYear = $('#hidCurrentYear').val();
            var noiNhan = $('#ddlNoiNhan').val();
            var loaiThe = $('#ddlLoaiThe').val();
            seriTu = $(this).val();
            seriTuOld = seriTu;
            seriPrefix = loaiThe + noiNhan + currentYear;

            switch (seriTu.length) {
                case 1:
                    seriTu = seriPrefix + '00000' + seriTu;
                    break;
                case 2:
                    seriTu = seriPrefix + '0000' + seriTu;
                    break;
                case 3:
                    seriTu = seriPrefix + '000' + seriTu;
                    break;
                case 4:
                    seriTu = seriPrefix + '00' + seriTu;
                    break;
                case 5:
                    seriTu = seriPrefix + '0' + seriTu;
                    break;
                case 6:
                    seriTu = seriPrefix + seriTu;
                    break;
                default:
                // code block
            }

            $('#txtSeriTu').val(seriTu);

        });

        // seri tu
        var seriDen = '';
        $('#txtSeriDen').off('blur').on('blur', function () {
            var seriDenOld = $(this).val();
            if (parseInt(seriTuOld) > parseInt(seriDenOld)) {
                bootbox.alert({
                    size: "small",
                    title: "Information",
                    message: "<b>Seri từ</b> không được lớn hơn <b>Seri đến</b> !",
                    callback: function () {
                        //e.preventDefault();
                    }
                });
            }
            else {               

                switch (seriDenOld.length) {
                    case 1:
                        seriDen = seriPrefix + '00000' + seriDenOld;
                        break;
                    case 2:
                        seriDen = seriPrefix + '0000' + seriDenOld;
                        break;
                    case 3:
                        seriDen = seriPrefix + '000' + seriDenOld;
                        break;
                    case 4:
                        seriDen = seriPrefix + '00' + seriDenOld;
                        break;
                    case 5:
                        seriDen = seriPrefix + '0' + seriDenOld;
                        break;
                    case 6:
                        seriDen = seriPrefix + seriDenOld;
                        break;
                    default:
                    // code block
                }
                $('#txtSeriDen').val(seriDen);

                // soluong
                var soLuong = parseInt(seriDen) - parseInt(seriTu) + 1;

                $('#txtSoLuong').val(soLuong);
            }

        });

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
createController.init();