﻿var indexController = {
    init: function () {
        indexController.registerEvent();
    },

    registerEvent: function () {

        $('.ddlKV').off('change').on('change', function () {
            var optionValue = $(this).val();
            $('#hidDdlKV').val(optionValue);
            $('#btnSubmit').click();
        });

        //$('.tdVal').click(function () {
        //    id = $(this).data('id');
        //    $('#hidMaBan').val(id);
        //    //var page = $('.active .page-link').text();
        //    var page = $('.active a').text();
        //    $('#hidPage').val(page);
        //    //$.ajax({
        //    //    url: '/CapThes/Index',
        //    //    data: {
        //    //        maCT: id
        //    //    },
        //    //    dataType: 'json',
        //    //    type: 'GET',
        //    //    success: function (response) {

        //    //    }
        //    //});

        //    $('#btnSubmit').click();
        //});

        //$('#btnExportAll').off('click').on('click', function () {

            //indexController.exportList();

        //});

    
    }
};
indexController.init();