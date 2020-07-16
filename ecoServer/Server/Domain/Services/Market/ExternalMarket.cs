using econoomic_planer_X.ResourceSet;
using ecoServer.Server.Domain.Services.Market;
using Server.Server.Domain.model.ResourceSet;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X.Market
{
    public class ExternalMarket
    {
        private double externalTreshold = 2;

        public int Id { get; set; }
        [ForeignKey("ExternalMarketRegion")]

        public virtual Region Ownregion { get; set; }

        public virtual List<TradeRegion> TradeRegions { get; set; }

        public virtual ExternalTradingResources ExternatlTradingResourcesInTransit { get; set; }
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
            ExternatlTradingResourcesInTransit = new ExternalTradingResources().Init();
            TradeRegions = new List<TradeRegion>();
        }

        public void AddNeighbour(Region region)
        {
            var trade = new TradeRegion(region);
            trade.Init();
            TradeRegions.Add(trade);
        }

        public void UpdateTrade(List<ExternalTradingResources> TradingResources)
        {
            foreach (ExternalTradingResource tradingResounce in ExternatlTradingResourcesInTransit.TradingResourceList)
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
            ExternatlTradingResourcesInTransit.TradingResourceList.RemoveAll(ex => ex.AtDestination());
        }

        public double GetBestCost(ResourceTypes.ResourceType resourceType, out TradeRegion bestRegion, out TradeRegion worstRegion)
        {
            double bestCost = 0;
            double worstCost = double.MaxValue;
            bestRegion = null;
            worstRegion = null;
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
                if (tradeRegion.GetTransportAmount(resourceType) > 0 && salesCost < worstCost)
                {
                    worstCost = salesCost;
                    worstRegion = tradeRegion;
                }
            }
            return bestCost;
        }

        public void IncreaseTradeWith(ResourceTypes.ResourceType resourceType, TradeRegion destination, TradeRegion worstTradeRegion)
        {
            foreach (TradeRegion tradeRegion in TradeRegions)
            {

                if (destination.ID == tradeRegion.ID)
                {
                    if (worstTradeRegion == null || worstTradeRegion.ID == destination.ID)
                    {
                        tradeRegion.MaxTrade(resourceType);
                        continue;
                    }
                    tradeRegion.IncreaseTrade(resourceType);
                }
                else if (worstTradeRegion != null && worstTradeRegion.ID == tradeRegion.ID)
                {
                    worstTradeRegion.DecreseTrade(resourceType);
                }
            }
        }

        public void ComputeExternalTrade(ExternalTradingResources externalTradingResources, double demand, ResourceTypes.ResourceType resourceType,
            double resourceRatio, InternalMarket internalMarket)
        {
            double externalPrice = GetBestCost(resourceType, out TradeRegion destination, out TradeRegion worstTradeRegion);
            IncreaseTradeWith(resourceType, destination, worstTradeRegion);

            double priceRatio = externalPrice / internalMarket.GetSupplyToDemandRatio(resourceType).ResourcesPrice;
            DoExternalTradeWithExternalResources(priceRatio, demand, externalTradingResources, resourceType);

            if (priceRatio <= 1.05)
            {
                return;
            }
            DoExternalTradeWithInternalResources(internalMarket.GetSupply(resourceType).TradingResourceList, resourceType, resourceRatio);
        }


        private void DoExternalTradeWithExternalResources(double priceRatio, double demand, ExternalTradingResources externalTradingResources, ResourceTypes.ResourceType resourceType)
        {
            double amountOfResources = externalTradingResources.TradingResourceList.Count;
            if (amountOfResources == 0)
            {
                return;
            }
            double transportTimeOutOfRigion = Ownregion.GetTransportTime() / 2;
            double keepAmount = getSendRatio(demand, priceRatio);
            if (keepAmount >= amountOfResources)
            {
                return;
            }
            double exportRatio = (amountOfResources - keepAmount) / amountOfResources;
            foreach (ExternalTradingResource externalTradingResource in externalTradingResources.TradingResourceList)
            {
                double totalSellAmount = externalTradingResource.Amount * exportRatio;

                foreach (TradeRegion tradeRegion in TradeRegions)
                {
                    double tradeRegionRatio = tradeRegion.GetTransportAmount(resourceType);
                    if (tradeRegionRatio <= 0)
                    {
                        continue;
                    }

                    if (externalTradingResource.AffordTransport(tradeRegionRatio, externalTradingResource.Amount))
                    {
                        ExternatlTradingResourcesInTransit.TradingResourceList.Add(externalTradingResource.SplitAmountExternal(totalSellAmount * tradeRegionRatio,
                            tradeRegion.GetExternalMarket(), transportTimeOutOfRigion));
                    }
                }

            }

        }

        private double getSendRatio(double demand, double priceRatio)
        {
            if (priceRatio > externalTreshold)
            {
                return 0;
            }
            else if (priceRatio < 0.9)
            {
                return double.MaxValue;
            }
            else
            {
                return demand * (1 / priceRatio);
            }
        }

        private void DoExternalTradeWithInternalResources(List<TradingResource> TradingResources, ResourceTypes.ResourceType resourceType, double resourceRatio)
        {
            if (resourceRatio == 0)
            {
                return;
            }
            double exportRatio = 1 - (1 / resourceRatio);
            if (exportRatio <= 0)
            {
                return;
            }

            double transportTimeOutOfRigion = Ownregion.GetTransportTime() / 2;
            foreach (TradingResource tradingResounce in TradingResources)
            {
                double totalSellAmount = tradingResounce.Amount * exportRatio;

                foreach (TradeRegion tradeRegion in TradeRegions)
                {
                    double tradeRegionRatio = tradeRegion.GetTransportAmount(resourceType);
                    if (tradeRegionRatio <= 0)
                    {
                        continue;
                    }

                    if (tradingResounce.AffordTransport(tradeRegionRatio, tradingResounce.Amount))
                    {
                        ExternatlTradingResourcesInTransit.TradingResourceList.Add(tradingResounce.SplitAmountExternal(totalSellAmount * tradeRegionRatio, tradeRegion.GetExternalMarket(), transportTimeOutOfRigion));
                    }
                }
            }
        }

        private void AddResource(ExternalTradingResource tradingResounce)
        {
            NewExternatlTradingResources.TradingResourceList.Add(tradingResounce);
        }

        public void FinilizeTrades()
        {
            foreach (ExternalTradingResource newExternatlTradingResource in NewExternatlTradingResources.TradingResourceList)
            {
                newExternatlTradingResource.SetDaysRemaning(Ownregion.GetTransportTime() / 2);
                ExternatlTradingResourcesInTransit.TradingResourceList.Add(newExternatlTradingResource);
            }
            NewExternatlTradingResources.TradingResourceList.Clear();
        }
    }
}

