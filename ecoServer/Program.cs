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



            int iter = 0;
            while (true)
            {
                Console.WriteLine("------------------------New gen------------------------------------" + iter++);

                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
