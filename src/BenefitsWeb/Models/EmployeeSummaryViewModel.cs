using Benefits.QueryData;

namespace BenefitsWeb.Models
{
    /// <summary>
    /// Represents employee summary information in the UI.
    /// </summary>
    public class EmployeeSummaryViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public static EmployeeSummaryViewModel FromData(EmployeeDetail data)
        {
            if (data == null)
                return null;

            return new EmployeeSummaryViewModel()
            {
                 Id = data.Id,
                 Name = data.Name
            };
        }
    }
}