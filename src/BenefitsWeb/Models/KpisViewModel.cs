using Benefits.QueryData;
using System.Collections.Generic;
using System.Linq;

namespace BenefitsWeb.Models
{
    /// <summary>
    /// Represents key performance indicator information in the UI.
    /// </summary>
    public class KpisViewModel
    {
        public int Employees { get; set; }

        public int GrossPay { get; set; }

        public int Benefits { get; set; }

        public int NetPay { get; set; }

        public static KpisViewModel FromQueryModel(IEnumerable<Kpi> data)
        {
            if (data == null || data.Count() == 0)
                return new KpisViewModel();

            return new KpisViewModel()
            {
                Employees = data.Single(i => i.Id == "Employees").Value,
                GrossPay = data.Single(i => i.Id == "GrossPay").Value,
                Benefits = data.Single(i => i.Id == "Benefits").Value,
                NetPay = data.Single(i => i.Id == "NetPay").Value
            };
        }
    }
}