using econoomic_planer_X;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ecoServer.Server.Domain.Services
{
    public class ContryService
    {
        EcoContext Context;

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
            Region region;
            foreach (Contry contry in Context.Contry)
            {
                IQueryable<Population> pop = GetPopulationOfContry(contry.ID, out region);
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
            return Context.Population.Where(p => regions.Any(r => r.regionID == p.RegionID));
        }
    }
}
