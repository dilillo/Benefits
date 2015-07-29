/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../shared/common.ts" />

//module responsible for displaying employees
angular.module("indexModule", ["servicesModule"])
    .controller("indexController", ["$scope", "employeesService",
        function ($scope, employeesService: IEmployeeService) {

            $scope.employees = [];
            
            //load up the data
            $scope.refresh = function () {

                $("#progressModal").modal("toggle");

                employeesService.get()
                .then(
                    function (data) {
                        angular.copy(data, $scope.employees);
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