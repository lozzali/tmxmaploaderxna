using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTMXObjects
{
    public abstract class LayerBase
    {
        public string Name { get; set; }            //the name of the layer
        public int Width { get; set; }              //the width of the layer. The number of horizontal tiles
        public int Height { get; set; }             //the height of the layer. The number of vertical tiles
        public double Opacity { get; set; }        // The opacity of the layer as a value from 0 to 1. Defaults to 1.
        public bool Visible { get; set; }          // Whether the layer is shown
        public Properties Properties { get; set; }

        public LayerBase()
        {
            Properties = new Properties();
        }

        public abstract void Draw(SpriteBatch sb, Orientation orientation);
    }

    /// <summary>
    /// The Layer object used in the game code. 
    /// </summary>
    public class Layer : LayerBase
    {
        public Tile[] Tiles { get; set; }             //An array to store the index value of the tiles

        public Layer()
            : base()
        {

        }

        /// <summary>
        /// Draw the layer
        /// </summary>
        /// <param name="sb">Instance of the SpriteBatch</param>
        public override void Draw(SpriteBatch sb, Orientation orientation)
        {
            if (Visible)
            {
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        Tile tile = Tiles[y * Width + x];
                        if (tile.Id != -1)
                        {
                            if (orientation == Orientation.Orthogonal)
                            {
                                sb.Draw(tile.Image,
                                    new Rectangle(x * tile.TileWidth, y * tile.TileHeight, tile.TileWidth, tile.TileHeight),
                                    tile.Source,
                                    Color.White,
                                    0,
                                    Vector2.Zero,
                                    SpriteEffects.None,
                                    0);
                            }
                            else
                            {
                                sb.Draw(tile.Image,
                                new Rectangle((x * tile.TileWidth/2) + (y*tile.TileWidth/2), (y * tile.TileHeight/2) - (x*tile.TileHeight/2), tile.TileWidth, tile.TileHeight),
                                tile.Source,
                                Color.White,
                                0,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0);
                            }
                        }
                    }
                }
            }

        }


    }
}
