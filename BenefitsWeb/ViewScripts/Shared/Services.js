angular.module("servicesModule", [])
    .factory("employeesService", ["$http", "$q", function ($http, $q) {
        return {
            get: function () {

                var deferred = $q.defer();

                $http.get("/Api/EmployeesApi")
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            getById: function (id) {

                var deferred = $q.defer();

                $http.get("/Api/EmployeesApi/" + id)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            post: function (employee) {

                var deferred = $q.defer();

                $http.post("/Api/EmployeesApi", employee)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            put: function (employee) {

                var deferred = $q.defer();

                $http.put("/Api/EmployeesApi/" + employee.id, employee)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            delete: function (id) {

                var deferred = $q.defer();

                $http.delete("/Api/EmployeesApi/" + id)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            }
        }
    }])
    .factory("dependentsService", ["$http", "$q", function ($http, $q) {
        return {
            getById: function (id) {

                var deferred = $q.defer();

                $http.get("/Api/DependentsApi/" + id)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            post: function (dependent) {

                var deferred = $q.defer();

                $http.post("/Api/DependentsApi", dependent)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            put: function (dependent) {

                var deferred = $q.defer();

                $http.put("/Api/DependentsApi/" + dependent.id, dependent)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            },
            delete: function (id, version) {

                var deferred = $q.defer();

                $http.delete("/Api/DependentsApi/" + id + "?version=" + version)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (data) {
                    deferred.reject(data);
                });

                return deferred.promise;
            }
        }
    }]);