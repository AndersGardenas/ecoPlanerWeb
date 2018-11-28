using econoomic_planer_X.ResourceSet;
using ecoServer.Server.Domain.Services.Market;
using Server.Server.Domain.model.ResourceSet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.Market
{
    public class InternalMarket
    {
        public Guid Id { get; set; }

        public virtual List<TradingResources> Supply { get; set; }
        public virtual List<ResourceData> ResourceData { get; set; }

        private Resources Demand;

        private const double ExternalTradingThreshold = 1;
        private const int PriceSlownes = 10;

        public InternalMarket() {
            ResourceData = new List<ResourceData>();
            Supply = new List<TradingResources>();
            for (int i = 0; i < ResourceType.totalAmount; i++)
            {
                ResourceData.Add(new ResourceData());
                Supply.Add(new TradingResources());
            }
            Demand = new Resources();
        }

        public void ComputeNewStock(ExternalMarket externalMarket) {
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                ComputeResourceRatio(resourceType);
                ComputeExternalTrade(resourceType, externalMarket);
            }
            externalMarket.UpdateTrade(Supply);

            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                MergeDublicates(resourceType);

                ComputeResourceRatio(resourceType);
            }
        }

        private void MergeDublicates(ResourceType resourceType) {
            if (Supply[resourceType.Id].Count() == 0) {
                return;
            }
            var dict = Supply[resourceType.Id].tradingResources.GroupBy(tr => new { tr.Owner.ID, tr.ResourceType.Id }).Select(g => g.First()).ToList().ToDictionary(su => new { su.Owner.ID, su.ResourceType.Id });
            foreach (var su in Supply[resourceType.Id].tradingResources) {
                TradingResource tradingResource = dict[new { su.Owner.ID, su.ResourceType.Id }];
                if (tradingResource.Id.CompareTo(su.Id) != 0 && tradingResource.CompareTo(su) == 0) {
                    tradingResource.Add(su);
                }
            }
            Supply[resourceType.Id].tradingResources = dict.Values.ToList();
        }

        private void ComputeResourceRatio(ResourceType resourceType) {
            double supplyAmount = Supply[resourceType.Id].tradingResources.Sum(su => su.Amount);
            double demandAmount = Demand.GetAmount(resourceType);
            ResourceData[resourceType.Id].ResourceRatio = demandAmount > 0 ? supplyAmount / demandAmount : double.PositiveInfinity;
        }

        private void ComputeExternalTrade(ResourceType resourceType, ExternalMarket externalMarket) {
            var resourceRatio = ResourceData[resourceType.Id].ResourceRatio;
            double externalPrice = externalMarket.GetBestCost(resourceType, out TradeRegion destination);

            double priceRatio = externalPrice / ResourceData[resourceType.Id].ResourcesPrice;
            externalMarket.IncreaseTradeWith(resourceType,destination,priceRatio);
            externalMarket.DoExternalTrade(Supply[resourceType.Id].tradingResources,resourceType);
        }


        public void DoTrade(List<Population> populations) {
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                var resourceRatio = ResourceData[resourceType.Id].ResourceRatio;
                double buyRatio = Math.Min(1, resourceRatio);
                double sellRatio = Math.Min(1, 1 / resourceRatio);
                double price = ResourceData[resourceType.Id].ResourcesPrice;

                foreach (Population pop in populations) {
                    pop.BuyAmount(buyRatio, price, resourceType);
                }
                Supply[resourceType.Id].tradingResources.ForEach(su => su.Trade(sellRatio, price));
                Supply[resourceType.Id].tradingResources.RemoveAll(su => su.Empty());

            }
        }

        public void UpdatedPrices() {
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                ResourceData[resourceType.Id].ResourcesPrice *= 1 + (Math.Max(Math.Min(1 / ResourceData[resourceType.Id].ResourceRatio - 1, PriceSlownes), -PriceSlownes)) / 100;

                Console.Out.WriteLine(resourceType.Name);
                Console.Out.WriteLine("price: " + ResourceData[resourceType.Id].ResourcesPrice);
            }
        }

        public void UpdateMarket(List<Population> populations) {
            UpdateDemand(populations);
            UpdateSupply(populations);
        }

        public void UpdateSupply(List<Population> populations) {
            foreach (Population population in populations) {
                population.Produce();
                Supply[population.producingType.Id].Add(new TradingResource(population, population.producingType, population.SellingAmount()));
            }
        }

        public void UpdateDemand(List<Population> populations) {
            foreach (Population population in populations) {
                population.UpdateDemand(this);
            }
        }

        public void CleanUp() {
            UpdatedPrices();
            Demand.Reset();
        }

        public double GetPrice(ResourceType resourceType, double amount) {
            return ResourceData[resourceType.Id].ResourcesPrice * amount;
        }


        public double GetPrice(ResourceType resourceType) {
            return GetPrice(resourceType, 1);
        }

        public void AddDemand(Resources resources) {
            foreach (Resource resource in resources) {
                AddDemand(resource);
            }
        }

        public void AddDemand(Resource resource) {
            Demand.Adjust(resource);
        }
    }

}