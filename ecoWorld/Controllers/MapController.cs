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
using ecoServer.Server.Domain.Services.EntitServices;

namespace ecoPlanerWeb.Controllers
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        protected EcoContext Context { get; set; }
        protected ContryService ContryService { get; set; }
        protected PopulationService PopulationService { get; set; }
        protected ResourceService ResourceService { get; set; }

        public MapController(EcoContext context, ContryService contryService, PopulationService populationService, ResourceService resourceService)
        {
            Context = context;
            ContryService = contryService;
            PopulationService = populationService;
            ResourceService = resourceService;
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

            string json;
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString("populations", ((long)populations.Sum(p => p.PopLevel)).ToString());
                    writer.WriteString("money", ((long)populations.Sum(p => p.Money)).ToString());

                    WriteResourcePrice(region, writer);

                    WriteRecoureces(populations, writer);

                    WriteExternalRecoureces(populations, writer);

                    writer.WriteEndObject();
                }
                json = Encoding.UTF8.GetString(stream.ToArray());

            }
            return Ok(json);
        }

        private void WriteResourcePrice(Region region, Utf8JsonWriter writer)
        {
            var resData = Context.ResourceData.Where(rd => rd.InternalMarketId == region.InternalMarketId).ToList();
            foreach (ResourceData resourceData in resData)
            {
                writer.WriteString("Cost_of_" + resourceData.ResourceType.ToString(), resourceData.ResourcesPrice.ToString());
            }
        }

        private void WriteRecoureces(IQueryable<Population> populations, Utf8JsonWriter writer)
        {
            var ownedResources = ResourceService.GetResourcesForContry(populations.ToList());
            if (ownedResources.Count == 0)
            {
                return;
            }
            foreach (var resourceType in ownedResources.GroupBy(r => r.ResourceType))
            {
                double amountORresourceType = resourceType.Sum(r => r.Amount);
                writer.WriteString(resourceType.Key.ToString(), (amountORresourceType).ToString());
            }
        }

        private void WriteExternalRecoureces(IQueryable<Population> populations, Utf8JsonWriter writer)
        {
            var ownedExternalResources = ResourceService.GetExternalResourcesForContry(populations.ToList());
            if (ownedExternalResources.Count == 0)
            {
                return;
            }
            foreach (var resourceType in ownedExternalResources.GroupBy(r => r.ResourceType))
            {
                double amountORresourceType = resourceType.Sum(r => r.Amount);
                writer.WriteString("External " + resourceType.Key.ToString(), (amountORresourceType).ToString());
            }
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
        public IActionResult GetContryTradePartners()
        {
            List<string> contriesPop = ContryService.GetAllContryTradePartners();
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
            ICollection<Region> regions = ContryService.GetTradingPartners(populations);
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
