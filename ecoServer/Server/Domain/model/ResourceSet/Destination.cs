using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Server.Domain.model.ResourceSet
{
    public class Destination
    {
        public int Id { get; set; }
        public double DaysRemaning { get; set; }
        public virtual ExternalMarket MarketDestination { get; set;}

        [ForeignKey("ExternatlTradingResource")]
        public int ExternatlTradingResourceId { get; set; }
        public virtual ExternatlTradingResource ExternatlTradingResource { get; set; }

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
