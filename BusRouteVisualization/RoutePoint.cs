using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusRouteVisualization
{
    public class RoutePoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; } // координата X на карте
        public int Y { get; set; } // координата Y на карте
        public TimeSpan TimeFromStart { get; set; } // время от начала маршрута
    }
}