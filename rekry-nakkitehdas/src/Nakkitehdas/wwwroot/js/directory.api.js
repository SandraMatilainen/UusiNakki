var api = angular.module("DirectoryApi", []);

//http://stackoverflow.com/questions/32803142/json-object-as-angularjs-factory

api.factory("directoryApi", ["$http", function ($http) {
    return {
        getItems: function (path) {

            return $http.get('/api/directory/GetItems');
        }
    };
}]);
