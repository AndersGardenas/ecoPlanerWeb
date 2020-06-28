using System;

namespace econoomic_planer_X.ResourceSet
{


    public class Resource
    {
        public int ResourceId { get; set; }

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
            if (Amount < 0)
            {
                Console.WriteLine("Negative Resource, should not be posible, Amount:" + value);
                return false;
            }
            return true;
        }

    }
}
