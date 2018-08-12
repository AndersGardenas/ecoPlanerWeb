using econoomic_planer_X.ResourceSet;
using System;
using System.Collections.Generic;


namespace econoomic_planer_X
{
    class Program
    {
        static void StartUp(string[] args)
        {
            Console.WriteLine("Hello World!");
            ResourceTypes.Init();
            int iter = 0;
            Region op = new Region(true);
            Region op2 = new Region(false);
            var regions = new List<Region>();
            regions.Add(op);
            regions.Add(op2);
            op.Connect(op2);

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
