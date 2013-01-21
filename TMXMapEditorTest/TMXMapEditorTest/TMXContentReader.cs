using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameTMXObjects;


namespace TMXMapEditorTest
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    public class TMXContentReader : ContentTypeReader<Map>
    {
        protected override Map Read(ContentReader input, Map existingInstance)
        {
            Map map = existingInstance;
            if (map == null)
            {
                map = new Map();
            }

            map.Version = input.ReadString();
            map.Orientation = input.ReadInt32() == 0 ? Orientation.Orthogonal : Orientation.Isometric;
            map.Width = input.ReadInt32();
            map.Height = input.ReadInt32();
            map.TileWidth = input.ReadInt32();
            map.TileHeight = input.ReadInt32();

            int layerCount = input.ReadInt32();
            for (int i = 0; i < layerCount; ++i)
            {
                map.LayerBaseList.Add(input.ReadObject<LayerBase>());
            }


            return map;
        }
    }
}
