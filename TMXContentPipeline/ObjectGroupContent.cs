using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TMXContentPipeline
{
    [ContentSerializerRuntimeType("GameTMXObjects.ObjectGroup, GameTMXObjects")]
    public class ObjectGroupContent : LayerBaseContent
    {
        public Color? Colour { get; set; }      //The color used to display the objects in this group.
        public List<ObjectBaseContent> ObjectList { get; set; }

        public ObjectGroupContent()
        {
            ObjectList = new List<ObjectBaseContent>();
        }



    }
}
