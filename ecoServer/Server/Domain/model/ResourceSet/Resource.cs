using System;

namespace econoomic_planer_X.ResourceSet
{


    public class Resource
    {
        public int Id { get; set; }

        public virtual ResourceTypes.ResourceType ResourceType { get; set; }
        public double Amount { get; set; }

        public Resource() { }


        public Resource(ResourceTypes.ResourceType ResourceType, double Amount)
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
