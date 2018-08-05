
using econoomic_planer_X.ResourceSet;
using System.Collections.Generic;

namespace econoomic_planer_X
{

    class Factory
    {
        List<Population> pops = new List<Population>();
        List<Resource> ResourcesDemand = new List<Resource>();
        List<Resource> ResourcesSales = new List<Resource>();

        ResourceType resourceType;

        public Factory(ResourceType resourceType)
        {
            this.resourceType = resourceType;
        }

        public Resource Update()
        {
            int sum = 0; 
            foreach(Population pop in pops)
            {
           //     sum += pop.;
            }
            return new Resource(resourceType,sum);
        }
        
    }
}
