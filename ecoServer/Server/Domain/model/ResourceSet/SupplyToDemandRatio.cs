using econoomic_planer_X.ResourceSet;
using System;

namespace Server.Server.Domain.model.ResourceSet
{
    public class SupplyToDemandRatio
    {

        public int Id { get; set; }
        public virtual double ResourcesPrice { get; set; }
        private double supplyToDemandRatio { get; set; }
        public virtual ResourceTypes.ResourceType ResourceType { get; set; }

        public virtual int InternalMarketId { get; set; }

        public SupplyToDemandRatio()
        {
            ResourcesPrice = 1;
        }

        public SupplyToDemandRatio(ResourceTypes.ResourceType resourceType) : this()
        {
            ResourceType = resourceType;
        }

        internal double GetSupplyToDemandRatio()
        {
            return supplyToDemandRatio;
        }

        internal void SetSupplyToDemandRatio(double supplyToDemandRatio)
        {
            if (double.IsNaN(supplyToDemandRatio) || double.IsInfinity(supplyToDemandRatio))
            {
                Console.Out.Write("resourceRatio is corrupt");
                supplyToDemandRatio = 1;
            }
            this.supplyToDemandRatio = supplyToDemandRatio;
        }

        public double ComputeSupplyToDemandRatio(double demandAmount, double supplyAmount)
        {
            return demandAmount > 0 ? supplyAmount / demandAmount : double.MaxValue;
        }
    }
}
