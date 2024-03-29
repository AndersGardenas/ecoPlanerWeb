﻿using System;

namespace econoomic_planer_X.ResourceSet
{
    public class ResourceTypes
    {
        static ResourceType[] types = (ResourceType[]) Enum.GetValues(typeof(ResourceType));
        public enum ResourceType
        {
            Fruit, Cloth
        };

        public static int TotalAmount()
        {
            return Enum.GetNames(typeof(ResourceType)).Length;
        }

        public static ResourceType[] GetIterator()
        {
            return types;
        }
    }
}
