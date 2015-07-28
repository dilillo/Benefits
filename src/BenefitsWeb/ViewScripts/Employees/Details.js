
angular.module("detailsModule", ["servicesModule"])
    .controller("detailsController", ["$scope", "$window", "$location", "employeesService", "dependentsService", "employeeId",
        function ($scope, $window, $location, employeesService, dependentsService, employeeId) {

            $scope.id = employeeId;
            $scope.name = "";
            $scope.version = 1;
            $scope.grossPay = 0.0;
            $scope.benefits = 0.0;
            $scope.netPay = 0.0;
            $scope.dependents = [];

            $scope.refresh = function () {

                $("#progressModal").modal("toggle");

                employeesService.getById($scope.id)
                .then(
                    function (data) {
                        $scope.id = data.id;
                        $scope.version = data.version
                        $scope.name = data.name;
                        $scope.grossPay = data.grossPay;
                        $scope.benefits = data.benefits;
                        $scope.netPay = data.netPay;

                        angular.copy(data.dependents, $scope.dependents);
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            $scope.delete = function () {

                var confirmed = $window.confirm("Are you sure want to delete this employee?");

                if (!confirmed)
                    return;

                $("#progressModal").modal("toggle");

                employeesService.delete($scope.id, $scope.version)
                .then(
                    function (data) {
                        $window.location("/Employees");
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            $scope.deleteDependent = function (dependent) {

                var confirmed = $window.confirm("Are you sure want to delete this dependent?");

                if (!confirmed)
                    return;

                $("#progressModal").modal("toggle");

                dependentsService.delete(dependent.id, $scope.version)
                .then(
                    function (data) {

                        var index = $scope.dependents.indexOf(dependent);

                        $scope.dependents.splice(dependent, 1);
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            $scope.refresh();

        }]);