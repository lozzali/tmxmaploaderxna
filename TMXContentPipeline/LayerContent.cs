using Microsoft.Xna.Framework.Content;

namespace TMXContentPipeline
{
    [ContentSerializerRuntimeType("GameTMXObjects.LayerBase, GameTMXObjects")]
    public abstract class LayerBaseContent
    {
        public string Name { get; set; }            //the name of the layer
        public int Width { get; set; }              //the width of the layer. The number of horizontal tiles
        public int Height { get; set; }             //the height of the layer. The number of vertical tiles
        public double Opacity { get; set; }        // The opacity of the layer as a value from 0 to 1. Defaults to 1.
        public bool Visible { get; set; }          // Whether the layer is shown
        public PropertiesContent Properties { get; set; }

        public LayerBaseContent()
        {
            Properties = new PropertiesContent();
        }
    }

    [ContentSerializerRuntimeType("GameTMXObjects.Layer, GameTMXObjects")]
    public class LayerContent : LayerBaseContent
    {
        public TileContent [] Tiles { get; set; }             //An array to store the index value of the tiles

        public LayerContent()
            : base()
        {
        }

    }
}
