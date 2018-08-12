using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static int Amount()
        {
            return ResourceType.Amount;
        }
        public static void AddResourceType(String name)
        {
            resourceTypes.Add(new ResourceType(name));
        }

        public static ResourceType GetResourceType(String name)
        {
            return resourceTypes.First(re => re.Name.CompareTo(name) == 0);
        }
    }
}
