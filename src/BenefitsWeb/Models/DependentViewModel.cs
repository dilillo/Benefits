using Benefits.Infrastructure.Models;
using Benefits.QueryData;
using System.ComponentModel.DataAnnotations;

namespace BenefitsWeb.Models
{
    /// <summary>
    /// Represents dependent information in the UI.
    /// </summary>
    public class DependentViewModel
    {
        public string Id { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        public short EmployeeVersion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public static DependentViewModel FromQueryModel(DependentDetail data)
        {
            if (data == null)
                return null;

            return new DependentViewModel()
            {
                Id = data.Id,
                EmployeeId = data.EmployeeId,
                EmployeeVersion = data.EmployeeDetail.Version,
                Name = data.Name
            };
        }

        public static DependentModel ToDomainModel(DependentViewModel value)
        {
            return new DependentModel()
            {
                Id = value.Id,
                Name = value.Name
            };
        }
    }
}