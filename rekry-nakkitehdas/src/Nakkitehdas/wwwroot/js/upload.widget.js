var widget = angular.module("UploadWidget", ["UploadApi"]);

widget.directive("fileUpload", function () {
    return {
        restrict: "A",
        controller: ["$scope", "uploadApi", function ($scope, uploadApi) {
            this.uploadApi = uploadApi;
        }],
        link: function (scope, element, attrs, ctrl) {
            element.on("drop dragdrop", function (e) {
                e.preventDefault();

                /*
                 * REVIEW QUESTION: What problem this solves and why? How would the widget behave 
                 *                  without these two lines?
                 */
                if (!isLeaf(e.target))
                    return;

                var loadingVotes = e.dataTransfer.files.length;
                var results = [];

                for (var i = 0; i < e.dataTransfer.files.length; ++i) {
                    /*
                     * REVIEW QUESTION: Why is this done like this? Why to wrap everything in an
                     *                  anonymous function and call it, instead of just by doing this:
                     *
                     *                      var file = e.dataTransfer.files[i];
                     *                      var reader = new FileReader();
                     *
                     *                  without the anonymous function altogether?
                     */
                    (function (file, reader) {
                        reader.readAsArrayBuffer(file);

                        reader.onloadend = function () {
                            results.push({ file: file.name, contents: reader.result });

                            if (loadingVotes == results.length)
                                ctrl.uploadApi.uploadMany(scope.$eval(attrs.fileUpload) || [], results).then(function(files) {
                                    for (var j = 0; files && j < files.length; ++j)
                                        scope.$evalAsync(attrs.onFileUploaded, { file: files[j] });
                                });
                        };
                    }(e.dataTransfer.files[i], new FileReader()));
                }
            });

            element.on("dragenter", function (e) {
                e.preventDefault();
            });

            element.on("dragover", function (e) {
                e.preventDefault();
            });

            function isLeaf(el) {
                el = angular.element(el);

                if (!el.length || el[0] == element[0])
                    return true;

                if (el.attr("file-upload") !== undefined || el.attr("data-file-upload") !== undefined)
                    return false;

                return isLeaf(el.parent());
            }
        }
    };
});
