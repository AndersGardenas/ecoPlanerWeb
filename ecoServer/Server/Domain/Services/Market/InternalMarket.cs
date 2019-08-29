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
        public int Id { get; set; }

        private  TradingResources [] Supply;
        public virtual List<ResourceData> ResourceData { get; set; }

        private Resources Demand = new Resources().Init();

        private const double ExternalTradingThreshold = 1;
        private const int PriceSlownes = 10;

        public InternalMarket()
        {
            ResourceTypes.ResourceType[] resourceTypes = ResourceTypes.GetIterator();
            Supply = new TradingResources[resourceTypes.Length];
            int i = 0;
            foreach (ResourceTypes.ResourceType resourceType in resourceTypes) { 
                Supply[i] = new TradingResources(resourceType);
                i++;
            }
        }

        public void Init()
        {
            ResourceData = new List<ResourceData>();
            ResourceTypes.ResourceType[] resourceTypes = ResourceTypes.GetIterator();
            foreach (ResourceTypes.ResourceType resourceType in resourceTypes)
            {
                ResourceData.Add(new ResourceData(resourceType));
            }
        }

        public void ComputeNewStock(ExternalMarket externalMarket)
        {
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                ComputeResourceRatio(resourceType);
                ComputeExternalTrade(resourceType, externalMarket);
            }
            externalMarket.UpdateTrade(Supply);

            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                MergeDublicates(resourceType);

                ComputeResourceRatio(resourceType);
            }
        }

        private void MergeDublicates(ResourceTypes.ResourceType resourceType)
        {
            if (GetSupply(resourceType).Count() == 0)
            {
                return;
            }
            var dict = GetSupply(resourceType).tradingResources.
                GroupBy(tr => new { tr.Owner.ID, tr.ResourceType }).
                Select(g => g.First()).ToList().
                ToDictionary(su => new { su.Owner.ID, su.ResourceType });
            foreach (var su in GetSupply(resourceType).tradingResources)
            {
                TradingResource tradingResource = dict[new { su.Owner.ID, su.ResourceType }];
                if (tradingResource.Id.CompareTo(su.Id) != 0 && tradingResource.CompareTo(su) == 0)
                {
                    tradingResource.Add(su);
                }
            }
            GetSupply(resourceType).tradingResources = dict.Values.ToList();
        }

        private void ComputeResourceRatio(ResourceTypes.ResourceType resourceType)
        {
            double supplyAmount = GetSupply(resourceType).tradingResources.Sum(su => su.Amount);
            double demandAmount = Demand.GetAmount(resourceType);
            GetResourceData(resourceType).ResourceRatio = demandAmount > 0 ? supplyAmount / demandAmount : double.MaxValue;
        }

        private void ComputeExternalTrade(ResourceTypes.ResourceType resourceType, ExternalMarket externalMarket)
        {
            //var resourceRatio = GetResourceData(resourceType).ResourceRatio;
            double externalPrice = externalMarket.GetBestCost(resourceType, out TradeRegion destination);

            double priceRatio = externalPrice / GetResourceData(resourceType).ResourcesPrice;
            externalMarket.IncreaseTradeWith(resourceType, destination, priceRatio);
            externalMarket.DoExternalTrade(GetSupply(resourceType).tradingResources, resourceType);
        }


        public void DoTrade(List<Population> populations)
        {
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                var resourceRatio = GetResourceData(resourceType).ResourceRatio;
                double buyRatio = Math.Min(1, resourceRatio);
                double sellRatio = Math.Min(1, 1 / resourceRatio);
                double price = GetResourceData(resourceType).ResourcesPrice;

                foreach (Population pop in populations)
                {
                    pop.BuyAmount(buyRatio, price, resourceType);
                }
                GetSupply(resourceType).tradingResources.ForEach(su => su.Trade(sellRatio, price, resourceType));
                GetSupply(resourceType).tradingResources.RemoveAll(su => su.Empty());
            }
        }

        public void UpdatedPrices()
        {
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                GetResourceData(resourceType).ResourcesPrice *= 1 + Math.Max(Math.Min(1 / GetResourceData(resourceType).ResourceRatio - 1, PriceSlownes), -PriceSlownes) / 100;
            }
        }

        public void UpdateMarket(List<Population> populations)
        {
            UpdateDemand(populations);
            UpdateSupply(populations);
        }

        public void UpdateSupply(List<Population> populations)
        {
            foreach (Population population in populations)
            {
                population.Produce();
                GetSupply(population.ProducingType)
                    .Add(new TradingResource(population, population.ProducingType, population.SellingAmount()));
            }
        }

        public void UpdateDemand(List<Population> populations)
        {
            foreach (Population population in populations)
            {
                population.UpdateDemand(this);
            }
        }

        public void CleanUp()
        {
            UpdatedPrices();
            Demand.Reset();
        }

        public double GetPrice(ResourceTypes.ResourceType resourceType, double amount)
        {
            return GetResourceData(resourceType).ResourcesPrice * amount;
        }


        public double GetPrice(ResourceTypes.ResourceType resourceType)
        {
            return GetPrice(resourceType, 1);
        }

        public void AddDemand(PrimitivResource[] resources)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                AddDemand(resources[i]);
            }
        }

        public void AddDemand(PrimitivResource resource)
        {
            Demand.Adjust(resource);
        }

        private TradingResources GetSupply(ResourceTypes.ResourceType resourceType)
        {
        //    return Supply.Find(s => s.ResourceType.Equals(resourceType));
              return Supply[(int)resourceType];
        }

        private ResourceData GetResourceData(ResourceTypes.ResourceType resourceType)
        {
            return ResourceData.Find(r => r.ResourceType.Equals(resourceType));
        }

    }

}