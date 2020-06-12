using econoomic_planer_X;
using econoomic_planer_X.Market;
using econoomic_planer_X.ResourceSet;
using Server.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ecoServer.Server.Domain.Services
{
    public class PopulationService
    {
        readonly EcoContext Context;
        public PopulationService(EcoContext context)
        {
            Context = context;
        }


    }
}
