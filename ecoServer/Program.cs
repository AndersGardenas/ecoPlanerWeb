using econoomic_planer_X;
using econoomic_planer_X.ResourceSet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace ecoPlanerWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Init();
        }


        public static void Init() {
            Console.WriteLine("Hello World!");
            ResourceTypes.Init();

            var op = new Region(true);
            var op2 = new Region(false);
            var regions = new List<Region> {
                op,
                op2
            };
            op.ConnectNeighbour(op2);

            int iter = 0;
            while (true)
            {
                Console.WriteLine("------------------------New gen------------------------------------" + iter++);
                regions.ForEach(re => re.Update());

                regions.ForEach(re => re.CleanUp());
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
