
using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;

namespace econoomic_planer_X
{
    public class Region
    {
        InternalMarket internalMarket;

        internal void Connect(Region op2)
        {
            Negbours.Add(op2);
            op2.Negbours.Add(this);
        }

        public List<Population> populations = new List<Population>();
        List<Factory> facties = new List<Factory>();
        public List<Region> Negbours = new List<Region>();
        Resources producedResources = new Resources();

        public List<Region> Negbours1 { get => Negbours; set => Negbours = value; }

        public Region(Boolean fruits)
        {
            internalMarket = new InternalMarket(this);
            if (fruits){
                populations.Add(new Farmer(10000,ResourceTypes.GetResourceType("Fruit")));
            }
            else
            {
            populations.Add(new Farmer(10000,ResourceTypes.GetResourceType("Cloth")));

            }
        }


        public double GetTransportCost()
        {
            return GetTransportTime()*0;
        }

        public double GetTransportTime()
        {
            return 1;
        }

        public double GetResorceCost(ResourceType resource)
        {
            return internalMarket.GetPrice(resource);
        }



        public Population GetBestPayed()
        {
            double bestSallery = -1; 
            Population bestPop = null;
            
            foreach(Population pop in populations)
            {
                double sallery = pop.GetSallery(internalMarket);
                if (sallery > bestSallery){
                    bestPop = pop;
                    bestSallery = sallery;
                }
            }
            return bestPop;
        }

        public ExternalMarket GetExternalMarket()
        {
            return internalMarket.externalMarket;
        }

        
        public void UpdatePopulation()
        {
            double newPop = 0;
            foreach(Population pop in populations)
            {
                newPop += pop.UpdatePopulation();
            }
            Population bestPop = GetBestPayed();
            bestPop.ChangePop(newPop);
        }

        public void Update()
        {
            internalMarket.UpdateMarket();

            internalMarket.ComputeNewStock();
            internalMarket.DoTrade();
            
            UpdatePopulation();


        }

        public void CleanUp()
        {
            internalMarket.CleanUp();
            foreach(Population pop in populations)
            {   
                pop.Print();
            }   
        }

    }
}

