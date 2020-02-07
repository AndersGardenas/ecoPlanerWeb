using System.Collections;
using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class Resources : IEnumerable
    {

        public int ResourcesId { get; set; }

        public virtual List<Resource> resources { get; set; }

        public Resources()
        {
        }

        public Resources Init()
        {
            resources = new List<Resource>();
            return this;
        }

        public double GetAmount(ResourceTypes.ResourceType resourceType)
        {
            if (ResoucreExist(resourceType) == false) return 0;
            return GetResource(resourceType).Amount;
        }

        public void Adjust(PrimitivResource resource)
        {
            if (resource.Amount == 0) return;
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

        public bool ResoucreExist(ResourceTypes.ResourceType producingTyp)
        {
            return resources.Exists(r => r.ResourceType.Equals(producingTyp));
        }

        public Resource GetResource(ResourceTypes.ResourceType producingType)
        {
            Resource r = resources.Find(r => r.ResourceType.Equals(producingType));
            if (r != null)
            {
                return r;
            }
            r = new Resource(producingType, 0);
            resources.Add(r);
            return r;
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
