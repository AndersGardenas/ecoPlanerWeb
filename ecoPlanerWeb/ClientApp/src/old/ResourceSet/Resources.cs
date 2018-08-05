using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace econoomic_planer_X.ResourceSet
{
    public class Resources  : IEnumerable
    {
        Resource[] resources =  new Resource[Resource.ResourceTypeSize()];

        public Resources()
        {
           foreach(ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                resources[resourceType.GuId] = new Resource(resourceType, 0);
            }
        }


        

        public double GetAmount(ResourceType resourceType)
        {
            return resources[resourceType.GuId].Amount;
        }

        public void Adjust(Resource resource)
        {
            resources[resource.ResourceType.GuId].Adjust(resource.Amount);
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
            return resources[producingType.GuId];
        }


        public void SetResource(Resource resourceDemand)
        {
            resources[resourceDemand.ResourceType.GuId].Amount = resourceDemand.Amount;
        }

        public void Reset()
        {
           Array.ForEach(resources, r => r.Amount = 0);
        }
    }
}
