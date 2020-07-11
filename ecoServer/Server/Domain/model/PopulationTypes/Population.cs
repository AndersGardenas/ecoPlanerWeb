using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;

namespace econoomic_planer_X
{
    public abstract class Population
    {

        public virtual ResourceTypes.ResourceType ProducingType { get; set; }
        public int ResourcesId { get; set; }
        public virtual Resources Stock { get; set; }
        public double Money { get; set; }
        public const double startMoney = 5; 

        public double Efficensy { get; set; }

        double selling = 0;
        public int ID { get; set; }
        public double PopLevel { get; set; }
        private double FoodLevel;

        public int RegionID { get; set; }
        public virtual Region Region { get; set; }

        public Demand demand = new Demand().Init();

        public Population() { }

        public Population(int Amount, ResourceTypes.ResourceType producingType, double efficensy)
        {
            SetPopLevel(Amount);
            Money = Amount * startMoney;
            ProducingType = producingType;
            Efficensy = efficensy;
        }

        public Population Init()
        {
            Stock = new Resources().Init();

            return this;
        }


        public double GetIntegerPopLevel()
        {
            return (int)PopLevel;
        }
        public void SetPopLevel(double value)
        {
            PopLevel = value;
        }


        public void UpdateDemand(InternalMarket market)
        {
            demand.UpdateDemand(market, this);
        }

        public bool AffordTransport()
        {
            return Money > PopLevel;
        }

        public void Trade(double money)
        {
            Money += money;
        }


        public virtual double GetEfficensy()
        {
            return Efficensy;
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

                double neededAmount = demand.GetDemand(resourceType);
                if (neededAmount == 0)
                {
                    continue;
                }

                double amountInStock = Stock.GetAmount(resourceType);

                if (amountInStock == 0)
                {
                    continue;
                }

                double ratio = Math.Min(1, amountInStock / neededAmount);
                FoodLevel += ratio * demand.GetLifeValueAdjusted(resourceType);
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
            double popChange = GetIntegerPopLevel() * (FoodLevel - 0.5) / 10000;
            if (popChange < 0)
            {
                ChangePop(popChange);
                return 0;
            }
            return popChange;
        }

        public double AddToMarket()
        {
            Stock.Adjust(new PrimitivResource(ProducingType, -selling));
            return selling;
        }


        public void Produce()
        {
            double produce = Math.Round(GetIntegerPopLevel() * GetEfficensy(),3,MidpointRounding.ToZero);
            var resource = new PrimitivResource(ProducingType, produce);

            Stock.Adjust(resource);
            selling = Stock.GetAmount(ProducingType);
        }

        public void BuyAmount(double ratio, double price, ResourceTypes.ResourceType resourceType)
        {
            double buyAmount = demand.GetDemand(resourceType) * ratio;
            if (buyAmount == 0)
            {
                return;
            }
            Stock.Adjust(new PrimitivResource(resourceType, buyAmount));
            Money -= buyAmount * price;
        }
    }
}
