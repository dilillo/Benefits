using Benefits.QueryBiz;
using System.Web.Mvc;

namespace BenefitsWeb.Controllers
{
    /// <summary>
    /// Handles requests for home related pages.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="queries">IQueries instance to use to pull kpi data for the index page</param>
        public HomeController(IQueries queries)
        {
            _queries = queries;
        }

        // internal state
        readonly IQueries _queries;

        /// <summary>
        /// Gets the index page.
        /// </summary>
        /// <returns>ViewResult instance representing the view to render</returns>
        public ActionResult Index()
        {
            return View(Models.KpisViewModel.FromQueryModel(_queries.GetAllKpis()));
        }

        /// <summary>
        /// Gets the about page.
        /// </summary>
        /// <returns>ViewResult instance representing the view to render</returns>
        [OutputCache(Duration = int.MaxValue)]
        public ActionResult About()
        {
            return View();
        }
    }
}