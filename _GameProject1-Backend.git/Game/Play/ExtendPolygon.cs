using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Regulus.CustomType;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class ExtendPolygon
    {
        public readonly Polygon Result;
        

        public ExtendPolygon(Polygon polygon, float extend)
        {
            var newPoints = ExtendPolygon._GetPoints(polygon, extend);
            Result = new Polygon(newPoints.ToArray());
        }

        private static IEnumerable<Vector2> _GetPoints(Polygon polygon, float extend)
        {
            var center = polygon.Center;
            var points = polygon.Points;
            
            var newPoints = from p in points
                            let unit = ((center - p).GetNormalized() * extend)
                            select  p - unit;
            return newPoints;
        }
    }
}
