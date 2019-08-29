using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace econoomic_planer_X
{
    public abstract class Population
    {

        public virtual ResourceTypes.ResourceType ProducingType { get; set; }
        public virtual Resources Stock { get; set; }
        public double Money { get; set; }
        public const double startMoney = 5;
        double selling = 0;
        public int ID { get; set; }
        public double PopLevel { get; set; }
        public double FoodLevel { get; set; }

        [ForeignKey("Standard")]
        public int RegionID { get; set; }
        public virtual Region Region { get; set; }

        public Population() { }

        public Population(int Amount, ResourceTypes.ResourceType producingType)
        {
            Stock = new Resources().Init();
            SetPopLevel(Amount);
            Money = Amount * startMoney;
            this.ProducingType = producingType;
        }


        public double GetPopLevel()
        {
            return (int)PopLevel;
        }
        public void SetPopLevel(double value)
        {
            PopLevel = value;
        }


        public void UpdateDemand(InternalMarket market)
        {
            Demand.UpdateDemand(market, this);
        }

        public bool AffordTransport()
        {
            return Money > PopLevel;
        }

        public void Trade(double money, PrimitivResource resource)
        {
            Money += money;
            Stock.Adjust(resource);
        }


        public virtual double GetEfficensy()
        {
            return 1.05;
        }


        public double GetSallery(InternalMarket market)
        {
            return GetEfficensy() * market.GetPrice(ProducingType);
        }

        public void Consume()
        {
            FoodLevel = 0;

            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {

                double neededAmount = Demand.GetDemand(resourceType);
                if (neededAmount == 0)
                {
                    continue;
                }

                double amountInStock = Stock.GetAmount(resourceType);

                double ratio = Math.Min(1, amountInStock / neededAmount);
                FoodLevel += ratio * Demand.GetLifeValueAdjusted(resourceType);
                Stock.Adjust(new PrimitivResource(resourceType, -ratio * neededAmount));
            }
        }

        public void ChangePop(double newPop)
        {
            PopLevel = Math.Max(newPop + PopLevel, 0);
        }

        public double UpdatePopulation()
        {
            Consume();
            double popChange = GetPopLevel() * (FoodLevel - 0.5) / 100;
            if (popChange < 0)
            {
                ChangePop(popChange);
                return 0;
            }
            return popChange;
        }

        public double SellingAmount()
        {
            return selling;
        }


        public Resource Selling()
        {
            double stockAmount = Stock.GetAmount(ProducingType);
            Stock.SetResource(new PrimitivResource(ProducingType, 0));
            return new Resource(ProducingType, stockAmount);
        }

        public void Produce()
        {
            double produce = GetPopLevel() * GetEfficensy();
            var resource = new PrimitivResource(ProducingType, produce);
            Stock.Adjust(resource);
            selling = Stock.GetAmount(ProducingType);
        }

        public void BuyAmount(double ratio, double price, ResourceTypes.ResourceType resourceType)
        {
            double buyAmount = Demand.GetDemand(resourceType) * ratio;
            Stock.Adjust(new PrimitivResource(resourceType, buyAmount));
            Money -= buyAmount * price;
        }
    }
}
