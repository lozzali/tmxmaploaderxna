namespace TMXObjects
{
    public class TObject
    {
        public int X { get; set; }              //The x coordinate of the object in pixels.
        public int Y { get; set; }              //The y coordinate of the object in pixels.
        public int GID { get; set; }            // An reference to a tile (optional).
        public int Width { get; set; }          //The width of the object in pixels (defaults to 0).
        public int Height { get; set; }         //The height of the object in pixels (defaults to 0).
        public string Name { get; set; }        // The name of the object. An arbitrary string.
        public string Type { get; set; }       //The type of the object. An arbitrary string.
        public bool Visible { get; set; }      //Whether the object is shown (1) or hidden (0). Defaults to 1
        public TProperties Properties { get; set; } 
        public TPolyBase PolyObject { get; set; }   //can only have one polygon or polyline per object

        public TObject()
        {
            Properties = new TProperties();
            PolyObject = null;
        }

    }
}
