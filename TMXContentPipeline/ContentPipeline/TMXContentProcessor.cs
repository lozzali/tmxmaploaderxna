using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using TMXObjects;
using System.IO;
using System;

namespace TMXContentPipeline.ContentPipeline
{
    /// <summary>
    /// The content processor that maps the data in the TiledObject project to the TiledContentPipeline Object
    /// </summary>
    [ContentProcessor(DisplayName = "TMX Processor")]
    public class TMXContentProcessor : ContentProcessor<TMap, MapContent>
    {
        protected MapContent mapContent = null;

        public override MapContent Process(TMap input, ContentProcessorContext context)
        {
            return ProcessMap(input, context);            
        }

        #region ProcessImage
        /// <summary>
        /// Process the image of the tileset
        ///This will output an binary .xnb file of the tileset image
        /// </summary>
        protected ExternalReference<Texture2DContent> ProcessImage(TImage image, ContentProcessorContext context)
        {
            Color? trans = ConvertHexStringToColour(image.Transparancy);

            // the asset name is the entire path, minus extension, after the content directory
            string asset = Path.GetFileNameWithoutExtension(image.Source);

            // build the asset as an external reference
            OpaqueDataDictionary data = new OpaqueDataDictionary();
            data.Add("TextureFormat", TextureProcessorOutputFormat.Color);
            data.Add("ColorKeyEnabled", trans.HasValue);
            data.Add("ColorKeyColor", trans.HasValue ? trans.Value : Microsoft.Xna.Framework.Color.Magenta);
            return context.BuildAsset<Texture2DContent, Texture2DContent>(
                new ExternalReference<Texture2DContent>(image.Source), null, data, null, asset);            
        }
        #endregion

        #region GetTile
        /// <summary>
        /// return the correct tile back based on the index id of the tile
        /// </summary>
        /// <param name="gidValue">the gid value of the tile</param>
        /// <returns>Return the Tile if found. Otherwise return a dummy Tile Object with a value of -1 as the Id, to signify that it is an offset tile</returns>
        protected TileContent GetTile(int gidValue)
        {
            foreach (TileSetContent tileset in mapContent.TileSetList)
            {
                int arrayIndex = gidValue - tileset.FirstGID;
                if (arrayIndex >= 0 && arrayIndex < tileset.TileList.Length)
                {
                    return tileset.TileList[arrayIndex];
                }
            }

            //If no tile is found, this means the tile is an offset tile.
            //Set the id to -1 to show its an offset tile.
            TileContent tile = new TileContent();
            tile.Id = -1;
            tile.Source = new Rectangle(0, 0, mapContent.TileWidth, mapContent.TileHeight);
            return tile;            
        }
        #endregion

        #region ProcessLayer
        /// <summary>
        /// Process the layer data
        /// </summary>
        /// <param name="layer">The instance of the TLayer</param>
        /// <param name="context">The instance of the LayerContent</param>
        /// <returns></returns>
        protected LayerContent ProcessLayer(TLayer layer, ContentProcessorContext context)
        {
            LayerContent res = new LayerContent();
            res.Width = layer.Width;
            res.Height = layer.Height;
            res.Opacity = layer.Opacity;
            res.Visible = layer.Visible;
            res.Name = layer.Name;
            res.Properties = ProcessProperties(layer.Properties, context);

            int length = layer.Height*layer.Width;
            res.Tiles = new TileContent[length];

            for (int x = 0; x < length; ++x)
            {
                res.Tiles[x] = GetTile(layer.Data[x]);
            }
            
            return res;
        }
        #endregion

        #region ProcessMap
        /// <summary>
        /// Process the Map data
        /// </summary>
        /// <param name="map">The instance of the TMap</param>
        /// <param name="context"></param>
        /// <returns>The instance of the MapContent</returns>
        protected MapContent ProcessMap(TMap map, ContentProcessorContext context)
        {
            mapContent = new MapContent();
            mapContent.Version = map.Version;
            mapContent.Width = map.Width;
            mapContent.Height = map.Height;
            mapContent.TileWidth = map.TileWidth;
            mapContent.TileHeight = map.TileHeight;
            mapContent.Orientation = map.Orientation == TMXObjects.TOrientation.Isometric ? (int)TMXContentPipeline.Orientation.Isometric : (int)TMXContentPipeline.Orientation.Orthogonal;

            foreach (TTileSet ts in map.TileSetList)
            {
                mapContent.TileSetList.Add(ProcessTileSet(ts, context));
            }

            foreach (TLayerBase layerBase in map.LayerBaseList)
            {
                TLayer layer = layerBase as TLayer;
                if (layer != null)
                {
                    mapContent.LayerBaseList.Add(ProcessLayer(layer, context));
                }

                TObjectGroup objectGroup = layerBase as TObjectGroup;
                if (objectGroup != null)
                {
                    mapContent.LayerBaseList.Add(ProcessObjectGroup(objectGroup, context));
                }
            }


            return mapContent;
        }
        #endregion

        #region ProcessObjectGroup
        /// <summary>
        /// Process the ObjectGroup data
        /// </summary>
        /// <param name="og">The instance of the TObjectGroup</param>
        /// <param name="context"></param>
        /// <returns>The instance of the ObjectGroupContent</returns>
        protected ObjectGroupContent ProcessObjectGroup(TObjectGroup og, ContentProcessorContext context)
        {
            ObjectGroupContent res = new ObjectGroupContent();
            res.Name = og.Name;
            res.Height = og.Height;
            res.Width = og.Width;
            res.Opacity = og.Opacity;
            res.Visible = og.Visible;
            res.Properties = ProcessProperties(og.Properties, context);

            res.Colour = ConvertHexStringToColour(og.Color);

            foreach (TObject obj in og.ObjectList)
            {
                res.ObjectList.Add(ProcessObject(obj, context));
            }

            return res;
        }
        #endregion

        #region ProcessObject
        /// <summary>
        /// Process the Object data
        /// </summary>
        /// <param name="obj">The instance of the TObject</param>
        /// <param name="context"></param>
        /// <returns>The instance of the Object</returns>
        protected ObjectBaseContent ProcessObject(TObject obj, ContentProcessorContext context)
        {
            //its a tiled object
            if (obj.GID > 0)
            {
                TileObjectContent res = new TileObjectContent();
                res.Name = obj.Name;
                res.X = obj.X;
                res.Y = obj.Y;
                res.Visible = obj.Visible;
                res.Type = obj.Type;
                res.Tile = GetTile(obj.GID);

                return res;
            }
            //It's a polygon or a polyline object
            else if (obj.PolyObject != null)
            {
                //its a polygon object
                if (obj.PolyObject.PType == TPolyBase.TPolyType.Type_Polygon)
                {
                    PolygonObjectContent res = new PolygonObjectContent();
                    res.Name = obj.Name;
                    res.X = obj.X;
                    res.Y = obj.Y;
                    res.Visible = obj.Visible;
                    res.Type = obj.Type;

                    res.Points = new Vector2[obj.PolyObject.Points.Length];
                    for (int i = 0; i < obj.PolyObject.Points.Length; ++i)
                    {
                        TPoint point = obj.PolyObject.Points[i];
                        res.Points[i] = new Vector2(point.X, point.Y);
                    }


                    return res;
                }
                //polyline object
                if (obj.PolyObject.PType == TPolyBase.TPolyType.Type_Polyline)
                {
                    PolylineObjectContent res = new PolylineObjectContent();
                    res.Name = obj.Name;
                    res.X = obj.X;
                    res.Y = obj.Y;
                    res.Visible = obj.Visible;
                    res.Type = obj.Type;

                    res.Points = new Vector2[obj.PolyObject.Points.Length];
                    for (int i = 0; i < obj.PolyObject.Points.Length; ++i)
                    {
                        TPoint point = obj.PolyObject.Points[i];
                        res.Points[i] = new Vector2(point.X, point.Y);
                    }


                    return res;
                }
            }
            //box object
            else
            {
                BoxObjectContent res = new BoxObjectContent();
                res.Name = obj.Name;
                res.X = obj.X;
                res.Y = obj.Y;
                res.Width = obj.Width;
                res.Height = obj.Height;
                res.Visible = obj.Visible;
                res.Type = obj.Type;

                return res;

            }

            //never should happen
            return null;
        }
        #endregion

        #region ProcessProperties
        /// <summary>
        /// Process the properties data
        /// </summary>
        /// <param name="properties">The instance of the TProperties</param>
        /// <param name="context"></param>
        /// <returns>The instance of the PropertiesContent</returns>
        protected PropertiesContent ProcessProperties(TProperties properties, ContentProcessorContext context)
        {
            PropertiesContent prop = new PropertiesContent();

            foreach (KeyValuePair<string, string> kvp in properties)
            {
                prop.Add(kvp.Key, kvp.Value);
            }

            return prop;
        }
        #endregion

        #region ProcessTileSet
        /// <summary>
        /// Process the tile set data. This will construct our tiles in our tileset used for reference in the layers
        /// </summary>
        /// <param name="tileset">The instance of the TTileSet</param>
        /// <param name="context"></param>
        /// <returns>The instance of the TileSetContent</returns>
        protected TileSetContent ProcessTileSet(TTileSet tileset, ContentProcessorContext context)
        {
            TileSetContent res = new TileSetContent();
            res.FirstGID = tileset.FirstGID;            
            res.Name= tileset.Name;
            res.Source = tileset.Source;
            res.Spacing = tileset.Spacing;
            res.Margin = tileset.Margin;
            res.TileHeight = tileset.TileHeight;
            res.TileWidth = tileset.TileWidth;
            res.TileOffsetX = tileset.TileOffsetX;
            res.TileOffsetY = tileset.TileOffsetY;
            
            //process the image for the tileset
            res.Image = ProcessImage(tileset.Image, context);
            int imageWidth = tileset.Image.Width;
            int imageHeight = tileset.Image.Height;

            //workout the image position for each tile
            imageWidth-=(2 * res.Margin);
            int noOfTilesX = 0;
            int measureTileWidth = 0;
            while (measureTileWidth < imageWidth)
            {
                noOfTilesX++;
                measureTileWidth = noOfTilesX * (tileset.TileWidth + tileset.Spacing);
            }

            //last tile is not included if it is less than the tilewidth
            int val = imageWidth % (tileset.TileWidth + tileset.Spacing);
            if (val != 0 && val != tileset.TileWidth)
            {
                noOfTilesX--;
            }

            //workout the image position for each tile
            imageHeight-=(2 * res.Margin);
            int noOfTilesY = 0;
            int measureTileHeight = 0;
            while (measureTileHeight < imageHeight)
            {
                noOfTilesY++;
                measureTileHeight = noOfTilesY * (tileset.TileHeight + tileset.Spacing);
            }

            val = imageHeight % (tileset.TileHeight + tileset.Spacing);
            //last tile is not included if it is less than the tile height
            if (val != 0 && val != tileset.TileHeight)
            {
                noOfTilesY--;
            }
            
            //now we can construct our tiles in our tileset
            res.TileList = new TileContent[noOfTilesY*noOfTilesX];

            for (int y = 0; y < noOfTilesY; y++)
            {
                for (int x = 0; x < noOfTilesX; x++)
                {
                    int index = (y * noOfTilesX) + x;
                    TileContent tile = new TileContent();
                    tile.Id = index;
                    int rx = tileset.Margin + x * (tileset.TileWidth + tileset.Spacing);
                    int ry = tileset.Margin + y * (tileset.TileHeight + tileset.Spacing);
                    tile.Source = new Rectangle(rx, ry, tileset.TileWidth, tileset.TileHeight);
                    tile.Image = res.Image;
                    
                    if (tileset.TilePropertyList.ContainsKey(index))
                    {
                        tile.Properties = ProcessProperties(tileset.TilePropertyList[index], context);
                    }

                    res.TileList[index] = tile;

                }

            }
            return res;
        }
        #endregion

        public static Color? ConvertHexStringToColour(string hexString)
        {
            if (hexString.Length > 5)
            {
                string col = hexString;
                if (hexString.StartsWith("#"))
                {
                    col = hexString.Substring(1);
                }
                string r = col.Substring(0, 2);
                string g = col.Substring(2, 2);
                string b = col.Substring(4, 2);

                return new Color((int)Convert.ToInt32(r, 16), (int)Convert.ToInt32(g, 16), (int)Convert.ToInt32(b, 16));
            }

            return null;
        }
    }


}