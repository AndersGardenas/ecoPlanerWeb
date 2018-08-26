using econoomic_planer_X.Market;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternatlTradingResource : TradingResource
    {
        public double DaysRemaning { get; set; }
        public ExternalMarket Destination { get; set; }

        public ExternatlTradingResource() { }

        public ExternatlTradingResource(Population Owner, ResourceType resourceType, ExternalMarket Destination, double Amount, double DaysRemaning) : base(Owner, resourceType, Amount)
        {
            this.DaysRemaning = DaysRemaning;
            this.Destination = Destination;
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
