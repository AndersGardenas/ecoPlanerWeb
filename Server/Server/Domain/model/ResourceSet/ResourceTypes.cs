using System;
using System.Collections.Generic;
using System.Linq;

namespace econoomic_planer_X.ResourceSet
{
    public class ResourceTypes  
    {
        public static List<ResourceType> resourceTypes = new List<ResourceType>();

        public static void Init()
        {
            resourceTypes.Add(new ResourceType("Fruit"));
            resourceTypes.Add(new ResourceType("Cloth"));
        }

        public static int TotalAmount()
        {
            return ResourceType.totalAmount;
        }
        public static void AddResourceType(string name)
        {
            resourceTypes.Add(new ResourceType(name));
        }

        public static ResourceType GetResourceType(string name)
        {
            return resourceTypes.First(re => re.Name.CompareTo(name) == 0);
        }
    }
}
