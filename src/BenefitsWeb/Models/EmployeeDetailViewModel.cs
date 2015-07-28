using Benefits.Infrastructure.Models;
using Benefits.QueryData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BenefitsWeb.Models
{
    /// <summary>
    /// Represents employee detail information in the UI.
    /// </summary>
    public class EmployeeDetailViewModel
    {
        public EmployeeDetailViewModel()
        {
            Dependents = new DependentViewModel[] { };
        }

        public string Id { get; set; }

        public short Version { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public decimal GrossPay { get; set; }

        public decimal Benefits { get; set; }

        public decimal NetPay { get; set; }

        public IEnumerable<DependentViewModel> Dependents { get; set; }

        public static EmployeeDetailViewModel FromQueryModel(EmployeeDetail value)
        {
            if (value == null)
                return null;

            return new EmployeeDetailViewModel()
            {
                Id = value.Id,
                Name = value.Name,
                Version = value.Version,
                GrossPay = value.GrossPay,
                Benefits = value.Benefits,
                NetPay = value.NetPay,
                Dependents = value.DependentDetails.Select(i => DependentViewModel.FromQueryModel(i))
            };
        }

        public static EmployeeModel ToDomainModel(EmployeeDetailViewModel value)
        {
            return new EmployeeModel()
            {
                Id = value.Id,
                Name = value.Name,
                Version = value.Version,
                GrossPay = value.GrossPay,
                Benefits = value.Benefits,
                NetPay = value.NetPay,
                Dependents = value.Dependents.Select(i => DependentViewModel.ToDomainModel(i)).ToArray()
            };
        }
    }
}