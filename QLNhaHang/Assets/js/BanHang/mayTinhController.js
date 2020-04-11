function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var mayTinhController = {
    init: function () {
        
        mayTinhController.registerEvent();
    },
    registerEvent: function () {
        
        $('#txtKhachDua').off('blur').on('blur', function () {
            var tongTien = $('#txtTongTien').val();
            tongTien = tongTien.replace(/[$,]+/g, "");
            var khachDua = $(this).val();
            khachDua = khachDua.replace(/[$,]+/g, "");
            if (khachDua < tongTien) {
                alert('Khách đưa phải lớn hơn tổng tiền');
            }
            else {
                var tienThua = khachDua - tongTien;
            }
            
            //var tienThua = tongTien - khachDua;
            $('#txtTienThua').val(tienThua);
        });   
        shortcut.add("Enter", function () {
            var tongTien = $('#txtTongTien').val();
            tongTien = tongTien.replace(/[$,]+/g, "");
            var khachDua = $('#txtKhachDua').val();
            khachDua = khachDua.replace(/[$,]+/g, "");
            if (khachDua < tongTien) {
                alert('Khách đưa phải lớn hơn tổng tiền');
            }
            else {
                var tienThua = khachDua - tongTien;
            }

            //var tienThua = tongTien - khachDua;
            $('#txtTienThua').val(addCommas(tienThua));
        });
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

     

    }

};
mayTinhController.init();