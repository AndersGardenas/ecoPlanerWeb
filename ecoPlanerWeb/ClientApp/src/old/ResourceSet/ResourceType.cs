using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace econoomic_planer_X.ResourceSet
{
    public class ResourceType: IComparable, IEquatable<ResourceType>
    {
        public int GuId {get; set; }
        public static int  Amount = 0;
        public String Name  {get; set; }

        public ResourceType(String Name)
        {
            GuId = Amount;
            Amount++;
            this.Name = Name;
        }

        public int CompareTo(ResourceType obj)
        {
            return GuId.CompareTo(obj.GuId); 
        }

        public int CompareTo(object obj)
        {
            return  CompareTo((ResourceType)obj); 
        }

        public bool Equals(ResourceType other)
        {
            return CompareTo(other) == 0;
        }
    }
}
