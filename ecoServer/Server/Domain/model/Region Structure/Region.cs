
using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;

namespace econoomic_planer_X
{
    public class Region
    {
        public Guid ID { get; set; }

        private InternalMarket internalMarket;
        private ExternalMarket externalMarket;
        public virtual List<Population> Populations { get; set; }
        public virtual List<Region> Negbours { get; set; }


        public Region() {
            Negbours = new List<Region>();
            internalMarket = new InternalMarket();
            externalMarket = new ExternalMarket(this, Negbours);
            Populations = new List<Population>();
        }


        public Region(bool fruits) {
            Negbours = new List<Region>();
            internalMarket = new InternalMarket();
            externalMarket = new ExternalMarket(this, Negbours);
            Populations = new List<Population>();

            if (fruits) {
                Populations.Add(new Farmer(10000, ResourceTypes.GetResourceType("Fruit")));
            } else {
                Populations.Add(new Farmer(10000, ResourceTypes.GetResourceType("Cloth")));
            }
        }


        internal void Connect(Region op2) {
            Negbours.Add(op2);
            op2.Negbours.Add(this);
        }



        public double GetTransportCost() {
            return GetTransportTime() * 0;
        }

        public double GetTransportTime() {
            return 1;
        }

        public double GetResorceCost(ResourceType resource) {
            return internalMarket.GetPrice(resource);
        }



        public Population GetBestPayed() {
            double bestSallery = -1;
            Population bestPop = null;

            foreach (Population pop in Populations) {
                double sallery = pop.GetSallery(internalMarket);
                if (sallery > bestSallery) {
                    bestPop = pop;
                    bestSallery = sallery;
                }
            }
            return bestPop;
        }

        public ExternalMarket GetExternalMarket() {
            return externalMarket;
        }


        public void UpdatePopulation() {
            double newPop = 0;
            foreach (Population pop in Populations) {
                newPop += pop.UpdatePopulation();
            }
            Population bestPop = GetBestPayed();
            bestPop.ChangePop(newPop);
        }

        public void Update() {
            internalMarket.UpdateMarket(Populations);

            internalMarket.ComputeNewStock(externalMarket);
            internalMarket.DoTrade(Populations);

            UpdatePopulation();


        }

        public void CleanUp() {
            externalMarket.FinilizeTrades();
            internalMarket.CleanUp();
            foreach (Population pop in Populations) {
                pop.Print();
            }
        }

    }
}

