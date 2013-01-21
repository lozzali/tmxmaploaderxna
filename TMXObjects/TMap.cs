using System.Collections.Generic;

namespace TMXObjects
{
    public enum TOrientation
    {
        Orthogonal,
        Isometric
    }

    public class TMap
    {
        public List<TTileSet> TileSetList { get; set; }             //list of the tilesets
        public List<TLayerBase> LayerBaseList { get; set; }                 //list of the layers
        
        public string Version { get; set; }             // The TMX version number
        public TOrientation Orientation { get; set; }    // Orientation. Tiled supports "orthogonal" and "isometric".
        public int Width { get; set; }                  // The number of tiles horizontally
        public int Height { get; set; }                 // The number of tiles vertically
        public int TileWidth { get; set; }              // The width of a tile.
        public int TileHeight { get; set; }             // The height of a tile

        public TMap()
        {
            TileSetList = new List<TTileSet>();
            LayerBaseList = new List<TLayerBase>();
            Orientation = TOrientation.Orthogonal;
        }
    }
}
