using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.PopulationTypes
{
    public class Demand
    {
        private double[] Needs = new double[Resource.ResourceTypeSize()];
        private double[] LifeValues = new double[Resource.ResourceTypeSize()];
        Resources resourceDemand = new Resources();
        double totalLifleValue {get; set;}


        public Demand()
        {
            SetDemand(ResourceTypes.GetResourceType("Fruit"),1,100);
            SetDemand(ResourceTypes.GetResourceType("Cloth"),1,10);
            totalLifleValue = LifeValues.Sum();
        }

        public double GetNeedAdjusted(ResourceType resourceType)
        {
            return LifeValues[resourceType.Id] / totalLifleValue;
        }


        public double GetDemand(ResourceType resourceType)
        {
            return resourceDemand.GetAmount(resourceType);
        }

        public double GetNeed(ResourceType resourceType)
        {
            return Needs[resourceType.Id];
        }


        public void SetDemand(ResourceType resourceType,double amountNeeded, double lifeValue)
        {
            Needs[resourceType.Id] = amountNeeded;
            LifeValues[resourceType.Id] = lifeValue;
        }

        public double TotalLifleValue()
        {
            return LifeValues.Sum();
        }

        public void UpdateDemand(InternalMarket market, Population population)
        {
            resourceDemand.Reset();

            var lifePriceRatioMap = new Dictionary<ResourceType, double>();
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                if (GetNeed(resourceType) == 0)
                {
                    continue;
                }
                lifePriceRatioMap[resourceType] = LifeValues[resourceType.Id] / market.GetPrice(resourceType);
            }
            ComputeWhatToBuy(market, population, lifePriceRatioMap);
            market.AddDemand(resourceDemand);
        }


        private void ComputeWhatToBuy(InternalMarket market, Population population, Dictionary<ResourceType, double> lifePriceRatioMap)
        {
            double money = population.money;
            IEnumerable<KeyValuePair<ResourceType, double>> orderedLifePriceRatio  = lifePriceRatioMap.OrderByDescending(ratio => ratio.Value);
            foreach (KeyValuePair<ResourceType, double> resoucreTypeRatio in orderedLifePriceRatio)
            {
                ResourceType resourceType = resoucreTypeRatio.Key;
                double price = market.GetPrice(resourceType);
                double buyAmount = Math.Min(money / price, population.GetPopLevel() * GetNeed(resourceType));
                resourceDemand.SetResource(new Resource(resourceType, buyAmount));
                money -= buyAmount * price;
                if (money <= 0)
                {
                    break;
                }
            }
        }

        //public static double GetResourceValue(ResourceType resourceType)
        //{
        //    switch (resourceType)
        //    {
        //        case ResourceType.Cloth:
        //            return 10;
        //        case ResourceType.Fruit:
        //            return 100;
        //        case ResourceType.Wool:
        //            return 0;
        //        default: 
        //            return 0;
        //    }
        //}
    }
}
