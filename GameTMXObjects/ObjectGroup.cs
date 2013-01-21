using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTMXObjects
{
    /// <summary>
    /// The ObjectGroup
    /// </summary>
    public class ObjectGroup : LayerBase
    {
        public Color? Colour { get; set; }      //The color used to display the objects in this group.        
        public List<ObjectBase> ObjectList { get; set; }

        public ObjectGroup() 
            : base()
        {
            ObjectList = new List<ObjectBase>();
        }

        /// <summary>
        /// Drawing the objectgroup which contains a collection of objects
        /// </summary>
        /// <param name="sb">Instance of the SpriteBatch</param>
        public override void Draw(SpriteBatch sb, Orientation orientation)
        {
            if (this.Visible)
            {
                foreach (ObjectBase obj in ObjectList)
                {
                    if (obj.Visible)
                    {
                        //default object colour to dark blue if no colour is found
                        obj.Draw(sb, this.Colour.HasValue? this.Colour.Value : Color.DarkBlue);
                    }
                }
            }
        }


        
    }
}
