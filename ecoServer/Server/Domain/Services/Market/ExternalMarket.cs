using econoomic_planer_X.ResourceSet;
using ecoServer.Server.Domain.Services.Market;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X.Market
{
    public class ExternalMarket
    {

        public int Id { get; set; }
        [ForeignKey("ExternalMarketRegion")]

        public virtual Region Ownregion { get; set; }

        public virtual List<TradeRegion> TradeRegions { get; set; }
        public virtual List<ExternatlTradingResource> ExternatlTradingResources { get; set; }
        public virtual List<ExternatlTradingResource> NewExternatlTradingResources { get; set; }

        public ExternalMarket()
        {
        }

        public ExternalMarket(Region Ownregion) : this()
        {
            ExternatlTradingResources = new List<ExternatlTradingResource>();
            NewExternatlTradingResources = new List<ExternatlTradingResource>();
            TradeRegions = new List<TradeRegion>();
            this.Ownregion = Ownregion;
        }

        public void SetNeighbors(List<NeighbourRegion> BorderRegions)
        {
            foreach (NeighbourRegion region in BorderRegions)
            {
                Region tempRegion = region.OwnRegion == Ownregion ? region.NeighbouringRegion : region.OwnRegion;
                TradeRegions.Add(new TradeRegion(tempRegion));
            }
        }

        public void AddNeighbour(Region region)
        {
            TradeRegions.Add(new TradeRegion(region));
        }


        public double GetBestCost(ResourceTypes.ResourceType resourceType, out TradeRegion bestRegion)
        {
            double bestCost = 0;
            bestRegion = null;
            double regionalTransportCost = Ownregion.GetTransportCost();

            foreach (TradeRegion tradeRegion in TradeRegions)
            {
                double externalTransportCost = tradeRegion.GetTransportCost();
                double totalTransportCost = (regionalTransportCost + externalTransportCost)/2;
                double resourceCost = tradeRegion.GetResorceCost(resourceType);
                double salesCost = resourceCost - totalTransportCost;
                if (salesCost > bestCost)
                {
                    bestCost = salesCost;
                    bestRegion = tradeRegion;
                }
            }
            return bestCost;
        }

        public void DoExternalTrade(List<TradingResource> TradingResources, ResourceTypes.ResourceType resourceType)
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


        public void IncreaseTradeWith(ResourceTypes.ResourceType resourceType, TradeRegion destination, double priceRatio)
        {
            foreach (TradeRegion tradeRegion in TradeRegions)
            {
                if (priceRatio > 1 && destination.ID == tradeRegion.ID)
                {
                    tradeRegion.IncreaseTrade(resourceType);
                }
                else
                {
                    tradeRegion.DecreseTrade(resourceType);
                }
            }
        }

        public void UpdateTrade(TradingResources[] TradingResources)
        {
            foreach (ExternatlTradingResource tradingResounce in ExternatlTradingResources)
            {
                tradingResounce.TransportADay();
                if (tradingResounce.AtDestination())
                {
                    if (tradingResounce.getDestination().Id == Id)
                    {
                       // TradingResources.Find(tr => tr.ResourceType.Equals(tradingResounce.ResourceType)).Add(tradingResounce);
                          TradingResources[(int)tradingResounce.ResourceType].Add(tradingResounce);
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

