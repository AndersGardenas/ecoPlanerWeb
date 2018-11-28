using econoomic_planer_X.ResourceSet;
using ecoServer.Server.Domain.Services.Market;
using System;
using System.Collections.Generic;

namespace econoomic_planer_X.Market
{
    public class ExternalMarket
    {

        public Guid Id { get; set; }
        public Region Ownregion { get; set; }

        public virtual List<TradeRegion> TradeRegions { get; set; }
        public virtual List<ExternatlTradingResource> ExternatlTradingResources { get; set; }
        public virtual List<ExternatlTradingResource> NewExternatlTradingResources { get; set; }


        public ExternalMarket()
        {
            ExternatlTradingResources = new List<ExternatlTradingResource>();
            NewExternatlTradingResources = new List<ExternatlTradingResource>();
        }

        public ExternalMarket(Region Ownregion, List<Region> BorderRegions)
        {
            ExternatlTradingResources = new List<ExternatlTradingResource>();
            NewExternatlTradingResources = new List<ExternatlTradingResource>();
            TradeRegions = new List<TradeRegion>();
            foreach (Region region in BorderRegions)
            {
                TradeRegions.Add(new TradeRegion(region));
            }
            this.Ownregion = Ownregion;
        }

        public void AddNeighbour(Region region)
        {
            TradeRegions.Add(new TradeRegion(region));
        }


        public double GetBestCost(ResourceType resourceType, out TradeRegion bestRegion)
        {
            double bestCost = 0;
            bestRegion = null;
            double regionalTransportCost = Ownregion.GetTransportCost() / 2;

            foreach (TradeRegion region in TradeRegions)
            {
                double externalTransportCost = region.GetTransportCost() / 2;
                double totalTransportCost = regionalTransportCost + externalTransportCost;
                double resourceCost = region.GetResorceCost(resourceType);
                double salesCost = resourceCost - totalTransportCost;
                if (salesCost > bestCost)
                {
                    bestCost = salesCost;
                    bestRegion = region;
                }
            }
            return bestCost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TradingResources"></param>
        /// <param name="resourceType"></param>
        public void DoExternalTrade(List<TradingResource> TradingResources, ResourceType resourceType)
        {
            foreach (TradeRegion tradeRegion in TradeRegions)
            {
                double tradeRegionRatio = tradeRegion.GetTransportAmount(resourceType);
                if (tradeRegionRatio == 0)
                {
                    continue;
                }
                foreach (TradingResource tradingResounce in TradingResources)
                {
                    if (tradingResounce.AffordTransport())
                    {
                        ExternatlTradingResources.Add(tradingResounce.SplitExternal(tradeRegionRatio, tradeRegion.GetExternalMarket(), Ownregion.GetTransportTime() / 2));
                    }
                }
            }
        }


        public void IncreaseTradeWith(ResourceType resourceType, TradeRegion destination,double priceRatio)
        {
            foreach (TradeRegion tradeRegion in TradeRegions)
            {
                if (priceRatio > 1 && destination.Id == tradeRegion.Id)
                {
                    tradeRegion.IncreaseTrade(resourceType);
                }
                else
                {
                    tradeRegion.DecreseTrade(resourceType);
                }
            }
        }


        public void UpdateTrade(List<TradingResources> TradingResources)
        {
            foreach (ExternatlTradingResource tradingResounce in ExternatlTradingResources)
            {
                tradingResounce.TransportADay();
                if (tradingResounce.AtDestination())
                {
                    if (tradingResounce.getDestination().Id == Id)
                    {
                        TradingResources[tradingResounce.ResourceType.Id].Add(tradingResounce);
                    }
                    else
                    {
                        tradingResounce.getDestination().AddResource(tradingResounce);
                    }
                }
            }
            ExternatlTradingResources.RemoveAll(ex => ex.AtDestination());
        }


        private void AddResource(ExternatlTradingResource tradingResounce)
        {
            NewExternatlTradingResources.Add(tradingResounce);
        }

        public void FinilizeTrades()
        {
            foreach (ExternatlTradingResource newExternatlTradingResource in NewExternatlTradingResources)
            {
                newExternatlTradingResource.SetDaysRemaning(Ownregion.GetTransportTime() / 2);
                ExternatlTradingResources.Add(newExternatlTradingResource);
            }
            NewExternatlTradingResources.Clear();
        }
    }
}

