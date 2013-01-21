using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TMXContentPipeline
{
    /// <summary>
    /// Base class for all the Object Types. 
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.ObjectBase, GameTMXObjects")]
    public abstract class ObjectBaseContent
    {
        public int X { get; set; }          //The x coordinate of the object in pixels.
        public int Y { get; set; }          //The y coordinate of the object in pixels.
        public int Width { get; set; }      //The width of the object in pixels (defaults to 0).
        public int Height { get; set; }     //The height of the object in pixels (defaults to 0).
        public string Name { get; set; }    // The name of the object. An arbitrary string.
        public string Type { get; set; }    //The type of the object. An arbitrary string.
        public string ObjectType { get; set; }  //The type of object it is in string value. 
                                                //Either a "tile", "box", "polygon" or "polyline" object type
        public bool Visible { get; set; }       //Whether the object is shown
        
        public PropertiesContent Properties { get; set; } 
        

        public ObjectBaseContent()
        {
            Visible = true;
            Width = 0;
            Height = 0;
            Properties = new PropertiesContent();
        }

    }

    /// <summary>
    /// A Tile Object
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.TileObject, GameTMXObjects")]
    public class TileObjectContent : ObjectBaseContent
    {
        public TileContent Tile { get; set; }   //Reference of the tile for this tile object.

        public TileObjectContent() 
            : base()
        {
            ObjectType = "tile";
        }
    }

    /// <summary>
    /// This is a base class for the polygon and polyline class
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.PolyBaseObject, GameTMXObjects")]
    public class PolyBaseObjectContent : ObjectBaseContent
    {
        public Vector2[] Points { get; set; }
    }

    /// <summary>
    /// A Polygon Object
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.PolygonObject, GameTMXObjects")]
    public class PolygonObjectContent : PolyBaseObjectContent
    {
        
        public PolygonObjectContent()
            : base()
        {
            ObjectType = "polygon";
        }
    }

    /// <summary>
    /// A Polyline Object
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.PolylineObject, GameTMXObjects")]
    public class PolylineObjectContent : PolyBaseObjectContent
    {
        public PolylineObjectContent()
            : base()
        {
            ObjectType = "polyline";
        }
    }

    /// <summary>
    /// A Box object. 
    /// </summary>
    [ContentSerializerRuntimeType("GameTMXObjects.BoxObject, GameTMXObjects")]
    public class BoxObjectContent : PolyBaseObjectContent
    {

        public BoxObjectContent()
            : base()
        {
            ObjectType = "box";
        }
    }
}
