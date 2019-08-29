
using econoomic_planer_X.Market;
using econoomic_planer_X.PopulationTypes;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;
using System.Windows;

namespace econoomic_planer_X
{
    public class Region
    {
        public int ID { get; set; }

        public virtual InternalMarket InternalMarket { get; set; }
        public virtual ExternalMarket ExternalMarket { get; set; }
        public virtual List<Population> Populations { get; set; }
        public virtual List<NeighbourRegion> Negbours { get; set; }

        private readonly List<Point> Polygon;
        public double CenterX { get; set; }
        public double CenterY { get; set; }

        public int ContryID { get; set; }


        public Region()
        {
        }

        public Region(bool fruits, int population, List<Point> polygon, Point center) : this()
        {
            Negbours = new List<NeighbourRegion>();
            InternalMarket = new InternalMarket();
            InternalMarket.Init();
            ExternalMarket = new ExternalMarket(this);
            Populations = new List<Population>();

            Polygon = polygon;
            CenterX = center.X;
            CenterY = center.Y;

            if (fruits)
            {
                Populations.Add(new Farmer(population, ResourceTypes.ResourceType.Fruit));
            }
            else
            {
                Populations.Add(new Farmer(population, ResourceTypes.ResourceType.Cloth));
            }
        }

        private void AddNeighbour(Region neighbour)
        {
            Negbours.Add(new NeighbourRegion(this, neighbour));
            ExternalMarket.AddNeighbour(neighbour);
        }

        public void ConnectNeighbour(Region neighbour)
        {
            AddNeighbour(neighbour);
            neighbour.AddNeighbour(this);
        }

        public List<Point> GetPolygon()
        {
            return Polygon;
        }

        public double GetTransportCost()
        {
            return GetTransportTime();
        }

        public double GetTransportTime()
        {
            return 0;
        }

        public double GetResorceCost(ResourceTypes.ResourceType resource)
        {
            return InternalMarket.GetPrice(resource);
        }

        public Population GetBestPayed()
        {
            double bestSallery = -1;
            Population bestPop = null;

            foreach (Population pop in Populations)
            {
                double sallery = pop.GetSallery(InternalMarket);
                if (sallery > bestSallery)
                {
                    bestPop = pop;
                    bestSallery = sallery;
                }
            }
            return bestPop;
        }

        public ExternalMarket GetExternalMarket()
        {
            return ExternalMarket;
        }


        public void UpdatePopulation()
        {
            double newPop = 0;
            foreach (Population pop in Populations)
            {
                newPop += pop.UpdatePopulation();
            }
            Population bestPop = GetBestPayed();
            bestPop.ChangePop(newPop);
        }

        public void Update()
        {
            InternalMarket.UpdateMarket(Populations);
            InternalMarket.ComputeNewStock(ExternalMarket);
            InternalMarket.DoTrade(Populations);

            UpdatePopulation();
            CleanUp();
        }

        public void CleanUp()
        {
            ExternalMarket.FinilizeTrades();
            InternalMarket.CleanUp();
            foreach (Population pop in Populations)
            {
                //pop.Print();
            }
        }
    }
}

