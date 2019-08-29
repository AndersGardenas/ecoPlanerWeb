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
            int contryId = context.Contry.First(c => c.Name.Equals(name)).ID;
            IQueryable<Region> regions = context.Region.Where(r => r.ContryID == contryId);
            Region region =  regions.FirstOrDefault();
            if (region == null)
            {
                return NotFound();
            }
            IQueryable<Population> populations = context.Population.Where(p => regions.Any(r => r.ID == p.RegionID));

            string toSend = populations.Sum(p => p.PopLevel).ToString();
            toSend += "," + populations.Sum(p => p.Money).ToString();
            var resData = context.ResourceData.Where(rd => rd.InternalMarketId == region.InternalMarket.Id).ToList();
            toSend += "," + resData[0].ResourceType.ToString() + " " + resData[0].ResourcesPrice;
            toSend += "," + resData[1].ResourceType.ToString() + " " + resData[1].ResourcesPrice;
            //foreach (ResourceData r in  regions.FirstOrDefault().InternalMarket.ResourceData)
            //{
            //    toSend += " " + r.Id + " " +  r.ResourcesPrice.ToString();
            //}
            return Ok(toSend);
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Region>> GetAllContry()
        {
            IEnumerable<Region> regions = context.Region;
            return Ok(regions);
        }
    }
}
