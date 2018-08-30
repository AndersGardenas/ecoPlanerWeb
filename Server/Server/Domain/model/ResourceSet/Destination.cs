using econoomic_planer_X.Market;
using System;

namespace Server.Server.Domain.model.ResourceSet
{
    public class Destination
    {
        public Guid Id  { get; set; }
        public double DaysRemaning { get; set; }
        public ExternalMarket MarketDestination { get; }


        public Destination() { }
        public Destination(ExternalMarket destination, double daysRemaning) {
            DaysRemaning = daysRemaning;
            MarketDestination = destination;
        }

        public void TransportADay() {
            DaysRemaning -= 1;
        }

        public bool AtDestination() {
            return DaysRemaning <= 0;
        }


    }
}
