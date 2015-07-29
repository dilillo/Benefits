/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />

//represents a dependent in the ui
class DependentViewModel {

    id: string;

    employeeId: string;

    employeeVersion: number;

    name: string;
}

//represents the summary view of the employee in the ui
class EmployeeSummaryViewModel {

    id: string;

    name: string;
}

//represents the detail view of the employee in the ui
class EmployeeDetailViewModel {

    id: string;

    version: number;

    name: string;

    grossPay: number;

    benefits: number;

    netPay: number;

    dependents: Array<DependentViewModel>;

    constructor() {
        this.dependents = [];
    }
}

//defines a service for geting and setting employee data
interface IEmployeeService {

    get(): ng.IPromise<Array<EmployeeSummaryViewModel>>;

    getById(id: string): ng.IPromise<EmployeeDetailViewModel>;

    post(employee: EmployeeDetailViewModel): ng.IPromise<EmployeeDetailViewModel>;

    put(employee: EmployeeDetailViewModel): ng.IPromise<any>

    delete(id: string): ng.IPromise<any>
}

//defines a service for getting and setting dependent data
interface IDependentService {

    getById(id: string): ng.IPromise<DependentViewModel>;

    post(employee: DependentViewModel): ng.IPromise<DependentViewModel>;

    put(employee: DependentViewModel): ng.IPromise<any>

    delete(id: string, version: number): ng.IPromise<any>
}