using System;
using System.Collections;
using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class Resources : IEnumerable
    {

        public Guid Id { get; set; }

        public virtual List<Resource> resources { get; set; }

        public Resources()
        {
            resources = new List<Resource>();

            foreach (ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                resources.Add(new Resource(resourceType, 0));
            }
        }

        public double GetAmount(ResourceType resourceType)
        {
            return resources[resourceType.Id].Amount;
        }

        public void Adjust(Resource resource)
        {
            resources[resource.ResourceType.Id].Adjust(resource.Amount);
        }


        public IEnumerator GetEnumerator()
        {
            return resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Resource GetResource(ResourceType producingType)
        {
            return resources[producingType.Id];
        }

        public void SetResource(Resource resourceDemand)
        {
            resources[resourceDemand.ResourceType.Id].Amount = resourceDemand.Amount;
        }

        public void Reset()
        {
            resources.ForEach(r => r.Amount = 0);
        }
    }
}
