/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/common.ts" />
//module responsible for editing a dependent
angular.module("editModule", ["servicesModule", "directivesModule"])
    .controller("editController", ["$scope", "$window", "dependentsService", "dependentId",
    function ($scope, $window, dependentsService, dependentId) {
        $scope.id = dependentId;
        $scope.name = "";
        $scope.employeeId = "";
        $scope.employeeVersion = 0;
        //loads up the data
        $scope.refresh = function () {
            $("#progressModal").modal("toggle");
            dependentsService.getById($scope.id)
                .then(function (data) {
                $scope.employeeId = data.employeeId;
                $scope.employeeVersion = data.employeeVersion;
                $scope.name = data.name;
            }, function (errMsg) {
                alert(errMsg);
            })
                .then(function () {
                $("#progressModal").modal("hide");
            });
        };
        //sends the data back to the server
        $scope.saveChanges = function () {
            //give all fields a chance to validate
            $scope.$broadcast("show-errors-event");
            if ($scope.dependentForm.$invalid) {
                $window.alert("You must correct all errors before saving changes.");
                return;
            }
            //show the progress display
            $("#progressModal").modal("toggle");
            var dependent = new DependentViewModel();
            dependent.id = $scope.id;
            dependent.employeeId = $scope.employeeId;
            dependent.employeeVersion = $scope.employeeVersion;
            dependent.name = $scope.name;
            //call the dependents service with the dependent to modify
            dependentsService.put(dependent)
                .then(function (data) {
                $window.location.href = "/Employees/Details/" + $scope.employeeId;
            }, function (errMsg) {
                alert(errMsg);
            })
                .then(function () {
                $("#progressModal").modal("hide");
            });
        };
        //cancels the edit
        $scope.cancel = function () {
            if ($scope.dependentForm.$dirty) {
                var ignoreChanges = $window.confirm("Are you sure want to leave this page and lose your changes?");
                if (!ignoreChanges)
                    return;
            }
            $window.location.href = "/Employees/Details/" + $scope.employeeId;
        };
        $scope.refresh();
    }]);
//# sourceMappingURL=Edit.js.map