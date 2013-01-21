using System.Collections.Generic;

namespace TMXObjects
{
    /// <summary>
    /// A class representing a point containing an x and a y co-ordinate
    /// </summary>
    public class TPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public TPoint()
        {
            X = 0;
            Y = 0;
        }

        public TPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    /// <summary>
    /// Represents a polygon. 
    /// </summary>
    public class TPolygon : TPolyBase
    {
        public TPolygon(string data)
            : base(data)
        {
            PType = TPolyType.Type_Polygon;
        }
    }

    /// <summary>
    /// Represents a polyline
    /// </summary>
    public class TPolyline : TPolyBase
    {
        public TPolyline(string data) 
            : base(data)
        {
            PType = TPolyType.Type_Polyline;
        }
    }

    /// <summary>
    /// Abstract class for all the polygon types
    /// </summary>
    public abstract class TPolyBase 
    {
        public enum TPolyType
        {
            Type_Polygon,
            Type_Polyline,
        };

        public TPolyType PType{get; set;}
        public TPoint [] Points { get; set; }

        public TPolyBase(string data)
        {
            string [] ptStr = data.Split(' ');

            Points = new TPoint[ptStr.Length];
            
            for (int i = 0; i < ptStr.Length; ++i)
            {
                string [] pt = ptStr[i].Split(',');
                Points[i] = new TPoint(int.Parse(pt[0]), int.Parse(pt[1]));
            }
            
        }

    }
}
