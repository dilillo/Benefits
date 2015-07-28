angular.module("createModule", ["servicesModule", "directivesModule"])
    .controller("createController", ["$scope", "$window", "$location", "employeesService",
        function ($scope, $window, $location, employeesService) {

            $scope.name = "";

            $scope.saveChanges = function () {

                $scope.$broadcast("show-errors-event");

                if ($scope.employeeForm.$invalid) {
                    $window.alert("You must correct all errors before saving changes.");
                    return;
                }
                
                $("#progressModal").modal("toggle");

                employeesService.post({
                    name: $scope.name
                })
                .then(
                    function (data) {
                        $window.location("/Employees/Details/" + data.id);
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

                $window.location("/Employees");
            }

        }]);