using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTMXObjects
{
    #region ObjectBase
    /// <summary>
    /// Base class of the objects
    /// </summary>
    public abstract class ObjectBase
    {
        public int X { get; set; }          // The x coordinate of the object in pixels.
        public int Y { get; set; }          //The y coordinate of the object in pixels.
        public int Width { get; set; }      //The width of the object in pixels (defaults to 0).
        public int Height { get; set; }     //The height of the object in pixels (defaults to 0).
        public string Name { get; set; }    //The name of the object. An arbitrary string.
        public string Type { get; set; }    //The type of the object. An arbitrary string.
        public string ObjectType { get; set; }  //The type of object it is. e.g. Tile, Polygon, Polyline or Box
        public bool Visible { get; set; }       //Whether the object is shown or hidden. Default to true
        public Properties Properties { get; set; }
        
        
        public ObjectBase()
        {
            Visible = true;
            Width = 0;
            Height = 0;
            Properties = new Properties();            
        }

        public abstract void Draw(SpriteBatch sb, Color objectColour);

        public virtual bool CheckCollision(Rectangle rect)
        {
            return false;
        }

    }
    #endregion

    #region TileObject
    /// <summary>
    /// A TileObject 
    /// </summary>
    public class TileObject : ObjectBase
    {        
        public Tile Tile { get; set; }  //The reference of the tile in this tile object

        public TileObject()
            : base()
        {
            ObjectType = "tile";
        }

        /// <summary>
        /// Drawing the tile object
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="objectColour"></param>
        public override void Draw(SpriteBatch sb, Color objectColour)
        {
            sb.Draw(Tile.Image,
                new Rectangle(X, Y, Tile.TileWidth, Tile.TileHeight),
                Tile.Source,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0);
        }
    }
    #endregion

    #region PolyBaseObject
    /// <summary>
    /// A base class for the polygon and polyline object
    /// </summary>
    public abstract class PolyBaseObject: ObjectBase    
    {
        protected Texture2D ObjectTexture = null;   //texture to draw the lines for constructing the polygon/polyline
        public Vector2[] Points { get; set; }       //an array of points to construct the polygon/polyline

        public PolyBaseObject()
            : base()
        {    
        }

        /// <summary>
        /// A function to draw a line between 2 points
        /// </summary>
        /// <param name="sb">Instance of the SpriteBatch</param>
        /// <param name="p1">The starting point to draw</param>
        /// <param name="p2">The end point to draw to</param>
        /// <param name="lineThickness">The thickness of the lines for the poly</param>
        /// <param name="lineColour">The colour of the lines</param>
        public virtual void DrawLine(SpriteBatch sb, Vector2 p1, Vector2 p2, int lineThickness, Color lineColour)
        {
            float size = Vector2.Distance(p1, p2);
            float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            sb.Draw(ObjectTexture, p1, null, lineColour,
                angle, Vector2.Zero, new Vector2(size, lineThickness),
                SpriteEffects.None, 0);

        }

        /// <summary>
        /// A function to draw a rectangle
        /// </summary>
        /// <param name="sb">Instance of the SpriteBatch</param>
        /// <param name="x">starting x position of the rectangle</param>
        /// <param name="y">starting y position of the rectangle</param>
        /// <param name="width">the width of the rectangle</param>
        /// <param name="height">the height of the rectangle</param>
        /// <param name="lineThickness">the thickness of the line</param>
        /// <param name="lineColour">the colour of the line</param>
        public virtual void DrawRectangle(SpriteBatch sb, int x, int y, int width, int height, int lineThickness, Color lineColour)
        {     
            sb.Draw(ObjectTexture, new Rectangle(x, y, width, lineThickness), lineColour);            
            // Draw left line
            sb.Draw(ObjectTexture, new Rectangle(X, Y, lineThickness, Height), lineColour);
            // Draw right line
            sb.Draw(ObjectTexture, new Rectangle((X + Width - lineThickness), Y, lineThickness, Height), lineColour);
            // Draw bottom line
            sb.Draw(ObjectTexture, new Rectangle(X, Y + Height - lineThickness, Width, lineThickness), lineColour);
        }


    }
    #endregion

    #region PolygonObject

    /// <summary>
    /// A PolygonObject in game
    /// </summary>
    public class PolygonObject : PolyBaseObject
    {               
        public PolygonObject()
            : base()
        {
            ObjectType = "polygon";
        }

        /// <summary>
        /// Draw the polygon object
        /// </summary>
        /// <param name="sb">
        /// <param name="sb">Instance of the SpriteBatch</param>
        /// <param name="objectColour">The colour of the object</param>
        public override void Draw(SpriteBatch sb, Color objectColour)
        {
            if (ObjectTexture == null)
            {
                ObjectTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                ObjectTexture.SetData(new [] {Color.White});
            }

            Vector2 ObjectPos = (new Vector2(X, Y));

            for (int i = 0; i < Points.Length; ++i)
            {
                if (i != Points.Length - 1)
                {
                    DrawLine(sb, ObjectPos + Points[i], ObjectPos + Points[i + 1], 2, objectColour);
                }
                else
                {
                    DrawLine(sb, ObjectPos + Points[i], ObjectPos + Points[0], 2, objectColour);
                }
            }
        }
    }
    #endregion

    #region PolylineObject
    /// <summary>
    /// A PolylineObject in game
    /// </summary>   
    public class PolylineObject : PolyBaseObject
    {      
        public PolylineObject()
            : base()
        {
            ObjectType = "polyline";
        }

        /// <summary>
        /// Draw the polyline object
        /// </summary>
        /// <param name="sb">
        /// <param name="sb">Instance of the SpriteBatch</param>
        /// <param name="objectColour">The colour of the object</param>
        public override void Draw(SpriteBatch sb, Color objectColour)
        {
            if (ObjectTexture == null)
            {
                ObjectTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                ObjectTexture.SetData(new[] { Color.White });
            }
            Vector2 ObjectPos = (new Vector2(X, Y));
            for (int i = 0; i < Points.Length-1; ++i)
            {
                DrawLine(sb, ObjectPos + Points[i], ObjectPos + Points[i + 1], 2, objectColour);
            }
        }
    }
    #endregion

    #region BoxObject
    /// <summary>
    /// A BoxObject in game
    /// </summary>   
    public class BoxObject : PolyBaseObject
    {

        public BoxObject()
            : base()
        {
            ObjectType = "box";
        }

        /// <summary>
        /// Draw the Box object
        /// </summary>
        /// <param name="sb">
        /// <param name="sb">Instance of the SpriteBatch</param>
        /// <param name="objectColour">The colour of the object</param>
        public override void Draw(SpriteBatch sb, Color objectColour)
        {
            if (ObjectTexture == null)
            {
                ObjectTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                ObjectTexture.SetData(new[] { Color.White });
            }

            if (Width != 0 && Height != 0)
            {
                DrawRectangle(sb, X, Y, Width, Height, 2, objectColour);
            }
        }

        public override bool CheckCollision(Rectangle rect)
        {
            return rect.Intersects(new Rectangle(X, Y, Width, Height));
        }
    }
    #endregion
}
