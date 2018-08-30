using System;
using econoomic_planer_X.Market;
using Server.Server.Domain.model.ResourceSet;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternatlTradingResource : TradingResource
    {
        private Destination destination;

        public ExternatlTradingResource() { }

        public ExternatlTradingResource(Population Owner, ResourceType resourceType, ExternalMarket destination, double Amount, double daysRemaning) : base(Owner, resourceType, Amount) {
            this.destination = new Destination(destination, daysRemaning);
        }

        public ExternalMarket getDestination() {
            return destination.MarketDestination;
        }

        public void TransportADay() {
            destination.TransportADay();
        }

        public bool AtDestination() {
            return destination.AtDestination();
        }

        public void SetDaysRemaning(double newTime) {
            destination.DaysRemaning = newTime;
        }
    }
}
