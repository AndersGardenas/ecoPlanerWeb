using econoomic_planer_X;
using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using System;

namespace ecoServer.Server.Domain.Services.Market
{
    public class TradeRegion
    {
        public Guid ID { get; set; }
        public Region Region {get; set;}
        private Resources transportAmount;
        private readonly double tradeChange = 0.05;


        public TradeRegion() { }


        public TradeRegion(Region region)
        {
            transportAmount = new Resources();
            Region = region;
        }

        public double GetTransportCost()
        {
            return Region.GetTransportCost();
        }

        public double GetResorceCost(ResourceType resourceType)
        {
            return Region.GetResorceCost(resourceType);
        }

        public ExternalMarket GetExternalMarket()
        {
            return Region.GetExternalMarket();
        }

        public void IncreaseTrade(ResourceType resourceType)
        {
            double newAmount = Math.Min(transportAmount.GetResource(resourceType).Amount + tradeChange,1);
            transportAmount.SetResource(resourceType,newAmount);
        }

        public void DecreseTrade(ResourceType resourceType)
        {
            double resourceAmount = transportAmount.GetResource(resourceType).Amount;
            if (resourceAmount == 0) return;

            double newAmount = Math.Min(resourceAmount - tradeChange,1);
            transportAmount.SetResource(resourceType,newAmount);
        }

        public double GetTransportAmount(ResourceType resourceType)
        {
            return transportAmount.GetAmount(resourceType);
        }
    }
}
