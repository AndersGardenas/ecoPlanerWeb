using econoomic_planer_X;
using Microsoft.AspNetCore.Mvc;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
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
        public IActionResult getContry(string name)
        {
            if (name == null)
            {
                return NotFound();
            }
            Guid contryId = context.Contry.First(c => c.Name.Equals(name)).ID;
            IQueryable<Region> regions = context.Region.Where(r => r.ContryID == contryId);
            if (regions.FirstOrDefault() == null)
            {
                return NotFound();
            }
            return  Ok(context.Population.Where(p => regions.Any(r => r.ID == p.RegionID)).Sum(p => p.popLevel).ToString());
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Region>> GetAllContry()
        {
            IEnumerable<Region> regions = context.Region;
            return Ok(regions);
         }
    }
}
