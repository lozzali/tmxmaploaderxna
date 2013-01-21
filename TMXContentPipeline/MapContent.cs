using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace TMXContentPipeline
{
    public enum Orientation : int
    {
        Orthogonal = 0,
        Isometric = 1
    }

    [ContentSerializerRuntimeType("GameTMXObjects.Map, GameTMXObjects")]
    public class MapContent
    {
        public List<TileSetContent> TileSetList { get; set; }             //list of the tilesets
        public List<LayerBaseContent> LayerBaseList { get; set; }                 //list of the layers        

        public string Version { get; set; }             // The TMX version number
        public int Orientation { get; set; }    // Orientation. Tiled supports "orthogonal" and "isometric".
        public int Width { get; set; }                  // The number of tiles horizontally
        public int Height { get; set; }                 // The number of tiles vertically
        public int TileWidth { get; set; }              // The width of a tile.
        public int TileHeight { get; set; }             // The height of a tile

        public MapContent()
        {
            TileSetList = new List<TileSetContent>();
            LayerBaseList = new List<LayerBaseContent>();            
            Orientation = (int)TMXContentPipeline.Orientation.Orthogonal;
        }
    }
}
