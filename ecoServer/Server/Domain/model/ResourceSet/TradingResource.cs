using econoomic_planer_X.Market;
using System;

namespace econoomic_planer_X.ResourceSet
{
    public class TradingResource : Resource, IComparable, IEquatable<TradingResource>
    {

        public virtual Population Owner { get; set; }

        public TradingResource() { }

        public TradingResource(Population Owner, ResourceTypes.ResourceType resourceType, double Amount) : base(resourceType, Amount)
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

        public void Trade(double ratio, double price, ResourceTypes.ResourceType resourceType)
        {
            Owner.Trade(ratio * Amount * price, new PrimitivResource(resourceType, -ratio * Amount));
            Amount -= ratio * Amount;
        }

        public bool Empty()
        {
            return Amount <= 0;
        }

        public int CompareTo(TradingResource tr)
        {
            if (Owner.ID.CompareTo(tr.Owner.ID) == 0 && ResourceType.Equals(tr.ResourceType))
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
            Amount += su.Amount;
        }

        public bool Equals(TradingResource other)
        {
            return CompareTo(other) == 0;
        }

        internal ExternatlTradingResource SplitExternal(double ratio, object getExternalMarket, double v)
        {
            throw new NotImplementedException();
        }
    }
}
