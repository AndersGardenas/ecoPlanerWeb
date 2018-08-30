using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;

namespace econoomic_planer_X.Market
{
    public class ExternalMarket
    {

        public Guid ID { get; set; }
        public Region Ownregion { get; set; }

        public virtual List<Region> BorderRegions { get; set; }
        public virtual List<ExternatlTradingResource> ExternatlTradingResources { get; set; }
        public virtual List<ExternatlTradingResource> NewExternatlTradingResources { get; set; }


        public ExternalMarket() {
            ExternatlTradingResources = new List<ExternatlTradingResource>();
            NewExternatlTradingResources = new List<ExternatlTradingResource>();
        }

        public ExternalMarket(Region Ownregion, List<Region> BorderRegions) {
            ExternatlTradingResources = new List<ExternatlTradingResource>();
            NewExternatlTradingResources = new List<ExternatlTradingResource>();

            this.BorderRegions = BorderRegions;
            this.Ownregion = Ownregion;
        }


        public double GetBestCost(ResourceType resourceType, out ExternalMarket bestRegion) {
            double bestCost = 0;
            bestRegion = null;
            double regionalTransportCost = Ownregion.GetTransportCost() / 2;

            foreach (Region region in BorderRegions) {
                double externalTransportCost = region.GetTransportCost() / 2;
                double totalTransportCost = regionalTransportCost + externalTransportCost;
                double resourceCost = region.GetResorceCost(resourceType);
                double salesCost = resourceCost - totalTransportCost;
                if (salesCost > bestCost) {
                    bestCost = salesCost;
                    bestRegion = region.GetExternalMarket();
                }
            }

            return bestCost;
        }

        public void StartTrade(List<TradingResource> TradingResources, double ratio, ExternalMarket destination) {
            foreach (TradingResource tradingResounce in TradingResources) {
                if (tradingResounce.AffordTransport()) {
                    ExternatlTradingResources.Add(tradingResounce.SplitExternal(ratio, destination, Ownregion.GetTransportTime() / 2));
                }
            }
        }


        public void UpdateTrade(List<TradingResources> TradingResources) {
            foreach (ExternatlTradingResource tradingResounce in ExternatlTradingResources) {
                tradingResounce.TransportADay();
                if (tradingResounce.AtDestination()) {
                    if (tradingResounce.getDestination().ID == ID) {
                        TradingResources[tradingResounce.ResourceType.Id].Add(tradingResounce);
                    } else {
                        tradingResounce.getDestination().AddResource(tradingResounce);
                    }
                }
            }
            ExternatlTradingResources.RemoveAll(ex => ex.AtDestination());
        }


        private void AddResource(ExternatlTradingResource tradingResounce) {
            NewExternatlTradingResources.Add(tradingResounce);
        }

        public void FinilizeTrades() {
            foreach (ExternatlTradingResource newExternatlTradingResource in NewExternatlTradingResources) {
                newExternatlTradingResource.SetDaysRemaning(Ownregion.GetTransportTime() / 2);
                ExternatlTradingResources.Add(newExternatlTradingResource);
            }
            NewExternatlTradingResources.Clear();
        }
    }
}

