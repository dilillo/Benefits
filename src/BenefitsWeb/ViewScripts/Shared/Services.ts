/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="common.ts" />

//module for commonly used services
angular.module("servicesModule", [])
    .factory("employeesService", ["$http", "$q", function ($http: ng.IHttpService, $q: ng.IQService) {
        return {
            get: function() : ng.IPromise<Array<EmployeeSummaryViewModel>> {

                var deferred = $q.defer<Array<EmployeeSummaryViewModel>>();

                $http.get("/Api/EmployeesApi")
                    .success(function (data: Array<EmployeeSummaryViewModel>) {
                        deferred.resolve(data);
                    })
                    .error(function (data: string) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            getById: function (id: string): ng.IPromise<EmployeeDetailViewModel> {

                var deferred = $q.defer<EmployeeDetailViewModel>();

                $http.get("/Api/EmployeesApi/" + id)
                    .success(function (data: EmployeeDetailViewModel) {
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            post: function (employee: EmployeeDetailViewModel): ng.IPromise<EmployeeDetailViewModel> {

                var deferred = $q.defer<EmployeeDetailViewModel>();

                $http.post("/Api/EmployeesApi", employee)
                    .success(function (data: EmployeeDetailViewModel) {
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            put: function (employee: EmployeeDetailViewModel): ng.IPromise<any> {

                var deferred = $q.defer<any>();

                $http.put("/Api/EmployeesApi/" + employee.id, employee)
                    .success(function () {
                        deferred.resolve();
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            delete: function (id: string): ng.IPromise<any> {

                var deferred = $q.defer();

                $http.delete("/Api/EmployeesApi/" + id)
                    .success(function () {
                        deferred.resolve();
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        }
    }])
    .factory("dependentsService", ["$http", "$q", function ($http: ng.IHttpService, $q: ng.IQService) {
        return {
            getById: function (id: string): ng.IPromise<DependentViewModel> {

                var deferred = $q.defer<DependentViewModel>();

                $http.get("/Api/DependentsApi/" + id)
                    .success(function (data: DependentViewModel) {
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            post: function (dependent: DependentViewModel): ng.IPromise<DependentViewModel> {

                var deferred = $q.defer<DependentViewModel>();

                $http.post("/Api/DependentsApi", dependent)
                    .success(function (data: DependentViewModel) {
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            put: function (dependent: DependentViewModel): ng.IPromise<any> {

                var deferred = $q.defer();

                $http.put("/Api/DependentsApi/" + dependent.id, dependent)
                    .success(function () {
                        deferred.resolve();
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            },
            delete: function (id: string, version: number): ng.IPromise<any> {

                var deferred = $q.defer();

                $http.delete("/Api/DependentsApi/" + id + "?version=" + version)
                    .success(function () {
                        deferred.resolve();
                    })
                    .error(function (data) {
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        }
    }]);