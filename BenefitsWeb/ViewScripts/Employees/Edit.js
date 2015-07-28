
angular.module("editModule", ["servicesModule", "directivesModule"])
    .controller("editController", ["$scope", "$window", "$location", "employeesService", "employeeId",
        function ($scope, $window, $location, employeesService, employeeId) {

            $scope.id = employeeId;
            $scope.name = "";
            $scope.version = 0;

            $scope.refresh = function () {

                $("#progressModal").modal("toggle");

                employeesService.getById($scope.id)
                .then(
                    function (data) {
                        $scope.id = data.id;
                        $scope.version = data.version;
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

                if ($scope.employeeForm.$invalid) {
                    $window.alert("You must correct all errors before saving changes.");
                    return;
                }

                $("#progressModal").modal("toggle");

                employeesService.put({
                    id: $scope.id,
                    version: $scope.version,
                    name: $scope.name
                })
                .then(
                    function (data) {
                        $window.location("/Employees/Details/" + $scope.id);
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

                if ($scope.employeeForm.$dirty) {

                    var ignoreChanges = $window.confirm("Are you sure want to leave this page and lose your changes?");

                    if (!ignoreChanges)
                        return;
                }

                $window.location("/Employees/Details/" + $scope.id);
            }

            $scope.refresh();

        }]);