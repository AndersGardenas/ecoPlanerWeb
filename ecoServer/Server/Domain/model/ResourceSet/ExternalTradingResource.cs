using econoomic_planer_X.Market;
using Server.Server.Domain.model.ResourceSet;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternatlTradingResource : TradingResource
    {
        public virtual Destination Destination { get; set; }

        public ExternatlTradingResource() { }

        public ExternatlTradingResource(Population Owner, ResourceTypes.ResourceType resourceType, ExternalMarket destination, double Amount, double daysRemaning) :
            base(Owner, resourceType, Amount)
        {
            Destination = new Destination(destination, daysRemaning);
        }

        public ExternalMarket getDestination()
        {
            return Destination.MarketDestination;
        }

        public void TransportADay()
        {
            Destination.TransportADay();
        }

        public bool AtDestination()
        {
            return Destination.AtDestination();
        }

        public void SetDaysRemaning(double newTime)
        {
            Destination.DaysRemaning = newTime;
        }
    }
}
