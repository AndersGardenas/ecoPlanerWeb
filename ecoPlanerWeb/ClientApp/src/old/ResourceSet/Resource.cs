
namespace econoomic_planer_X.ResourceSet
{


    public class Resource
    {
        public ResourceType ResourceType {get; set; }
        public double Amount {get;set; }

        public bool Adjust(double value) {
            Amount += value;
            return value >= 0;
        }


        public Resource(ResourceType resourceType, double Amount)
        {
            this.ResourceType = resourceType;
            this.Amount = Amount;
        }

        public static int ResourceTypeSize()
        {
            return ResourceTypes.Amount();
        }
           
    }
}
