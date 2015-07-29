/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
//represents a dependent in the ui
var DependentViewModel = (function () {
    function DependentViewModel() {
    }
    return DependentViewModel;
})();
//represents the summary view of the employee in the ui
var EmployeeSummaryViewModel = (function () {
    function EmployeeSummaryViewModel() {
    }
    return EmployeeSummaryViewModel;
})();
//represents the detail view of the employee in the ui
var EmployeeDetailViewModel = (function () {
    function EmployeeDetailViewModel() {
        this.dependents = [];
    }
    return EmployeeDetailViewModel;
})();
//# sourceMappingURL=common.js.map