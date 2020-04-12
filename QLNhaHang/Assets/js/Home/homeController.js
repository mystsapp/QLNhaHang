function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function addCommas1(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

var app = angular.module("app", ["chart.js"]);

//app.config(['ChartJsProvider', function (ChartJsProvider) {
//    // Configure all charts
//    ChartJsProvider.setOptions({
//        //colours: ['#72C02C', '#3498DB', '#717984', '#F1C40F'],
//        colours: [
//            { // blue
//                fillColor: "rgba(62, 92, 154,1)",
//                strokeColor: "rgba(151,200,205,1)",
//                pointColor: "rgba(151,187,205,1)",
//                pointStrokeColor: "#fff",
//                pointHighlightFill: "#fff",
//                pointHighlightStroke: "rgba(151,187,205,0.8)"
//            },
//            { // dark grey
//                fillColor: "rgba(255, 242, 0,1)",
//                strokeColor: "rgba(255,200,96,1)",
//                pointColor: "rgba(77,83,96,1)",
//                pointStrokeColor: "#fff",
//                pointHighlightFill: "#fff",
//                pointHighlightStroke: "rgba(77,83,96,1)"
//            }
//        ],
//        responsive: true
//    });
//    // Configure all line charts
//    //ChartJsProvider.setOptions('Bar', {
//    //    datasetFill: false
//    //});

//}])

app.controller("PieCtrl", PieCtrl);

PieCtrl.$inject = ['$scope', '$http'];

function PieCtrl($scope, $http) {
    var vm = this;

    $scope.labels = ["Download Sales", "In-Store Sales", "Mail-Order Sales"];
    $scope.data = [300, 500, 100];    

    //Make sure to use color codes, instead of color name.
    //$scope.colorsPie = ['#90EE90', '#FF6600', '#8080FF'];
    //PieDataSetOverride is used to draw lines to display the labels

    $scope.PieDataSetOverride = [{ yAxisID: 'y-axis-1' }]; //y-axis-1 is the ID defined in scales under options.

    $scope.optionsPie = {
        legend: { display: true },
        responsive: true,  // set to false to remove responsiveness. Default responsive value is true.
        scales: {
            yAxes: [
                {
                    id: 'y-axis-1',
                    type: 'linear',
                    display: true,
                    position: 'left'
                }]
        }
    };

    //$scope.clickme = function ($event) {
    //    alert("You've clicked upon " + $event[0]._view.label);
    //};

    //$scope.hoverme = function ($event) {
    //    alert("You hovered over " + $event[0]._view.label);
    //};    
    ///////////
    $http({
        url: '/Home/ListSevenDay',
        type: 'GET'
        //data: {
        //    tungay: "01/01/2016",
        //    denngay: "01/01/2019",
        //    chinhanh: "STS",
        //    khoi: "OB"
        //}
    }).then(function successCallback(response) {
        // this callback will be called asynchronously
        // when the response is available
        
        var labels = [];
        var chartData = [];
        //var sokhachht = [];
        //var sokhachtt = [];

        //var data = JSON.parse(response.data);
        var ajaxdata = response.data;
        var tableData = ajaxdata.data;
       // console.log(tableData);
        toString();
        $.each(tableData, function (i, item) {
            labels.push(item.NgayBan);
            //console.log(item.NgayBan);
            //sokhachht.push(item.soKhachHT);
            //sokhachtt.push(item.soKhachTT);

            chartData.push(item.TongTien);
        });

        //chartData.push(sokhachht);
        //chartData.push(sokhachtt);
        
        $scope.data = chartData;
        $scope.labels = labels;
        //console.log($scope.data);
        //console.log($scope.labels);
        //$scope.tableData = tableData;

    }, function errorCallback(response) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
        $scope.error = response.statusText;
    });

}
