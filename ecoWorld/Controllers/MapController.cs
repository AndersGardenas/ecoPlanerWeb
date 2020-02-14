using econoomic_planer_X;
using ecoServer.Server.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System;
using System.Threading;
using System.Globalization;
using Server.Server.Domain.model.ResourceSet;

namespace ecoPlanerWeb.Controllers
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        protected EcoContext Context { get; set; }
        protected ContryService ContryService { get; set; }
        protected PopulationService PopulationService { get; set; }

        public MapController(EcoContext context, ContryService contryService, PopulationService populationService)
        {
            Context = context;
            ContryService = contryService;
            PopulationService = populationService;
        }

        [HttpGet("[action]")]
        public IActionResult GetContry(string name)
        {
            int? contryId = ContryService.GetIdByName(name);
            if (contryId == null)
            {
                return NotFound();
            }
            IQueryable<Population> populations = ContryService.GetPopulationOfContry(contryId.Value, out Region region);
            var resData = Context.ResourceData.Where(rd => rd.InternalMarketId == region.InternalMarketId).ToList();

            string json;
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString("populations", ((long)populations.Sum(p => p.PopLevel)).ToString());
                    writer.WriteString("money", ((long)populations.Sum(p => p.Money)).ToString());

                    foreach (ResourceData resourceData in resData)
                    {
                        writer.WriteString(resourceData.ResourceType.ToString(), resourceData.ResourcesPrice.ToString());
                    }
                    writer.WriteEndObject();
                }
                json = Encoding.UTF8.GetString(stream.ToArray());

            }
            return Ok(json);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllContry()
        {
            List<string> contriesPop = ContryService.GetAllContryPop();
            string result = "";
            foreach (var contry in contriesPop)
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

        [HttpGet("[action]")]
        public IActionResult GetTradingPartner(string name)
        {
            int? contryId = ContryService.GetIdByName(name);
            if (contryId == null)
            {
                return NotFound();
            }

            IQueryable<Population> populations = ContryService.GetPopulationOfContry(contryId.Value, out Region outRegion);
            ICollection<Region> regions = PopulationService.GetTradingPartners(populations);
            if (regions == null)
            {
                return Ok();
            }
            string CultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = ci;
            }
            string json;
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString("home", outRegion.CenterX + "|" + outRegion.CenterY);
                    int i = 0;
                    foreach (Region region in regions)
                    {
                        writer.WriteString(i.ToString(), region.CenterX + "|" + region.CenterY);
                        i++;
                    }
                    writer.WriteEndObject();
                }
                json = Encoding.UTF8.GetString(stream.ToArray());

            }
            return Ok(json);
        }


    }
}
