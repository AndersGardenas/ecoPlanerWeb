using econoomic_planer_X;
using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;

namespace ecoPlanerWeb
{
    public class Program
    {
        public static void Main()
        {
            Init(null);
        }


        public static void Init(List<Contry> contries) {
            Console.WriteLine("Hello World!");
            ResourceTypes.Init();



            int iter = 0;
            while (true)
            {
                Console.WriteLine("------------------------New gen------------------------------------" + iter++);
                contries.ForEach(c => c.Update());
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
