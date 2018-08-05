using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.Market
{
    public class InternalMarket
    {
        List<TradingResource>[] supply = new List<TradingResource>[Resource.ResourceTypeSize()];
        Resources demand = new Resources();
        double[] resourcesPrice =   new double[Resource.ResourceTypeSize()];
        List<Population> populations = new List<Population>();

        double[] resourceRatio = new double[Resource.ResourceTypeSize()];

        public ExternalMarket externalMarket;
        double externalTradingThreshold = 1;
        int priceSlownes = 10;

        public InternalMarket(Region region)
        {
            externalMarket = new ExternalMarket(region,region.Negbours);
            populations = region.populations;
            for(int i = 0; i < resourcesPrice.Length; i++)
            {
                resourcesPrice[i] = 1;
                supply[i] = new List<TradingResource>();
            }
        }

        public void ComputeNewStock()
        {
            foreach(ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                ComputeResourceRatio(resourceType, out double supplyAmount, out double demandAmount);
                ComputeExternalTrade(resourceType);
            }
            externalMarket.UpdateTrade(supply);

            foreach(ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                MergeDublicates(resourceType);

                ComputeResourceRatio(resourceType, out double supplyAmount, out double demandAmount);
            }
        }

        private void MergeDublicates(ResourceType resourceType)
        {
            if (supply[resourceType.GuId].Count == 0)
            {
                return;
            }
            var dict = supply[resourceType.GuId].GroupBy(tr => new {tr.Owner.Id, tr.ResourceType.GuId }).Select(g => g.First()).ToList().ToDictionary(su => new {su.Owner.Id, su.ResourceType.GuId });
            foreach (var su in supply[resourceType.GuId])
            {
                TradingResource tradingResource = dict[new {su.Owner.Id, su.ResourceType.GuId }];
                if (tradingResource.Id.CompareTo(su.Id) != 0 && tradingResource.CompareTo(su) == 0)
                {
                    tradingResource.Add(su);
                }
            }
            supply[resourceType.GuId] = dict.Values.ToList();
        }

        private void ComputeResourceRatio(ResourceType resourceType, out double supplyAmount, out double demandAmount)
        {
            supplyAmount = supply[resourceType.GuId].Sum(su => su.Amount);
            demandAmount = demand.GetAmount(resourceType);
            resourceRatio[resourceType.GuId] = demandAmount > 0 ? supplyAmount / demandAmount  : double.PositiveInfinity;
        }

        private void ComputeExternalTrade(ResourceType resourceType)
        {

            if (resourceRatio[resourceType.GuId] > 1)
            {
                double externalPrice = externalMarket.GetBestCost(resourceType, out ExternalMarket destination);
                
                double priceRatio = externalPrice / resourcesPrice[resourceType.GuId];
                if (priceRatio > externalTradingThreshold)
                {
                    externalMarket.StartTrade(supply[resourceType.GuId],((resourceRatio[resourceType.GuId]-1)/ resourceRatio[resourceType.GuId]),destination);
                }
            }     
        }


        public void DoTrade()
        {
            foreach(ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                double ratio = resourceRatio[resourceType.GuId];
                double buyRatio = Math.Min(1,ratio);
                double sellRatio = Math.Min(1,1/ratio);
                double price = resourcesPrice[resourceType.GuId];

                foreach(Population pop in populations)
                {
                    pop.BuyAmount(buyRatio,price,resourceType);
                }
                supply[resourceType.GuId].ForEach(su => su.Trade(sellRatio,price));
                supply[resourceType.GuId].RemoveAll(su => su.Empty());

            }
        }

        public void UpdatedPrices()
        {
            foreach(ResourceType resourceType in ResourceTypes.resourceTypes){
                 resourcesPrice[resourceType.GuId] *=  1 + (Math.Max(Math.Min(1/resourceRatio[resourceType.GuId] - 1,priceSlownes),-priceSlownes))/100;

                 Console.Out.WriteLine(resourceType.Name);
                 Console.Out.WriteLine("price: " +  resourcesPrice[resourceType.GuId]);
            }
        }

        public void UpdateMarket()
        {
            UpdateDemand();
            UpdateSupply();
        }

        public void UpdateSupply(){
            foreach(Population population in populations)
            {
                population.Produce();
                supply[population.producingType.GuId].Add(new TradingResource(population,population.producingType, population.SellingAmount()));
            }
        }

        public void UpdateDemand(){
            foreach(Population population in populations)
            {
                population.UpdateDemand(this);
            } 
        }

        public void CleanUp()
        {
            externalMarket.FinilizeTrades();
            UpdatedPrices();
            demand.Reset();
        }

        public double GetPrice(ResourceType resourceType,double amount)
        {
            return resourcesPrice[resourceType.GuId] * amount;
        }
       

        public double GetPrice(ResourceType resourceType)
        {
             return resourcesPrice[resourceType.GuId];
        }

        public void AddDemand(Resources resources)
        {
            foreach(Resource resource in resources)
            {
                AddDemand(resource);
            }
        }

        public void AddDemand(Resource resource)
        {
            demand.Adjust(resource);
        }
    }

}