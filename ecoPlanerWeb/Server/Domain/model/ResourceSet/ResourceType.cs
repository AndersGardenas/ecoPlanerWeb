using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace econoomic_planer_X.ResourceSet
{
    public class ResourceType: IComparable, IEquatable<ResourceType>
    {
        [Key]
        public int Id {get; set; }
        public static int  Amount = 0;
        public String Name  {get; set; }

        public ResourceType(String Name)
        {
            Id = Amount;
            Amount++;
            this.Name = Name;
        }

        public int CompareTo(ResourceType obj)
        {
            return Id.CompareTo(obj.Id); 
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
