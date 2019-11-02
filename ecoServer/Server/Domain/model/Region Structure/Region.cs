
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
        public int regionID { get; set; }

        public int ContryID { get; set; }

        public double CenterX { get; set; }
        public double CenterY { get; set; }

        public int InternalMarketId { get; set; }
        public virtual InternalMarket InternalMarket { get; set; }
        public virtual ExternalMarket ExternalMarket { get; set; }
        public virtual List<Population> Populations { get; set; }
        // public virtual List<Region> Negbours { get; set; }

        private readonly List<Point> Polygon;
        private float size = 0;


        public Region()
        {
        }

        public Region(List<Point> polygon, Point center) : this()
        {
            // Negbours = new List<Region>();
            Polygon = polygon;
            CenterX = center.X;
            CenterY = center.Y;
        }

        public Region Init(int population)
        {
            InternalMarket = new InternalMarket();
            InternalMarket.Init();
            ExternalMarket = new ExternalMarket(this);
            ExternalMarket.Init();
            Populations = new List<Population>();
            Random rnd = new Random();
            if (1 < rnd.Next(0,3))
            {
                Populations.Add(new Farmer(population, ResourceTypes.ResourceType.Fruit).Init());
            }
            else
            {
                Populations.Add(new Farmer(population, ResourceTypes.ResourceType.Cloth).Init());
            }
            return this;
        }

        private void AddNeighbour(NeighbourRegion neighbourRegion, Region neighbour)
        {
            // Negbours.Add(neighbour);
            ExternalMarket.AddNeighbour(neighbour);
        }

        public void ConnectNeighbour(Region neighbour)
        {
            var neighbourRegion = new NeighbourRegion(this, neighbour);
            AddNeighbour(neighbourRegion, neighbour);
            neighbour.AddNeighbour(neighbourRegion, this);
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
            return size;
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
        }
    }
}

