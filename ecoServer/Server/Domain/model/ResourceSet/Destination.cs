using econoomic_planer_X.Market;

namespace Server.Server.Domain.model.ResourceSet
{
    public class Destination
    {
        public int Id { get; set; }
        public double DaysRemaning { get; set; }
        public virtual ExternalMarket MarketDestination { get; set;}


        public Destination() { }
        public Destination(ExternalMarket destination, double daysRemaning)
        {
            DaysRemaning = daysRemaning;
            MarketDestination = destination;
        }

        public void TransportADay()
        {
            DaysRemaning -= 1;
        }

        public bool AtDestination()
        {
            return DaysRemaning <= 0;
        }
    }
}
