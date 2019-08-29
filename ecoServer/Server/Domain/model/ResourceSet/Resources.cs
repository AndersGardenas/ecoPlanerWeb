using System;
using System.Collections;
using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class Resources : IEnumerable
    {

        public int Id { get; set; }

        public virtual List<Resource> resources { get; set; }

        public Resources()
        {
        }

        public Resources Init()
        {
            resources = new List<Resource>();
            foreach (ResourceTypes.ResourceType resourceType in ResourceTypes.GetIterator())
            {
                resources.Add(new Resource(resourceType, 0));
            }
            return this;
        }

        public double GetAmount(ResourceTypes.ResourceType resourceType)
        {
            return GetResource(resourceType).Amount;
        }

        public void Adjust(PrimitivResource resource)
        {
            GetResource(resource.ResourceType).Adjust(resource.Amount);
        }


        public IEnumerator GetEnumerator()
        {
            return resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Resource GetResource(ResourceTypes.ResourceType producingType)
        {
            return resources.Find(r => r.ResourceType.Equals(producingType));
        }

        public void SetResource(PrimitivResource resourceDemand)
        {
            SetResource(resourceDemand.ResourceType, resourceDemand.Amount);
        }

        public void SetResource(ResourceTypes.ResourceType resourceType, double amount)
        {
            GetResource(resourceType).Amount = amount;
        }

        public void Reset()
        {
            resources.ForEach(r => r.Amount = 0);
        }
    }
}
