using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;

namespace econoomic_planer_X
{
    public abstract class Population
    {

        public ResourceType producingType;
        public double money;
        public Demand demand;
        private const int startMoney = 5;
        double selling = 0;
        public Guid ID { get; set; }


        public Population(){}

        public Population(int Amount, ResourceType producingType)
        {
            demand = new Demand();

            SetPopLevel(Amount);
            money = Amount * startMoney;
            this.producingType = producingType;
        }

        private double popLevel;

        public double GetPopLevel()
        {
            return (int)popLevel;
        }
        public void SetPopLevel(double value)
        {
            popLevel = value;
        }
        public double FoodLevel { get; set; }
        public Resources stock = new Resources();

        public void UpdateDemand(InternalMarket market)
        {
            demand.UpdateDemand(market, this);
        }

        public bool AffordTransport()
        {
            return money > popLevel;
        }

        public void TradeGain(double money)
        {
            this.money += money;
        }


        public virtual double GetEfficensy()
        {
            return 1.05;
        }


        public double GetSallery(InternalMarket market)
        {
            return GetEfficensy() * market.GetPrice(producingType);
        }

        public void Consume()
        {
            FoodLevel = 0;

            foreach (ResourceType resourceType in ResourceTypes.resourceTypes)
            {

                double neededAmount = demand.GetDemand(resourceType);
                if (neededAmount == 0)
                {
                    continue;
                }

                double amountInStock = stock.GetAmount(resourceType);

                double ratio = Math.Min(1, amountInStock / neededAmount);
                FoodLevel += ratio * demand.GetLifeValueAdjusted(resourceType);
                stock.Adjust(new Resource(resourceType, -ratio * neededAmount));
            }
        }

        internal void Print()
        {
            Console.Out.WriteLine("Poplevel is: " + GetPopLevel() + " Money level is: " + money);
        }

        public void ChangePop(double newPop)
        {
            popLevel = Math.Max(newPop + popLevel, 0);

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
            double stockAmount = stock.GetAmount(producingType);
            stock.SetResource(new Resource(producingType, 0));
            return new Resource(producingType, stockAmount);
        }


        public void Produce()
        {
            double produce = GetPopLevel() * GetEfficensy();
            Resource resource = new Resource(producingType, produce);
            stock.Adjust(resource);
            selling = stock.GetAmount(producingType);
        }

        public double BuyAmount(double ratio, double price, ResourceType resourceType)
        {
            double buyAmount = demand.GetDemand(resourceType) * ratio;
            stock.Adjust(new Resource(resourceType, buyAmount));
            money -= buyAmount * price;
            return demand.GetDemand(resourceType);
        }

        public bool Producing(ResourceType resourceType)
        {
            return resourceType == producingType;
        }
    }
}
