var app = angular.module("Nakkitehdas", ["FolderWidget"]);

//service here

app.controller('MainCtrl', function ($scope, directoryApi) {

    directoryApi.getItems().success(function (data) {
        $scope.items = data;
    });
});