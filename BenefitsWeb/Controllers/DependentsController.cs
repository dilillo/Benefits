
using System.Web.Mvc;

namespace BenefitsWeb.Controllers
{
    /// <summary>
    /// Handles requests for dependent related pages.
    /// </summary>
    public class DependentsController : Controller
    {
        /// <summary>
        /// Gets the create depenent page.
        /// </summary>
        /// <param name="employeeId">unique id of the employee to which the dependent will be associated.</param>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue, VaryByParam = "employeeId")]
        public ActionResult Create(string employeeId)
        {
            ViewBag.EmployeeId = employeeId;

            return View();
        }

        /// <summary>
        /// Gets the edit dependent page.
        /// </summary>
        /// <param name="id">unique id of the dependent to be edited</param>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult Edit(string id)
        {
            ViewBag.DependentId = id;

            return View();
        }
    }
}