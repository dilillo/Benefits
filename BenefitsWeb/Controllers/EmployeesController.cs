
using System.Web.Mvc;

namespace BenefitsWeb.Controllers
{
    /// <summary>
    /// Handles requests for employee related pages.
    /// </summary>
    public class EmployeesController : Controller
    {
        /// <summary>
        /// Gets the employee index page.
        /// </summary>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the employee create page.
        /// </summary>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue)]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Gets the employee details page.
        /// </summary>
        /// <param name="id">unique id of the employee</param>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult Details(string id)
        {
            ViewBag.EmployeeId = id;

            return View();
        }

        /// <summary>
        /// Gets the employee edit page.
        /// </summary>
        /// <param name="id">unique id of the employee</param>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult Edit(string id)
        {
            ViewBag.EmployeeId = id;

            return View();
        }
    }
}