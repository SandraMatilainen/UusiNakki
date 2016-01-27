var widget = angular.module("FolderWidget", ["DirectoryApi"]);

widget.directive("folderStructure", function () {
    return {
        restirct: "E",
        transclude: true,
        scope: true,
        controller: ["$scope", "directoryApi", function ($scope, directoryApi) {
            directoryApi.getItems($scope.path).then(function(items) {
                $scope.items = items;
            });
        }],
        link: function (scope, element, attrs, ctrl, transclude) {
            transclude(scope.$new(), function (clone) {
                var ul = angular.element("<ul>");

                ul.addClass("folder-structure");

                ul.append(clone);
                element.append(ul);
            });
        }
    }
});

widget.directive("subFolderStructure", ["$compile", "$templateRequest", function ($compile, $templateRequest) {
    return {
        restirct: "E",
        require: "^folderStructure",
        link: function (scope, element, attrs) {
            if (!attrs.parentId)
                throw new Error("'parent-id' attribute is missing");

            if (!attrs.template)
                throw new Error("'template' attribute is missing");

            var id = scope.$eval(attrs.parentId);

            $templateRequest(attrs.template).then(function (html) {
                var template = angular.element(html);
                var childScope = scope.$new(true);

                childScope.path = angular.copy(scope.path || []);
                childScope.path.push(id);

                element.append(template);
                $compile(template)(childScope);
            });
        }
    };
}]);

widget.directive("folder", function () {
    return {
        restirct: "E",
        require: "^folderStructure",
        transclude: true,
        scope: {},
        link: function (scope, element, attrs, ctrl, transclude) {
            transclude(function (clone) {
                var li = angular.element("<li>");

                li.addClass("folder");

                li.append(clone);
                element.append(li);
            });
        }
    };
});

widget.directive("file", function () {
    return {
        restirct: "E",
        require: "^folderStructure",
        transclude: true,
        link: function (scope, element, attrs, ctrl, transclude) {
            transclude(function (clone) {
                var li = angular.element("<li>");

                li.addClass("file");

                li.append(clone);
                element.append(li);
            });
        }
    };
});
