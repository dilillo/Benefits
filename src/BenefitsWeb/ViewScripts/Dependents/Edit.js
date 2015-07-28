angular.module("editModule", ["servicesModule", "directivesModule"])
    .controller("editController", ["$scope", "$window", "$location", "dependentsService", "dependentId",
        function ($scope, $window, $location, dependentsService, dependentId) {

            $scope.id = dependentId;
            $scope.name = "";
            $scope.employeeId = "";
            $scope.employeeVersion = 0;

            $scope.refresh = function () {

                $("#progressModal").modal("toggle");

                dependentsService.getById($scope.id)
                .then(
                    function (data) {
                        $scope.employeeId = data.employeeId;
                        $scope.employeeVersion = data.employeeVersion;
                        $scope.name = data.name;
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            $scope.saveChanges = function () {

                $scope.$broadcast("show-errors-event");

                if ($scope.dependentForm.$invalid) {
                    $window.alert("You must correct all errors before saving changes.");
                    return;
                }

                $("#progressModal").modal("toggle");

                dependentsService.put({
                    id: $scope.id,
                    employeeId: $scope.employeeId,
                    employeeVersion: $scope.employeeVersion,
                    name: $scope.name
                })
                .then(
                    function (data) {
                        $window.location("/Employees/Details/" + $scope.employeeId);
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            $scope.cancel = function () {

                if ($scope.dependentForm.$dirty) {

                    var ignoreChanges = $window.confirm("Are you sure want to leave this page and lose your changes?");

                    if (!ignoreChanges)
                        return;
                }

                $window.location("/Employees/Details/" + $scope.employeeId);
            }

            $scope.refresh();

        }]);