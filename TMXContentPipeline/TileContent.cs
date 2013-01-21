using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework;

namespace TMXContentPipeline
{
    /// <summary>
    /// Store inforamtion about a tile.
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.Tile, GameTMXObjects")]
    public class TileContent
    {
        public int Id { get; set; }         //The id value of the tile
        public ExternalReference<Texture2DContent> Image { get; set; }  //The external reference of the image
        public Rectangle Source { get; set; }   //The source rectangle of the tile in the image

        public PropertiesContent Properties { get; set; }

        public TileContent()
        {
            Properties = new PropertiesContent();
        }
    }
}
