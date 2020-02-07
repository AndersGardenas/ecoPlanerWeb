using econoomic_planer_X.Market;
using Server.Server.Domain.model.ResourceSet;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternatlTradingResource : TradingResource
    {
        public virtual Destination Destination { get; set; }

        public ExternatlTradingResource() { }

        public ExternatlTradingResource(Population Owner, ResourceTypes.ResourceType resourceType, double Amount) :
            base(Owner, resourceType, Amount)
        {
        }

        public ExternatlTradingResource Init(ExternalMarket destination, double daysRemaning)
        {
            Destination = new Destination(destination, daysRemaning);
            return this;
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

        public new bool Empty()
        {
            if (Amount <= 0)
            {
                Destination = null;
                Owner = null;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
