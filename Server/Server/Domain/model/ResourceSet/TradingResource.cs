using econoomic_planer_X.Market;
using System;

namespace econoomic_planer_X.ResourceSet
{
    public class TradingResource : Resource, IComparable, IEquatable<TradingResource>
    {

        public Population Owner { get; set; }

        public TradingResource() { }

        public TradingResource(Population Owner, ResourceType resourceType, double Amount) : base(resourceType, Amount)
        {
            this.Owner = Owner;
        }


        public ExternatlTradingResource SplitExternal(double ratio, ExternalMarket destination, double localTravelTime)
        {
            double splitAmount = Amount * ratio;
            Amount -= splitAmount;
            return new ExternatlTradingResource(Owner, ResourceType, destination, splitAmount, localTravelTime);
        }

        public bool AffordTransport()
        {
            return Owner.AffordTransport();
        }

        public void Trade(double ratio, double price)
        {
            Owner.TradeGain(ratio * Amount * price);
            Amount -= ratio * Amount;
        }

        public bool Empty()
        {
            return Amount <= 0;
        }

        public int CompareTo(TradingResource tr)
        {
            if (Owner.ID.CompareTo(tr.Owner.ID) == 0 && ResourceType.Id == tr.ResourceType.Id)
            {
                return 0;
            }
            return -1;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((TradingResource)obj);
        }

        internal void Add(TradingResource su)
        {
            this.Amount += su.Amount;
        }

        public bool Equals(TradingResource other)
        {
            return CompareTo(other) == 0;
        }
    }
}
