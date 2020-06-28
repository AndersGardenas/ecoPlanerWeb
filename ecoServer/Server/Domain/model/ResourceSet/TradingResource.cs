using econoomic_planer_X.Market;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace econoomic_planer_X.ResourceSet
{
    public class TradingResource: IComparer<TradingResource>
    {
        public int TradingResourceId { get; set; }
        public virtual Population Owner { get; set; }

        public virtual ResourceTypes.ResourceType ResourceType { get; set; }
        public double Amount { get; set; }

        [ForeignKey("TradingResources")]
        public int? TradingResourcesID { get; set; }
        [ForeignKey("ExternalTradingResources")]
        public int? ExternalTradingResourcesID { get; set; }


        public TradingResource() { }


        public TradingResource(Population Owner, ResourceTypes.ResourceType resourceType, double Amount)
        {
            ResourceType = resourceType;
            this.Amount = Amount;
            this.Owner = Owner;
        }

        public ExternatlTradingResource SplitExternal(double ratio, ExternalMarket destination, double localTravelTime)
        {
            double splitAmount = Amount * ratio;
            Amount -= splitAmount;
            return new ExternatlTradingResource(Owner, ResourceType, splitAmount).Init(destination, localTravelTime);
        }

        public ExternatlTradingResource SplitAmountExternal(double amount, ExternalMarket destination, double localTravelTime)
        {
            double splitAmount = amount;
            Amount -= splitAmount;
            return new ExternatlTradingResource(Owner, ResourceType, splitAmount).Init(destination, localTravelTime);
        }

        public bool AffordTransport(double tradeRegionRatio, double amount)
        {
            return (tradeRegionRatio * amount > Owner.GetIntegerPopLevel() * 0.1) && Owner.AffordTransport();
        }

        public void Trade(double ratio, double price, ResourceTypes.ResourceType resourceType)
        {
            Owner.Trade(ratio * Amount * price, new PrimitivResource(resourceType, -ratio * Amount));
            Amount -= ratio * Amount;
        }



        public bool Empty()
        {
            if (Amount <= 0)
            {
                Owner = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CompareTo(TradingResource tr)
        {
            return Amount.CompareTo(tr.Amount);
        }

        internal void Add(TradingResource su)
        {
            Amount += su.Amount;
        }

        public bool Adjust(double value)
        {
            Amount += value;
            return value >= 0;
        }

        public int Compare([AllowNull] TradingResource x, [AllowNull] TradingResource y)
        {
            return  x.CompareTo(y);
        }
    }
}
