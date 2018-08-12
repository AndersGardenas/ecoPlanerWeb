using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace econoomic_planer_X.Market
{
    public class ExternalMarket
    {
        public List<Region> BorderRegions;
        Region ownregion;
        List<ExternatlTradingResource> externatlTradingResources = new List<ExternatlTradingResource>();
        List<ExternatlTradingResource> newExternatlTradingResources = new List<ExternatlTradingResource>();


        public ExternalMarket(Region ownregion, List<Region> BorderRegions)
        {
            this.BorderRegions = BorderRegions;
            this.ownregion = ownregion;
        }

        
        public double GetBestCost(ResourceType resourceType, out ExternalMarket bestRegion)
        {
            double bestCost = 0;
            bestRegion = null;
            double regionalTransportCost = ownregion.GetTransportCost()/2;

            foreach(Region region in BorderRegions)
            {
                double externalTransportCost = region.GetTransportCost()/2;
                double totalTransportCost = regionalTransportCost + externalTransportCost;
                double resourceCost = region.GetResorceCost(resourceType);
                double salesCost = resourceCost - totalTransportCost;
                if (salesCost > bestCost)
                {
                    bestCost = salesCost;
                    bestRegion = region.GetExternalMarket();
                }
            }

            return bestCost;
        }

        public void StartTrade(List<TradingResource> TradingResources, double ratio,ExternalMarket destination)
        {
            foreach(TradingResource tradingResounce in TradingResources)
            {
                if (tradingResounce.AffordTransport()){
                    externatlTradingResources.Add(tradingResounce.SplitExternal(ratio,destination,ownregion.GetTransportTime()/2));
                }
            }
        }


        public void UpdateTrade(List<TradingResource>[] TradingResources)
        {        
            foreach(ExternatlTradingResource tradingResounce in externatlTradingResources)
            {
                tradingResounce.TransportADay(); 
                if(tradingResounce.AtDestination()){
                    if(tradingResounce.Destination == this){
                        TradingResources[tradingResounce.ResourceType.Id].Add(tradingResounce);
                    }
                   else
                    {
                        tradingResounce.Destination.AddResource(tradingResounce);
                    }
                }
            }
            externatlTradingResources.RemoveAll(ex => ex.AtDestination());
        }


        private void AddResource(ExternatlTradingResource tradingResounce)
        {
            newExternatlTradingResources.Add(tradingResounce);
        }

        public void FinilizeTrades()
        {
            foreach(ExternatlTradingResource newExternatlTradingResource in newExternatlTradingResources){
                newExternatlTradingResource.DaysRemaning = ownregion.GetTransportTime()/2;
                externatlTradingResources.Add(newExternatlTradingResource); 
            }
            newExternatlTradingResources.Clear();
        }
    }
}

