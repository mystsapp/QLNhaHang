
//returns a Date() object in dd/MM/yyyy
$.formattedDate = function (dateToFormat) {
    var dateObject = new Date(dateToFormat);
    var day = dateObject.getDate();
    var month = dateObject.getMonth() + 1;
    var year = dateObject.getFullYear();
    day = day < 10 ? "0" + day : day;
    month = month < 10 ? "0" + month : month;
    var formattedDate = day + "/" + month + "/" + year;
    return formattedDate;
};


$.stringToDate = function (_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

var indexController = {
    init: function () {
        indexController.LoadData();
        setInterval(function () { indexController.LoadData(); }, 10000);
    },

    registerEvent: function () {
        $(".cursor-pointer").each(function () {

            var daLam = $(this).data('id');

            if (daLam === true) {
                $(this).css("background-color", "#6c757d").css("color", "white");
                //$(this).addClass("bg-info");

            }

        });
        $('#btnPrintAll').off('click').on('click', function () {
            indexController.printList();
        });

        $('.btnConfirm').off('click').on('click', function () {
            //alert($(this).data('id'));
            
            if ($(this).data('id')) {
                return;
            }
            else {
                $('.class1').val($(this).data('abc'));
                $('.frmDaLam').submit();
            }
        });
        //$('.modal-dialog').draggable();

        //$("#txtNgaySinh, #txtNgayCMND, #txtHanTheHDV, #txtHieuLucHoChieu, #txtHanVisa").datepicker({
        //    changeMonth: true,
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy"

        //});

    },

    LoadData: function () {

        $.ajax({
            url: '/PhaChes/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log(response.data);
                if (response.status) {
                    //console.log(response.data);
                    var data = response.data;
                    //var data = JSON.parse(response.data);

                    //alert(data);
                    var html = '';
                    var template = $('#data-template').html();
                    var stt = 1;
                    for (var i = 0; i < data.length; i++) {
                        html += Mustache.render(template, {
                            sTT: stt,
                            idMon: data[i].Id,
                            tenBan: data[i].Ban.TenBan,
                            tenMon: data[i].ThucDon.TenMon,
                            giaTien: numeral(data[i].GiaTien).format('0,0'),
                            soLuong: data[i].SoLuong,
                            thanhTien: numeral(data[i].ThanhTien).format('0,0'),
                            lanGui: data[i].LanGui,
                            daGui: data[i].DaGui === true ? "<span class=\"badge badge-success\">Đã gửi</span>" : "<span class=\"badge badge-danger\">Chưa gửi</span>",
                            daLam: data[i].DaLam === true ? "<span class=\"badge badge-success\">Đã làm</span>" : "<span class=\"badge badge-danger\">Chưa làm</span>",
                            lamRoi: data[i].DaLam
                        });
                        stt = stt + 1;

                    }

                    //$.each(data, function (i, item) {
                    //    var stt = 1;
                    //    //var nt = "";
                    //    //if (item.ngaytao === null)
                    //    //    nt = "";
                    //    //else
                    //    //    nt = $.formattedDate(new Date(parseInt(item.ngaytao.substr(6)))); badge badge-success

                    //    html += Mustache.render(template, {
                    //        sTT: stt,
                    //        tenBan: item.Ban.TenBan,
                    //        tenMon: item.ThucDon.TenMon,
                    //        giaTien: item.GiaTien,
                    //        soLuong: item.SoLuong,
                    //        thanhTien: item.ThanhTien,
                    //        lanGui: item.LanGui,
                    //        daGui: item.DaGui === true ? "<span class=\"badge badge-success\">Đã gửi</span>" : "<span class=\"badge badge-danger\">Chưa gửi</span>",
                    //        daLam: item.DaLam === true ? "<span class=\"badge badge-success\">Đã làm</span>" : "<span class=\"badge badge-danger\">Chưa làm</span>",
                    //    });
                    //    stt = stt + 1;
                    //});

                    $('#tblData').html(html);



                    indexController.registerEvent();
                }
            }
        });
    },
    printList: function () {
        var idList = [];
        $.each($('.ckId'), function (i, item) {
            if ($(this).prop('checked')) {
                idList.push({
                    Id: $(item).data('id')
                });
            }

        });

        if (idList.length !== 0) {
            $('#stringId').val(JSON.stringify(idList));
            $('#frmPrintListId').submit();
        }
        else {
            bootbox.alert({
                size: "small",
                title: "Information",
                message: "Bạn chưa chọn dòng nào!",
                callback: function () {
                    //e.preventDefault();

                }
            });
        }
    },

};
indexController.init();