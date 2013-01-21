using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTMXObjects
{
    public enum Orientation : int
    {
        Orthogonal = 0,
        Isometric = 1
    }

    /// <summary>
    /// The Map Object. 
    /// </summary>
    public class Map
    {
        public List<LayerBase> LayerBaseList { get; set; }                 //list of the layers

        public string Version { get; set; }             // The TMX version number
        public Orientation Orientation { get; set; }    // Orientation. Tiled supports "orthogonal" and "isometric".
        public int Width { get; set; }                  // The number of tiles horizontally
        public int Height { get; set; }                 // The number of tiles vertically
        public int TileWidth { get; set; }              // The width of a tile.
        public int TileHeight { get; set; }             // The height of a tile

        /// <summary>
        /// The maximum width of the map in pixels
        /// </summary>
        public int MaxWidthPx
        {
            get
            {
               return Width * TileWidth;
            }
        }

        /// <summary>
        /// The maximum height of the map in pixels
        /// </summary>
        public int MaxHeightPx
        {
            get
            {
                return Height * TileHeight;

            }
        }

        public Map()
        {           
            LayerBaseList = new List<LayerBase>();
            Orientation = Orientation.Orthogonal;
        }
    }
}
