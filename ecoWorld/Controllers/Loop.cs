
using econoomic_planer_X;
using econoomic_planer_X.ResourceSet;
using Microsoft.AspNetCore.Mvc;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace ecoPlanerWeb
{
    [Route("api2/[controller]")]
    public class Loop : Controller
    {
        protected EcoContext context;

        public Loop(EcoContext context)
        {
            this.context = context;
        }

        public void Init()
        {
            Console.WriteLine("Hello World!");
            ResourceTypes.Init();
            IEnumerable<Contry> contries = context.Contry;


            int iter = 0;
            while (true)
            {
                Console.WriteLine("------------------------New gen------------------------------------" + iter++);
                foreach (Contry contry in contries)
                {
                    contry.Update();
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
