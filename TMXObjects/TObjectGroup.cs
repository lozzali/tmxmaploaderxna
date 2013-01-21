using System.Collections.Generic;

namespace TMXObjects
{
    public class TObjectGroup : TLayerBase
    {
        public string Color { get; set; }               //The color used to display the objects in this group.
        public List<TObject> ObjectList { get; set; }   //List of Objects

        public TObjectGroup()
            : base()
        {
            ObjectList = new List<TObject>();
        }



    }
}
