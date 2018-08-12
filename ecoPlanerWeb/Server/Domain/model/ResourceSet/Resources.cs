using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace econoomic_planer_X.ResourceSet
{
    public class Resources  : IEnumerable
    {
        [Key]
        String Guid ;

        Resource[] resources =  new Resource[Resource.ResourceTypeSize()];

        public Resources()
        {
           foreach(ResourceType resourceType in ResourceTypes.resourceTypes)
            {
                resources[resourceType.Id] = new Resource(resourceType, 0);
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
           Array.ForEach(resources, r => r.Amount = 0);
        }
    }
}
