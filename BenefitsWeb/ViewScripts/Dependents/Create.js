angular.module("createModule", ["servicesModule", "directivesModule"])
    .controller("createController", ["$scope", "$window", "$location", "dependentsService", "employeeId",
        function ($scope, $window, $location, dependentsService, employeeId) {

            $scope.name = "";
            
            $scope.saveChanges = function () {

                $scope.$broadcast("show-errors-event");

                if ($scope.dependentForm.$invalid) {
                    $window.alert("You must correct all errors before saving changes.");
                    return;
                }
                
                $("#progressModal").modal("toggle");

                dependentsService.post({
                    employeeId: employeeId,
                    name: $scope.name
                })
                .then(
                    function (data) {
                        $window.location("/Employees/Details/" + employeeId);
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

                $window.location("/Employees/Details/" + employeeId);
            }

        }]);