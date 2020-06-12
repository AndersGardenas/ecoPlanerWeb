using econoomic_planer_X;
using econoomic_planer_X.ResourceSet;
using Server.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace ecoServer.Server.Domain.Services.EntitServices
{
    public class ResourceService
    {
        readonly EcoContext Context;

        public ResourceService(EcoContext context)
        {
            Context = context;
        }


        public List<TradingResource> GetResourcesForContry(List<Population> populations)
        {

            IQueryable<TradingResource> tr = Context.TradingResource.Where(t => t.ExternalTradingResourcesID == null && populations.Contains(t.Owner));

            return tr.ToList();
        }


        public List<TradingResource> GetExternalResourcesForContry(List<Population> populations)
        {

            IQueryable<TradingResource> tr = Context.TradingResource.Where(t => t.ExternalTradingResourcesID != null && populations.Contains(t.Owner));

            return tr.ToList();
        }

    }
}
