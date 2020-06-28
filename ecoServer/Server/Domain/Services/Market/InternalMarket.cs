using econoomic_planer_X.ResourceSet;
using ecoServer.Server.Domain.Services.Market;
using Server.Server.Domain.model.ResourceSet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace econoomic_planer_X.Market
{
    public class InternalMarket
    {
        public int InternalMarketId { get; set; }

        [ForeignKey("Supply")]
        public virtual List<TradingResources> Supply { get; set; }
        [ForeignKey("ExternalSupply")]
        public virtual List<ExternalTradingResources> ExternalSupply { get; set; }
        public virtual List<ResourceData> ResourceData { get; set; }

        [NotMapped]
        private readonly Resources Demand = new Resources().Init();
        private readonly PrimitivResource[] SupplySum = new PrimitivResource[ResourceTypes.TotalAmount()];
        private const int PriceSlownes = 10;

        public InternalMarket()
        {

        }

        public void Init()
        {
            ResourceData = new List<ResourceData>();
            Supply = new List<TradingResources>();
            ExternalSupply = new List<ExternalTradingResources>();
            int i = 0;
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                ResourceData.Add(new ResourceData(resourceType));
                Supply.Add(new TradingResources(resourceType).Init());
                ExternalSupply.Add(new ExternalTradingResources());
                i++;

            }
        }

        public void ComputeNewStock(ExternalMarket externalMarket)
        {
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                ComputeResourceRatio(resourceType);
                externalMarket.ComputeExternalTrade(GetExternalSupply(resourceType), resourceType, GetResourceData(resourceType).GetResourceRatio(),  this);
            }
            externalMarket.UpdateTrade(ExternalSupply);

            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                MergeDublicates(GetSupply(resourceType));
                MergeDublicates(GetExternalSupply(resourceType));
                ComputeResourceRatio(resourceType);
            }
        }

        private void MergeDublicates(TradingResources tradingResources)
        {
            tradingResources.TradingResourceList.RemoveAll(r => r.Amount <= 0);
            if (tradingResources.Count() < 2)
            {
                return;
            }

            var toKepp = new List<TradingResource>();
            foreach (var groupedTradingResources in tradingResources.TradingResourceList.GroupBy(r => r.Owner))
            {
                BuildCombinedResourceList(toKepp, groupedTradingResources);
            }

            tradingResources.TradingResourceList.RemoveAll(r => !toKepp.Any(re => re.TradingResourceId == r.TradingResourceId));
        }

        private void MergeDublicates(ExternalTradingResources tradingResources)
        {
            tradingResources.TradingResourceList.RemoveAll(r => r.Amount <= 0);
            if (tradingResources.Count() < 2)
            {
                return;
            }

            var toKepp = new List<TradingResource>();
            foreach (var groupedTradingResources in tradingResources.TradingResourceList.GroupBy(r => r.Owner))
            {
                BuildCombinedResourceList(toKepp, groupedTradingResources);
            }

            tradingResources.TradingResourceList.RemoveAll(r => !toKepp.Any(re => re.TradingResourceId == r.TradingResourceId));
        }

        private static void BuildCombinedResourceList(List<TradingResource> toKepp, IGrouping<Population, TradingResource> rList)
        {
            TradingResource first = rList.First();
            first.Amount = rList.Sum(r => r.Amount);
            toKepp.Add(first);
        }

        private void ComputeResourceRatio(ResourceTypes.ResourceType resourceType)
        {
            var supplyAmount = GetSupply(resourceType).TradingResourceList.Sum(su => su.Amount);
            supplyAmount += GetExternalSupply(resourceType).TradingResourceList.Sum(su => su.Amount);
            SupplySum[(int)resourceType].Amount = supplyAmount;
            double demandAmount = Demand.GetAmount(resourceType);
            GetResourceData(resourceType).SetResourceRatio(ComputeResourceRatio(demandAmount, supplyAmount));
        }

        private double ComputeResourceRatio(double demandAmount, double supplyAmount)
        {
            return demandAmount > 0 ? supplyAmount / demandAmount : double.MaxValue;
        }

        public void DoTrade(List<Population> populations)
        {
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                var demmand = Demand.GetAmount(resourceType);
                if (demmand == 0 || SupplySum[(int)resourceType].Amount == 0)
                {
                    return;
                }

                var resoucreData = GetResourceData(resourceType);
                var resourceRatio = resoucreData.GetResourceRatio();
                double buyRatio = Math.Min(1, resourceRatio);
                double price = resoucreData.ResourcesPrice;

                var supply = GetSupply(resourceType);

                var exportSupply = GetExternalSupply(resourceType);

                double sellRatio = Math.Min(1, 1 / ComputeResourceRatio(demmand, SupplySum[(int)(resourceType)].Amount));

                foreach (Population pop in populations)
                {
                    pop.BuyAmount(buyRatio, price, resourceType);
                }

                supply.TradingResourceList.ForEach(tr => tr.Trade(sellRatio, price, resourceType));
                supply.TradingResourceList.RemoveAll(su => su.Empty());

                exportSupply.TradingResourceList.ForEach(tr => tr.Trade(sellRatio, price, resourceType));
                exportSupply.TradingResourceList.RemoveAll(su => su.Empty());
            }
        }

        double AutoTrade(TradingResources supply, double demmand, double price, ResourceTypes.ResourceType resourceType)
        {
            foreach (TradingResource tr in supply.TradingResourceList)
            {
                if (tr.Amount < (demmand * 0.1))
                {
                    demmand -= tr.Amount;
                    SupplySum[(int)resourceType].Amount -= tr.Amount;
                    tr.Trade(1, price, resourceType);
                }
                else
                {
                    break;
                }
            }
            supply.TradingResourceList.RemoveAll(su => su.Empty());
            return demmand;
        }

        public void UpdatedPrices()
        {
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                GetResourceData(resourceType).ResourcesPrice = GetNewPrice(resourceType);
            }
        }

        private double GetNewPrice(ResourceTypes.ResourceType resourceType)
        {
            return Math.Max(0.000001, Math.Min(1000000, GetResourceData(resourceType).ResourcesPrice * (1 + Math.Max(Math.Min(1 / GetResourceData(resourceType).GetResourceRatio() - 1, PriceSlownes), -PriceSlownes) / 100)));
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
                    .Update(population, population.ProducingType, population.AddToMarket());
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

        public TradingResources GetSupply(ResourceTypes.ResourceType resourceType)
        {
            return Supply[(int)resourceType];
        }

        private ExternalTradingResources GetExternalSupply(ResourceTypes.ResourceType resourceType)
        {
            return ExternalSupply[(int)resourceType];
        }

        public ResourceData GetResourceData(ResourceTypes.ResourceType resourceType)
        {
            return ResourceData[(int)resourceType];
        }

    }

}