using econoomic_planer_X;
using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using Microsoft.EntityFrameworkCore;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ecoServer.Server.Domain.Services
{
    public class ContryService
    {
        readonly EcoContext Context;

        public ContryService(EcoContext context)
        {
            Context = context;
        }


        public List<string> GetAllContryGDP()
        {
            var contryInfo = new List<string>();
            foreach (Contry contry in Context.Contry)
            {
                IQueryable<Population> pop = GetPopulationOfContry(contry.ID, out Region region);
                contryInfo.Add(contry.Name + ":" + Math.Max(0, pop.Sum(p => p.Money) / pop.Sum(p => p.PopLevel)).ToString());
            }
            return contryInfo;
        }

        public List<string> GetAllContryPop()
        {
            var contryInfo = new List<string>();
            foreach (Contry contry in Context.Contry)
            {
                IQueryable<Population> pop = GetPopulationOfContry(contry.ID, out Region region);
                contryInfo.Add(contry.Name + ":" + ((long)pop?.Sum(p => p.PopLevel)).ToString());
            }
            return contryInfo;
        }


        public IQueryable<Population> GetPopulationOfContry(int contryId, out Region region)
        {
            IQueryable<Region> regions = Context.Region.Where(r => r.ContryID == contryId);
            region = regions.FirstOrDefault();
            if (region == null)
            {
                return null;
            }
            return Context.Population.Where(p => regions.Any(r => r.RegionID == p.RegionID));
        }

        public int? GetIdByName(string name)
        {
            if (name == null)
            {
                return null;
            }
            return Context.Contry.First(c => c.Name.Equals(name)).ID;
        }

        public List<string> GetAllContryTradePartners()
        {
            var contryInfo = new List<string>();
            foreach (Contry contry in Context.Contry)
            {
                IQueryable<Population> pop = GetPopulationOfContry(contry.ID, out _);
                var tradingPartners = GetTradingPartners(pop);
                if (tradingPartners == null) {
                    contryInfo.Add(contry.Name + ":0");
                }
                else {
                    contryInfo.Add(contry.Name + ":" + tradingPartners.Count);
                }
            }
            return contryInfo;
        }


        public ICollection<Region> GetTradingPartners(IQueryable<Population> populations)
        {
            IQueryable<ExternatlTradingResource> tr = Context.ExternatlTradingResource.Where(t => populations.Contains(t.Owner));
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
