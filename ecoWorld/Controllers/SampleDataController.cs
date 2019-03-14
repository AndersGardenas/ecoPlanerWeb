using econoomic_planer_X;
using Microsoft.AspNetCore.Mvc;
using Server.Server.Infrastructure;
using System.Linq;

namespace ecoPlanerWeb.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        protected EcoContext context;
        public SampleDataController(EcoContext context)
        {
            this.context = context;
        }

        [HttpGet("[action]")]
        public string getContry(string name)
        {
            if (name == null)
            {
                return "";
            }
            IQueryable<Region> regions = context.Region.Where(r => r.ContryID == context.Contry.First(c => c.Name.Equals(name)).ID);
            return context.Population.Where(p => regions.Any(r => r.ID == p.RegionID)).Sum(p => p.popLevel).ToString();
        }
    }
}
