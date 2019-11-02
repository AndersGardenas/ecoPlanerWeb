using econoomic_planer_X;
using econoomic_planer_X.Market;
using Microsoft.AspNetCore.Mvc;
using Server.Server.Domain.model.ResourceSet;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ecoPlanerWeb.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        protected EcoContext Context { get; set; }

        public SampleDataController(EcoContext context)
        {
            Context = context;
        }

        [HttpGet("[action]")]
        public IActionResult getContry(string name)
        {
            if (name == null)
            {
                return NotFound();
            }
            int contryId = Context.Contry.First(c => c.Name.Equals(name)).ID;
            IQueryable<Region> regions = Context.Region.Where(r => r.ContryID == contryId);
            Region region =  regions.FirstOrDefault();
            if (region == null)
            {
                return NotFound();
            }
            IQueryable<Population> populations = Context.Population.Where(p => regions.Any(r => r.regionID == p.RegionID));

            var toSend = ((long)populations.Sum(p => p.PopLevel)).ToString();
            toSend += "|" + ((long)populations.Sum(p => p.Money)).ToString();
            var resData = Context.ResourceData.Where(rd => rd.InternalMarketId == region.InternalMarketId).ToList();
            toSend += "|" + resData[0].ResourceType.ToString() + " " + resData[0].ResourcesPrice;
            toSend += "|" + resData[1].ResourceType.ToString() + " " + resData[1].ResourcesPrice;

            return Ok(toSend);
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Region>> GetAllContry()
        {
            IEnumerable<Region> regions = Context.Region;
            return Ok(regions);
        }
    }
}
