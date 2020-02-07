using econoomic_planer_X.ResourceSet;
using ecoServer.Server.Domain.Services.Market;
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

        public virtual ExternalTradingResources ExternatlTradingResources { get; set; }
        [NotMapped]
        private ExternalTradingResources NewExternatlTradingResources { get; set; }

        public ExternalMarket()
        {
            NewExternatlTradingResources = new ExternalTradingResources().Init();
        }

        public ExternalMarket(Region Ownregion) : this()
        {

            this.Ownregion = Ownregion;
        }

        public void Init()
        {
            ExternatlTradingResources = new ExternalTradingResources().Init();
            TradeRegions = new List<TradeRegion>();
        }

        public void AddNeighbour(Region region)
        {
            var trade = new TradeRegion(region);
            trade.Init();
            TradeRegions.Add(trade);
        }


        public double GetBestCost(ResourceTypes.ResourceType resourceType, out TradeRegion bestRegion)
        {
            double bestCost = 0;
            bestRegion = null;
            double regionalTransportCost = Ownregion.GetTransportCost();

            foreach (TradeRegion tradeRegion in TradeRegions)
            {
                double externalTransportCost = tradeRegion.GetTransportCost();
                double totalTransportCost = (regionalTransportCost + externalTransportCost) / 2;
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
                if (tradeRegionRatio <= 0)
                {
                    continue;
                }
                foreach (TradingResource tradingResounce in TradingResources)
                {
                    if (tradingResounce.AffordTransport(tradeRegionRatio, tradingResounce.Amount))
                    {
                        ExternatlTradingResources.Values.Add(tradingResounce.SplitExternal(tradeRegionRatio, tradeRegion.GetExternalMarket(), Ownregion.GetTransportTime() / 2));
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

        public void UpdateTrade(List<TradingResources> TradingResources)
        {
            foreach (ExternatlTradingResource tradingResounce in ExternatlTradingResources.Values)
            {
                tradingResounce.TransportADay();
                if (tradingResounce.AtDestination())
                {
                    if (tradingResounce.getDestination().Id == Id)
                    {
                        //Add the transaction the suply list
                        TradingResources[(int)tradingResounce.ResourceType].Add(tradingResounce);
                    }
                    else
                    {
                        //Resoucre leaving region 
                        tradingResounce.getDestination().AddResource(tradingResounce);
                    }
                }
            }
            ExternatlTradingResources.Values.RemoveAll(ex => ex.AtDestination());
        }

        private void AddResource(ExternatlTradingResource tradingResounce)
        {
            NewExternatlTradingResources.Values.Add(tradingResounce);
        }

        public void FinilizeTrades()
        {
            foreach (ExternatlTradingResource newExternatlTradingResource in NewExternatlTradingResources.Values)
            {
                newExternatlTradingResource.SetDaysRemaning(Ownregion.GetTransportTime() / 2);
                ExternatlTradingResources.Values.Add(newExternatlTradingResource);
            }
            NewExternatlTradingResources.Values.Clear();
        }
    }
}

