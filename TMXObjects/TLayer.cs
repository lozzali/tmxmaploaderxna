
namespace TMXObjects
{
    public abstract class TLayerBase
    {
        public string Name { get; set; }            //the name of the layer
        public int Width { get; set; }              //the width of the layer. The number of horizontal tiles
        public int Height { get; set; }             //the height of the layer. The number of vertical tiles
        public double Opacity { get; set; }        // The opacity of the layer as a value from 0 to 1. Defaults to 1.
        public bool Visible { get; set; }          // Whether the layer is shown
        public TProperties Properties { get; set; }

        public TLayerBase()
        {
            Properties = new TProperties();
            Visible = true;
            Opacity = 1.0;
            Width = 0;
            Height = 0;
        }
    }

    public class TLayer : TLayerBase
    {
        public int[] Data { get; set; }             //An array to store the index value of the tiles

        public TLayer()
            : base()
        {

        }


    }
}
