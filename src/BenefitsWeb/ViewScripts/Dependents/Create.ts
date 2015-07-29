/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/common.ts" />

//module responsible for creating a dependent
angular.module("createModule", ["servicesModule", "directivesModule"])
    .controller("createController", ["$scope", "$window", "dependentsService", "employeeId",
        function ($scope, $window: ng.IWindowService, dependentsService: IDependentService, employeeId: string) {

            $scope.name = "";
            
            //creates the dependent
            $scope.saveChanges = function () {

                //force all fields to validate
                $scope.$broadcast("show-errors-event");

                if ($scope.dependentForm.$invalid) {
                    $window.alert("You must correct all errors before saving changes.");
                    return;
                }
                
                //show the progress indicator
                $("#progressModal").modal("toggle");

                var dependent = new DependentViewModel();

                dependent.employeeId = employeeId;
                dependent.name = $scope.name;

                //post the new dependents info to the service
                dependentsService.post(dependent)
                .then(
                    function (data) {
                        $window.location.href = "/Employees/Details/" + employeeId;
                    },
                    function (errMsg) {
                        alert(errMsg);
                    })
                .then(
                    function () {
                        $("#progressModal").modal("hide");
                    });
            }

            //cancels the edit
            $scope.cancel = function () {

                $window.location.href = "/Employees/Details/" + employeeId;
            }

        }]);