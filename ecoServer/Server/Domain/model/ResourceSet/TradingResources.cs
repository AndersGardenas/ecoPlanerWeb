using econoomic_planer_X.Market;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X.ResourceSet
{
    public class TradingResources
    {
        public int TradingResourcesID { get; set; }

        public virtual ResourceTypes.ResourceType ResourceType { get; set; }
        public virtual List<TradingResource> TradingResourceList { get; set; }

        public TradingResources()
        {
        }

        public TradingResources(ResourceTypes.ResourceType ResourceType)
        {
            this.ResourceType = ResourceType;
        }

        public TradingResources Init()
        {
            TradingResourceList = new List<TradingResource>();
            return this;
        }

        public void Update(Population population, ResourceTypes.ResourceType producingType, double amount)
        {
            TradingResource tradingResource = TradingResourceList.Find(tr => tr.ResourceType == producingType && tr.Owner.ID == population.ID);
            if (tradingResource != null)
            {
                tradingResource.Amount = amount;

            }
            else
            {
                TradingResourceList.Add(new TradingResource(population, producingType, amount));
            }
        }

        public void Add(TradingResource tradingResource)
        {
            TradingResourceList.Add(tradingResource);
        }

        public TradingResource Get(ResourceTypes.ResourceType producingType)
        {
            return TradingResourceList.Find(tr => tr.ResourceType == producingType);
        }

        internal int Count()
        {
            return TradingResourceList.Count;
        }

        public void Sort()
        {
            TradingResourceList.Sort();
        }
    }
}
