var api = angular.module("DirectoryApi", []);

//http://stackoverflow.com/questions/32803142/json-object-as-angularjs-factory

//api.controller('dirController', ['$scope', 'directoryApi', function ($scope, directoryApi) {
//    $scope.path = "";


//    $scope.setpath = function (pathname) {
//        $scope.path = pathname;
//    }

//}]);

api.factory("directoryApi", ["$http", function ($http) {
    return {

        getItems: function (path) {
            apiroute = '/api/directory/GetItems';

            if (path != null && path != undefined) {
                apiroute += path;
            }
            var items = [];
            items = $http.get(apiroute).then(
                function successCallback(result) {

                return result.data;
            },
        function errorCallback() {
        });

            return items;

        },
        setPath: function (pathname) {
            return pathname;
        }

    };
}]);


