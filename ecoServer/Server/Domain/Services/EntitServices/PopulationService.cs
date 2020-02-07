using econoomic_planer_X;
using econoomic_planer_X;
using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using Server.Server.Infrastructure;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ecoServer.Server.Domain.Services
{
    public class PopulationService
    {
        EcoContext Context;
        public PopulationService(EcoContext context)
        {
            Context = context;
        }


        public ICollection<Region> GetTradingPartners(IQueryable<Population> populations)
        {
            IQueryable<ExternatlTradingResource> tr =  Context.ExternatlTradingResource.Where(t => t.Destination != null && populations.Contains(t.Owner));
            if (tr.Count() == 0)
            {
                return null;
            }
            List<ExternalMarket> market = new List<ExternalMarket>();
            foreach (ExternatlTradingResource resource in tr)
            {
                if (resource?.Destination?.MarketDestination != null)
                {
                    market.Add(resource.Destination.MarketDestination);
                }
            }
            IQueryable<Region> regions = Context.Region.Where(r => market.Contains(r.ExternalMarket));
            if (regions.Count() == 0)
            {
                return null;
            }
            return regions.ToList();
        }
    }
}
