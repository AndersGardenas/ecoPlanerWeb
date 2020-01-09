using econoomic_planer_X;
using econoomic_planer_X.Market;
using ecoServer.Server.Domain.Services;
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
        protected ContryService ContryService { get; set; }

        public SampleDataController(EcoContext context, ContryService contryService)
        {
            Context = context;
            ContryService = contryService;
        }

        [HttpGet("[action]")]
        public IActionResult GetContry(string name)
        {
            Region region;

            if (name == null)
            {
                return NotFound();
            }
            int contryId = Context.Contry.First(c => c.Name.Equals(name)).ID;
            IQueryable<Population> populations = ContryService.GetPopulationOfContry(contryId, out region);

            var toSend = ((long)populations.Sum(p => p.PopLevel)).ToString();
            toSend += "|" + ((long)populations.Sum(p => p.Money)).ToString();
            var resData = Context.ResourceData.Where(rd => rd.InternalMarketId == region.InternalMarketId).ToList();
            toSend += "|" + resData[0].ResourceType.ToString() + " " + resData[0].ResourcesPrice;
            toSend += "|" + resData[1].ResourceType.ToString() + " " + resData[1].ResourcesPrice;

            return Ok(toSend);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllContry()
        {
            List<string> contriesPop = ContryService.GetAllContryPop();
            string result = "";
            foreach(var contry in contriesPop)
            {
                result += contry + ";";
            }
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetGDPContry()
        {
            List<string> contriesPop = ContryService.GetAllContryGDP();
            string result = "";
            foreach (var contry in contriesPop)
            {
                result += contry + ";";
            }
            return Ok(result);
        }


    }
}
