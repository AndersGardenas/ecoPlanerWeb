using System;

namespace econoomic_planer_X.ResourceSet
{
    public class ResourceTypes
    {
        public enum ResourceType
        {
            Fruit, Cloth
        };

        public static int TotalAmount()
        {
            return ResourceType.GetNames(typeof(ResourceType)).Length;
        }

        public static ResourceType[] GetIterator()
        {
            return (ResourceType[])Enum.GetValues(typeof(ResourceType));
        }
    }
}
