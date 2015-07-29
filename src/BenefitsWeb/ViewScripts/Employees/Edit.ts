/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/common.ts" />

//module responsible for editing an employee
angular.module("editModule", ["servicesModule", "directivesModule"])
    .controller("editController", ["$scope", "$window", "employeesService", "employeeId",
        function ($scope, $window: ng.IWindowService, employeesService: IEmployeeService, employeeId: string) {

            $scope.id = employeeId;
            $scope.name = "";
            $scope.version = 0;

            //loads up the screen
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

            //pushes the edits back to the server
            $scope.saveChanges = function () {

                //give all fields a chance to validate
                $scope.$broadcast("show-errors-event");

                if ($scope.employeeForm.$invalid) {
                    $window.alert("You must correct all errors before saving changes.");
                    return;
                }

                //show the progress model
                $("#progressModal").modal("toggle");

                var employee = new EmployeeDetailViewModel();

                employee.id = $scope.id,
                employee.version = $scope.version;
                employee.name = $scope.name;

                //send the updates to the server
                employeesService.put(employee)
                .then(
                    function (data) {
                        $window.location.href = "/Employees/Details/" + $scope.id;
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            //cancel the edit
            $scope.cancel = function () {

                if ($scope.employeeForm.$dirty) {

                    var ignoreChanges = $window.confirm("Are you sure want to leave this page and lose your changes?");

                    if (!ignoreChanges)
                        return;
                }

                $window.location.href = "/Employees/Details/" + $scope.id;
            }

            $scope.refresh();

        }]);