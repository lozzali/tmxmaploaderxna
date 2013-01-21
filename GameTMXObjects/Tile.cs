using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTMXObjects
{
    /// <summary>
    /// The Tile Object
    /// </summary>
    public class Tile
    {
        public int Id { get; set; }                 //The id value of the 
        public Texture2D Image { get; set; }        //The reference to the Image of the Tileset
        public Rectangle Source { get; set; }       //The source rectangle for the tile image
        public Properties Properties { get; set; }

        public Tile()
        {
            Properties = new Properties();
        }

        /// <summary>
        /// The tile width in pixels
        /// </summary>
        public int TileWidth
        {
            get
            {
                return Source.Width;
            }
        }

        /// <summary>
        /// The tile height in pixels
        /// </summary>
        public int TileHeight
        {
            get
            {
                return Source.Height;
            }
        }
    }
}
