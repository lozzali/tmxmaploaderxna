using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace TMXContentPipeline
{   
    /// <summary>
    /// A TileSet object 
    /// </summary>
    public class TileSetContent
    {
        public int FirstGID { get; set; }   //The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
        public string Source { get; set; }  //If this tileset is stored in an external TSX (Tile Set XML) file, this attribute refers to 
                                            //that file. That TSX file has the same structure as the attribute as described here. 
                                            //(There is the firstgid attribute missing and this source attribute is also not there. 
                                            //These two attributes are kept in the TMX map, since they are map specific.)
        public string Name { get; set; }    //The name of this tileset.
        public int TileWidth { get; set; }  // The (maximum) width of the tiles in this tileset.
        public int TileHeight { get; set; } //The (maximum) height of the tiles in this tileset.       
        public int Spacing { get; set; }    //The spacing in pixels between the tiles in this tileset (applies to the tileset image).
        public int Margin { get; set; }     //The margin around the tiles in this tileset (applies to the tileset image).
        public int TileOffsetX { get; set; }    //Horizontal offset in pixels
        public int TileOffsetY { get; set; }    //Vertical offset in pixels 
        public ExternalReference<Texture2DContent> Image { get; set; } //The reference of the tileset image
        public TileContent [] TileList { get; set; }    //An array of Tiles representing each tile in the tileset

    }
}
