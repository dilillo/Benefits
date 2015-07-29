/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/common.ts" />
//module responsible for creating an employee
angular.module("createModule", ["servicesModule", "directivesModule"])
    .controller("createController", ["$scope", "$window", "employeesService",
    function ($scope, $window, employeesService) {
        $scope.name = "";
        //saves the new employee
        $scope.saveChanges = function () {
            //give all fields a chance to validate
            $scope.$broadcast("show-errors-event");
            if ($scope.employeeForm.$invalid) {
                $window.alert("You must correct all errors before saving changes.");
                return;
            }
            //show the progress display
            $("#progressModal").modal("toggle");
            var employee = new EmployeeDetailViewModel();
            employee.name = $scope.name;
            //send the new employees info to the server
            employeesService.post(employee)
                .then(function (data) {
                $window.location.href = "/Employees/Details/" + data.id;
            }, function (errMsg) {
                alert(errMsg);
            })
                .then(function () {
                $("#progressModal").modal("hide");
            });
        };
        //cancel the edit
        $scope.cancel = function () {
            $window.location.href = "/Employees";
        };
    }]);
//# sourceMappingURL=Create.js.map