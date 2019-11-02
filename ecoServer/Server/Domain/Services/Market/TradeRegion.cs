using econoomic_planer_X;
using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using System;

namespace ecoServer.Server.Domain.Services.Market
{
    public class TradeRegion
    {
        public int ID { get; set; }
        public virtual Region Region { get; set; }
        public int ResourcesId { get; set; }
        public virtual Resources transportAmount { get; set; }
        private readonly double tradeChange = 0.01;

        public TradeRegion() { }

        public TradeRegion(Region region)
        {
            Region = region;
        }

        public void Init()
        {
            transportAmount = new Resources().Init();
        }

        public double GetTransportCost()
        {
            return Region.GetTransportCost();
        }

        public double GetResorceCost(ResourceTypes.ResourceType resourceType)
        {
            return Region.GetResorceCost(resourceType);
        }

        public ExternalMarket GetExternalMarket()
        {
            return Region.ExternalMarket;
        }

        public void IncreaseTrade(ResourceTypes.ResourceType resourceType)
        {
            double newAmount = Math.Min(transportAmount.GetResource(resourceType).Amount + tradeChange, 1);
            transportAmount.SetResource(resourceType, newAmount);
        }

        public void DecreseTrade(ResourceTypes.ResourceType resourceType)
        {
            if (transportAmount.ResoucreExist(resourceType) == false)
            {
                return;
            }
            double resourceAmount = transportAmount.GetResource(resourceType).Amount;
            if (resourceAmount == 0) return;

            double newAmount = Math.Max(resourceAmount - tradeChange, 0);
            transportAmount.SetResource(resourceType, newAmount);
        }

        public double GetTransportAmount(ResourceTypes.ResourceType resourceType)
        {
            return transportAmount.GetAmount(resourceType);
        }
    }
}
