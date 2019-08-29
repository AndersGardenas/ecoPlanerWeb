using System;

namespace econoomic_planer_X.ResourceSet
{


    public struct PrimitivResource
    {
        public ResourceTypes.ResourceType ResourceType { get; set; }
        public double Amount { get; set; }


        public PrimitivResource(ResourceTypes.ResourceType ResourceType, double Amount)
        {
            this.ResourceType = ResourceType;
            this.Amount = Amount;
        }

        public static int ResourceTypeSize()
        {
            return ResourceTypes.TotalAmount();
        }

        public bool Adjust(double value)
        {
            Amount += value;
            return value >= 0;
        }

    }
}
