angular.module("indexModule", ["servicesModule"])
    .controller("indexController", ["$scope", "employeesService",
        function ($scope, employeesService) {

            $scope.employees = [];
            
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