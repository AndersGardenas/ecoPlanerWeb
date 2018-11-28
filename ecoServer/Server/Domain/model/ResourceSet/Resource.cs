using System;

namespace econoomic_planer_X.ResourceSet
{


    public class Resource
    {
        public Guid Id { get; set; }
        public ResourceType ResourceType { get; set; }
        public double Amount { get; set; }

        public Resource(){}


        public Resource(ResourceType ResourceType, double Amount)
        {
            Id = Guid.NewGuid();
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
