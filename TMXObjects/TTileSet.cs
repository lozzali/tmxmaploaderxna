using System.Collections.Generic;

namespace TMXObjects
{
    /// <summary>
    /// Hold the image property of the image used in the tileset
    /// </summary>
    public class TImage
    {
        public string Source { get; set; }  //The reference to the tileset image file
        public int Width { get; set; }      //width of the image in pixels
        public int Height { get; set; }     //height of the image in pixels
        public string Transparancy { get; set; }    //the transparancy value in string format
    }

    public class TTileSet
    {
        public int FirstGID { get; set; }       //The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
        public string Source { get; set; }      //If this tileset is stored in an external TSX (Tile Set XML) file, 
                                                //this attribute refers to that file. That TSX file has the same structure as the attribute 
                                                //as described here. (There is the firstgid attribute missing and this source attribute is also not there. 
                                                //These two attributes are kept in the TMX map, since they are map specific.)

        public string Name { get; set; }        //The name of this tileset.
        public int TileWidth { get; set; }      //the tile width in pixels
        public int TileHeight { get; set; }     //the tile height in pixels
        public int Spacing { get; set; }        //spacing value of the tiles in the tileset
        public int Margin { get; set; }         //the margin value of the tiles
        public int TileOffsetX { get; set; }    //Horizontal offset in pixels 
        public int TileOffsetY { get; set; }    //Vertical offset in pixels

        public TImage Image { get; set; }
        public Dictionary<int, TProperties> TilePropertyList { get; set; }

        public TTileSet()
        {
            Image = new TImage();
            TilePropertyList = new Dictionary<int, TProperties>();
        }

    }
}
