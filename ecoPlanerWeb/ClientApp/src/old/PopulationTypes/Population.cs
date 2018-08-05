﻿using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;

namespace econoomic_planer_X
{
    public abstract class Population
    {

        public ResourceType producingType;
        public Demand demand = new Demand();
        public double money;
        private const int startMoney = 5;
        double selling = 0;
        public String Id;


        
        public Population(int Amount, ResourceType producingType)
        {
            SetPopLevel(Amount);
            money = Amount * startMoney;
            this.producingType = producingType;
            Id = Guid.NewGuid().ToString("N");
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
        public double FoodLevel{get; set;}
        public Resources stock = new Resources();

        public void UpdateDemand(InternalMarket market)
        {
            demand.UpdateDemand(market,this);
        }

        public bool AffordTransport()
        {
            return money > popLevel;
        }

        public void TradeGain(double money)
        {
            this.money += money;
        }


        public virtual double getEfficensy()
        {
            return 1.05;
        }


        public double GetSallery(InternalMarket market)
        {
            return getEfficensy() * market.GetPrice(producingType);
        }

        public void Consume()
        {
             FoodLevel = 0;
  
            foreach(ResourceType resourceType in ResourceTypes.resourceTypes){

                double neededAmount = demand.GetDemand(resourceType);
                if (neededAmount == 0)
                {
                    continue;
                }

                double amountInStock = stock.GetAmount(resourceType);
 
                double ratio = Math.Min(1,amountInStock/neededAmount);
                FoodLevel += ratio * demand.GetNeedAdjusted(resourceType);
                stock.Adjust(new Resource(resourceType,-ratio*neededAmount));
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
            double popChange= GetPopLevel() *  (FoodLevel-0.5)/100;
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
            stock.SetResource(new Resource(producingType,0));
            return new Resource(producingType,stockAmount);
        }


        public void Produce()
        {
            double produce = GetPopLevel() * getEfficensy();
            Resource resource = new Resource(producingType,produce);
            stock.Adjust(resource);
            selling = stock.GetAmount(producingType);
        }    

        public double BuyAmount(double ratio, double price, ResourceType resourceType)
        {
            double buyAmount = demand.GetDemand(resourceType)* ratio;
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
