using BL.IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace IBL
    {
        interface IBL
        {
            public Station AddStation(int id, int name, Location location, int AvailableStations);
        }
    }
}
