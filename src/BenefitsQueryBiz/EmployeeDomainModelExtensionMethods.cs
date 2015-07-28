using Benefits.Infrastructure.Models;
using Benefits.QueryData;

namespace Benefits.QueryBiz
{
    /// <summary>
    /// Container for extension methods used to covert between domain and read side versions of employee model.
    /// </summary>
    public static class EmployeeDomainModelExtensionMethods
    {
        /// <summary>
        /// Maps an domain side employee model to a read side one.
        /// </summary>
        /// <param name="value">read side instance</param>
        /// <param name="existing">existing version to map to</param>
        /// <returns>read side model</returns>
        public static EmployeeDetail ToEmployeeDetail(this EmployeeModel value, EmployeeDetail existing = null)
        {
            existing = existing ?? new EmployeeDetail();
            existing.Id = value.Id;
            existing.Name = value.Name;
            existing.Benefits = value.Benefits;
            existing.Dependents = (byte)value.Dependents.Length;
            existing.GrossPay = value.GrossPay;
            existing.IsDeleted = value.IsDeleted;
            existing.NetPay = value.NetPay;
            existing.Version = value.Version;

            return existing;
        }

        /// <summary>
        /// Maps an domain side dependent model to a read side one.
        /// </summary>
        /// <param name="value">read side instance</param>
        /// <param name="existing">existing version to map to</param>
        /// <returns>read side model</returns>
        public static DependentDetail ToDependentDetail(this DependentModel value, DependentDetail existing = null)
        {
            existing = existing ?? new DependentDetail();
            existing.Id = value.Id;
            existing.EmployeeId = value.EmployeeId;
            existing.Name = value.Name;

            return existing;
        }
    }
}
