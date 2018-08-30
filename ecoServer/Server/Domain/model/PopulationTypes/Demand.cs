using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using Server.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.PopulationTypes
{
    public class Demand
    {
        public Guid ID { get; set; }
        public virtual StringArray AmountNeeded { get; set; }
        public virtual StringArray LifeValues { get; set; }
        public Resources ResourceDemand { get; set; }
        public double TotalLifleValue  { get; set; }

        public Demand() {
            ResourceDemand = new Resources();

            AmountNeeded = new StringArray(Resource.ResourceTypeSize());
            LifeValues =  new StringArray(Resource.ResourceTypeSize());

            SetDemand(ResourceTypes.GetResourceType("Fruit"), 1, 100);
            SetDemand(ResourceTypes.GetResourceType("Cloth"), 1, 10);

            TotalLifleValue = 100 + 10;
        }

        public double GetLifeValueAdjusted(ResourceType resourceType) {
            return Convert.ToDouble(LifeValues[resourceType.Id]) / TotalLifleValue;
        }

        public double GetAmountNeeded(ResourceType resourceType) {
            return Convert.ToDouble(AmountNeeded[resourceType.Id]);
        }


        public double GetDemand(ResourceType resourceType) {
            return ResourceDemand.GetAmount(resourceType);
        }




        public void SetDemand(ResourceType resourceType, double amountNeeded, double lifeValue) {
            AmountNeeded[resourceType.Id] = amountNeeded.ToString();
            LifeValues[resourceType.Id] = lifeValue.ToString();
        }

        public void UpdateDemand(InternalMarket market, Population population) {
            ResourceDemand.Reset();

            var lifePriceRatioMap = new Dictionary<ResourceType, double>();
            foreach (ResourceType resourceType in ResourceTypes.resourceTypes) {
                if (GetAmountNeeded(resourceType) == 0) {
                    continue;
                }
                lifePriceRatioMap[resourceType] = Convert.ToDouble(LifeValues[resourceType.Id]) / market.GetPrice(resourceType);
            }
            ComputeWhatToBuy(market, population, lifePriceRatioMap);
            market.AddDemand(ResourceDemand);
        }


        private void ComputeWhatToBuy(InternalMarket market, Population population, Dictionary<ResourceType, double> lifePriceRatioMap) {
            double money = population.money;
            IEnumerable<KeyValuePair<ResourceType, double>> orderedLifePriceRatio = lifePriceRatioMap.OrderByDescending(ratio => ratio.Value);
            foreach (KeyValuePair<ResourceType, double> resoucreTypeRatio in orderedLifePriceRatio) {
                ResourceType resourceType = resoucreTypeRatio.Key;
                double price = market.GetPrice(resourceType);
                double buyAmount = Math.Min(money / price, population.GetPopLevel() * GetAmountNeeded(resourceType));
                ResourceDemand.SetResource(new Resource(resourceType, buyAmount));
                money -= buyAmount * price;
                if (money <= 0) {
                    break;
                }
            }
        }
    }
}
