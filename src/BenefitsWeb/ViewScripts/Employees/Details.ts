/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/common.ts" />

// module responsible for displaying the details of an employee and performing deletes as necessary
angular.module("detailsModule", ["servicesModule"])
    .controller("detailsController", ["$scope", "$window", "employeesService", "dependentsService", "employeeId",
        function ($scope, $window: ng.IWindowService, employeesService: IEmployeeService, dependentsService: IDependentService, employeeId: string) {

            $scope.id = employeeId;
            $scope.name = "";
            $scope.version = 1;
            $scope.grossPay = 0.0;
            $scope.benefits = 0.0;
            $scope.netPay = 0.0;
            $scope.dependents = [];

            //loads up the screen
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

            //deletes the employee
            $scope.delete = function () {

                var confirmed = $window.confirm("Are you sure want to delete this employee?");

                if (!confirmed)
                    return;

                $("#progressModal").modal("toggle");

                //tell the service to delete the employee
                employeesService.delete($scope.id)
                .then(
                    function (data) {
                        $window.location.href = "/Employees";
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            //deletes a dependent
            $scope.deleteDependent = function (dependent) {

                var confirmed = $window.confirm("Are you sure want to delete this dependent?");

                if (!confirmed)
                    return;

                $("#progressModal").modal("toggle");

                //tell the service to delete the dependent
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