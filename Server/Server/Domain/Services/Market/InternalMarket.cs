using econoomic_planer_X.ResourceSet;
using Server.Server.Domain.model.ResourceSet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.Market
{
    public class InternalMarket
    {
        public Guid Id { get; set; }
        private Region Ownregion { get; set; }

        public virtual List<TradingResources> Supply { get; set; }
        public virtual List<Population> Populations { get; set; }
        public virtual List<ResourceData> ResourceData { get; set; }

        public ExternalMarket ExternalMarket { get; set; }
        private Resources Demand { get; set; }

        private const double ExternalTradingThreshold = 1;
        private const int PriceSlownes = 10;

        public InternalMarket() { }


        public InternalMarket(Region region) {
            ResourceData = new List<ResourceData>();
            Populations = new List<Population>();
            Supply = new List<TradingResources>();
            ExternalMarket = new ExternalMarket(region, region.Negbours);
            Populations = region.populations;
            Demand = new Resources();
        }

        public void ComputeNewStock() {
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                ComputeResourceRatio(resourceType, out double supplyAmount, out double demandAmount);
                ComputeExternalTrade(resourceType);
            }
            ExternalMarket.UpdateTrade(Supply);

            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                MergeDublicates(resourceType);

                ComputeResourceRatio(resourceType, out double supplyAmount, out double demandAmount);
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

        private void ComputeResourceRatio(ResourceType resourceType, out double supplyAmount, out double demandAmount) {
            supplyAmount = Supply[resourceType.Id].tradingResources.Sum(su => su.Amount);
            demandAmount = Demand.GetAmount(resourceType);
            ResourceData[resourceType.Id].ResourceRatio = demandAmount > 0 ? supplyAmount / demandAmount : double.PositiveInfinity;
        }

        private void ComputeExternalTrade(ResourceType resourceType) {
            var resourceRatio = ResourceData[resourceType.Id].ResourceRatio;
            if (resourceRatio > 1) {
                double externalPrice = ExternalMarket.GetBestCost(resourceType, out ExternalMarket destination);

                double priceRatio = externalPrice / ResourceData[resourceType.Id].ResourcesPrice;
                if (priceRatio > ExternalTradingThreshold) {
                    ExternalMarket.StartTrade(Supply[resourceType.Id].tradingResources, (resourceRatio - 1) / resourceRatio, destination);
                }
            }
        }


        public void DoTrade() {
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                var resourceRatio = ResourceData[resourceType.Id].ResourceRatio;
                double buyRatio = Math.Min(1, resourceRatio);
                double sellRatio = Math.Min(1, 1 / resourceRatio);
                double price = ResourceData[resourceType.Id].ResourcesPrice;

                foreach (Population pop in Populations) {
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

        public void UpdateMarket() {
            UpdateDemand();
            UpdateSupply();
        }

        public void UpdateSupply() {
            foreach (Population population in Populations) {
                population.Produce();
                Supply[population.producingType.Id].Add(new TradingResource(population, population.producingType, population.SellingAmount()));
            }
        }

        public void UpdateDemand() {
            foreach (Population population in Populations) {
                population.UpdateDemand(this);
            }
        }

        public void CleanUp() {
            ExternalMarket.FinilizeTrades();
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