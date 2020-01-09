﻿using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.PopulationTypes
{
    public static class Demand
    {
        private const double minemumDemand = 0.01;

        public static double[] AmountNeeded { get; set; }
        public static double[] LifeValues { get; set; }
        public static PrimitivResource[] ResourceDemand { get; set; }
        public static double TotalLifleValue { get; set; }

        public static void Init()
        {
            ResourceDemand = new PrimitivResource[Resource.ResourceTypeSize()];

            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                ResourceDemand[(int)resourceType] = new PrimitivResource(resourceType, 0);
            }

            AmountNeeded = new double[Resource.ResourceTypeSize()];
            LifeValues = new double[Resource.ResourceTypeSize()];
            double lifeValueFruit = 100;
            double lifeValueCloth = 10;
            TotalLifleValue = lifeValueFruit + lifeValueCloth;

            SetDemand(ResourceTypes.ResourceType.Fruit, 1, lifeValueFruit / TotalLifleValue);
            SetDemand(ResourceTypes.ResourceType.Cloth, 1, lifeValueCloth / TotalLifleValue);

        }

        public static double GetLifeValueAdjusted(ResourceTypes.ResourceType resourceType)
        {
            return LifeValues[(int)resourceType];
        }

        public static double GetAmountNeeded(ResourceTypes.ResourceType resourceType)
        {
            return Convert.ToDouble(AmountNeeded[(int)resourceType]);
        }


        public static double GetDemand(ResourceTypes.ResourceType resourceType)
        {
            return ResourceDemand[(int)resourceType].Amount;
        }


        public static void SetDemand(ResourceTypes.ResourceType resourceType, double amountNeeded, double lifeValue)
        {
            AmountNeeded[(int)resourceType] = amountNeeded;
            LifeValues[(int)resourceType] = lifeValue;
        }

        public static void UpdateDemand(InternalMarket market, Population population)
        {
            resetDemand();

            var lifePriceRatioMap = new Dictionary<ResourceTypes.ResourceType, double>();
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                if (GetAmountNeeded(resourceType) == 0)
                {
                    continue;
                }
                lifePriceRatioMap[resourceType] = Convert.ToDouble(LifeValues[(int)resourceType]) / market.GetPrice(resourceType);
            }
            ComputeWhatToBuy(market, population, lifePriceRatioMap);
            market.AddDemand(ResourceDemand);
        }


        private static void ComputeWhatToBuy(InternalMarket market, Population population, Dictionary<ResourceTypes.ResourceType, double> lifePriceRatioMap)
        {
            double money = population.Money;
            IEnumerable<KeyValuePair<ResourceTypes.ResourceType, double>> orderedLifePriceRatio = lifePriceRatioMap.OrderByDescending(ratio => ratio.Value);
            foreach (KeyValuePair<ResourceTypes.ResourceType, double> resoucreTypeRatio in orderedLifePriceRatio)
            {
                ResourceTypes.ResourceType resourceType = resoucreTypeRatio.Key;
                double price = market.GetPrice(resourceType);
                double buyAmount = Math.Min(money / price, population.GetIntegerPopLevel() * GetAmountNeeded(resourceType));
                if (buyAmount < minemumDemand)
                {
                    continue;
                }
                ResourceDemand[(int)resourceType].Amount = buyAmount;
                money -= buyAmount * price;
                if (money <= 0)
                {
                    break;
                }
            }
        }

        private static void resetDemand()
        {
            for (int i = 0; i < ResourceDemand.Length; i++)
            {
                ResourceDemand[i].Amount = 0;
            }
        }
    }
}
