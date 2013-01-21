using System;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace TMXObjects
{
    /// <summary>
    /// A static class to load the TMX file format created by Tiled.
    /// Download Tiled Map Editor http://www.mapeditor.org/
    /// TMX File Specification https://github.com/bjorn/tiled/wiki/TMX-Map-Format  
    /// </summary>
    public class _TMXLoader
    {
        
        #region Map
        //<map>
        //version: The TMX format version, generally 1.0.
        //orientation: Map orientation. Tiled supports "orthogonal" and "isometric" at the moment.
        //width: The map width in tiles.
        //height: The map height in tiles.
        //tilewidth: The width of a tile.
        //tileheight: The height of a tile.
        //The tilewidth and tileheight properties determine the general grid size of the map. The individual tiles may have different sizes. Larger tiles will extend at the top and right (anchored to the bottom left).
        //Can contain: properties, tileset, layer, objectgroup

        /// <summary>
        /// This function loads the tmx file created by the Tiled Map Editor
        /// </summary>
        /// <param name="filename">the path of the tmx file</param>
        /// <returns>A TMap instance that contains the properties of a Map read from the TMX file</returns>
        public static TMap LoadMap(string filename)
        {
            XDocument doc;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;

            using (XmlReader reader = XmlReader.Create(filename, settings))
            {
                doc = XDocument.Load(reader);
            }

            TMap map = new TMap();            
            try
            {                

                XElement MapElement = doc.Element("map");
                map.Version = _Helper.GetStringAttribute(MapElement, "version");
                map.Orientation = _Helper.GetStringAttribute(MapElement, "orientation") == "orthogonal" ? TOrientation.Orthogonal : TOrientation.Isometric;
                map.Width = _Helper.GetIntAttribute(MapElement, "width");
                map.Height = _Helper.GetIntAttribute(MapElement, "height");

                map.TileWidth = _Helper.GetIntAttribute(MapElement, "tilewidth");
                map.TileHeight = _Helper.GetIntAttribute(MapElement, "tileheight");

                foreach (XElement element in MapElement.Descendants())
                {
                    if (element.Name.LocalName.Equals("tileset"))
                    {
                        map.TileSetList.Add(LoadTileSet(element));
                    }
                    else if (element.Name.LocalName.Equals("layer"))
                    {
                        map.LayerBaseList.Add(LoadLayer(element));
                    }
                    else if (element.Name.LocalName.Equals("objectgroup"))
                    {
                        map.LayerBaseList.Add(LoadObjectGroup(element));
                    }
                }

                return map;
            }
            catch (Exception e)
            {
                Console.WriteLine("Loading Map Failed. Message: {0}", e.Message);
                return null;
            }

            
        }
        #endregion

        #region TileSet
        //<tileset>

        //firstgid: The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
        //source: If this tileset is stored in an external TSX (Tile Set XML) file, this attribute refers to that file. That TSX file has the same structure as the attribute as described here. (There is the firstgid attribute missing and this source attribute is also not there. These two attributes are kept in the TMX map, since they are map specific.)
        //name: The name of this tileset.
        //tilewidth: The (maximum) width of the tiles in this tileset.
        //tileheight: The (maximum) height of the tiles in this tileset.
        //spacing: The spacing in pixels between the tiles in this tileset (applies to the tileset image).
        //margin: The margin around the tiles in this tileset (applies to the tileset image).
        //Can contain: tileoffset (since 0.8.0), properties (since 0.8.0), image, tile

        /// <summary>
        /// Load the Tile Set elements in the tmx file. 
        /// </summary>
        /// <param name="element">The XML Element of the TileSet</param>
        /// <returns>A TTileSet instance that contains the properties of the specified Tile Set</returns>
        private static TTileSet LoadTileSet(XElement element)
        {
            try
            {
                TTileSet tileSet = new TTileSet();

                tileSet.FirstGID = _Helper.GetIntAttribute(element, "firstgid");
                tileSet.Source = _Helper.GetStringAttribute(element, "source");
                tileSet.Name = _Helper.GetStringAttribute(element, "name");
                tileSet.TileWidth = _Helper.GetIntAttribute(element, "tilewidth");
                tileSet.TileHeight = _Helper.GetIntAttribute(element, "tileheight");
                tileSet.Spacing = _Helper.GetIntAttribute(element, "spacing");
                tileSet.Margin = _Helper.GetIntAttribute(element, "margin");


                //tileoffset
                XElement tileoffsetElement = element.Element("tileoffset");
                if (tileoffsetElement != null)
                {
                    tileSet.TileOffsetX = _Helper.GetIntAttribute(tileoffsetElement, "x");
                    tileSet.TileOffsetY = _Helper.GetIntAttribute(tileoffsetElement, "y");
                }

                //Load Image
                XElement imageElement = element.Element("image");
                if (imageElement != null)
                {
                    tileSet.Image.Source = _Helper.GetStringAttribute(imageElement, "source");
                    tileSet.Image.Width = _Helper.GetIntAttribute(imageElement, "width");
                    tileSet.Image.Height = _Helper.GetIntAttribute(imageElement, "height");

                    tileSet.Image.Transparancy = _Helper.GetStringAttribute(imageElement, "trans");
                }

                //Load Tiles
                foreach (XElement tileElement in element.Descendants("tile"))
                {
                    int id = _Helper.GetIntAttribute(tileElement, "id");
                    TProperties TileProperties = new TProperties();
                    LoadProperties(tileElement, TileProperties);

                    tileSet.TilePropertyList.Add(id, TileProperties);
                }

                return tileSet;
            }
            catch (Exception e)
            {
                Console.WriteLine("Loading TileSet Failed. Message: {0}", e.Message);
                return null;
            }
        }
        #endregion

        #region Layer

        //<layer>
        //name: The name of the layer.
        //x: The x coordinate of the layer in tiles. Defaults to 0 and can no longer be changed in Tiled Qt.
        //y: The y coordinate of the layer in tiles. Defaults to 0 and can no longer be changed in Tiled Qt.
        //width: The width of the layer in tiles. Traditionally required, but as of Tiled Qt always the same as the map width.
        //height: The height of the layer in tiles. Traditionally required, but as of Tiled Qt always the same as the map height.
        //opacity: The opacity of the layer as a value from 0 to 1. Defaults to 1.
        //visible: Whether the layer is shown (1) or hidden (0). Defaults to 1.

        //Can contain: properties, data

        /// <summary>
        /// Load the Layer elements in the tmx file. 
        /// </summary>
        /// <param name="element">The XML Element of the Layer</param>
        /// <returns>A TLayer instance that contains the properties of the specified Layer</returns>
        private static TLayer LoadLayer(XElement element)
        {
            try
            {
                int width = _Helper.GetIntAttribute(element, "width");
                int height = _Helper.GetIntAttribute(element, "height");
                bool visible = _Helper.GetIntAttribute(element, "visible", 1) == 0 ? false : true;
                double opacity = _Helper.GetDoubleAttribute(element, "opacity", 1.0);

                TLayer layer = new TLayer();
                layer.Width = width;
                layer.Height = height;
                layer.Visible = visible;
                layer.Opacity = opacity;
                layer.Name = _Helper.GetStringAttribute(element, "name");

                //Properties
                LoadProperties(element, layer.Properties);

                LoadLayerData(element, ref layer);

                return layer;

            }
            catch (Exception e)
            {
                Console.WriteLine("Loading Layer Failed. Message: {0}", e.Message);
                return null;
            }
        }
        #endregion

        #region LayerData
        //<data>
        //encoding: The encoding used to encode the tile layer data. When used, it can be "base64" and "csv" at the moment.
        //compression: The compression used to compress the tile layer data. Tiled Qt supports "gzip" and "zlib".

        //When no encoding or compression is given, the tiles are stored as individual XML tile elements. Next to that, the easiest format to parse is the "csv" (comma separated values) format.

        //The base64-encoded and optionally compressed layer data is somewhat more complicated to parse. First you need to base64-decode it, then you may need to decompress it. Now you have an array of bytes, which should be interpreted as an array of unsigned 32-bit integers using little-endian byte ordering.
        //Whatever format you choose for your layer data, you will always end up with so called "global tile IDs" (gids). They are global, since they may refer to a tile from any of the tilesets used by the map. In order to find out from which tileset the tile is you need to find the tileset with the highest firstgid that is still lower or equal than the gid. The tilesets are always stored with increasing firstgids.

        /// <summary>
        /// Load the Layer data from the specified data element. 
        /// </summary>
        /// <param name="element">The XML element of the data</param>
        /// <param name="layer">The reference to the TLayer instance needed to populate the data property</param>
        private static void LoadLayerData(XElement element, ref TLayer layer)
        {
            XElement dataElement = element.Element("data");

            string encoding = _Helper.GetStringAttribute(dataElement, "encoding");
            string compression = _Helper.GetStringAttribute(dataElement, "compression");
            string data = dataElement.Value.Trim();
            byte[] val = null;
            switch (encoding)
            {
                case "base64":
                    {

                        if (compression.Equals("gzip"))
                        {
                            val = _Helper.DecompressGZip(Convert.FromBase64String(data));
                        }
                        else if (compression.Equals("zlib"))
                        {
                            val = _Helper.DecompressZLib(Convert.FromBase64String(data));
                        }
                        else
                        {
                            val = Convert.FromBase64String(data);   //uncompressed
                        }
                        if (val != null)
                        {
                            if (layer.Data == null)
                            {
                                layer.Data = new int[layer.Width * layer.Height];
                            }
                            using (BinaryReader br = new BinaryReader(new MemoryStream(val)))
                            {
                                for (int i = 0; i < layer.Data.Length; ++i)
                                {
                                    layer.Data[i] = br.ReadInt32();
                                }
                            }
                        }
                    }
                    break;
                case "csv":
                    {

                        string[] dataList = data.Split(',');
                        layer.Data = new int[dataList.Length];
                        for (int i = 0; i < dataList.Length; ++i)
                        {
                            //layer.Data should already be instantiated when the Layer instance is created. Size of layer.Data is determined by width*height
                            layer.Data[i] = Convert.ToInt32(dataList[i]);
                        }

                    }
                    break;
                default:                    //XML
                    {
                        int count = 0;

                        layer.Data = new int[layer.Width * layer.Height];
                        foreach (XElement tileElement in dataElement.Descendants("tile"))
                        {
                            layer.Data[count] = _Helper.GetIntAttribute(tileElement, "gid");
                            count++;
                        }
                    }
                    break;

            }
        }
        #endregion

        #region ObjectGroup
        //<objectgroup>
        //name: The name of the object group.
        //color: The color used to display the objects in this group.
        //x: The x coordinate of the object group in tiles. Defaults to 0 and can no longer be changed in Tiled Qt.
        //y: The y coordinate of the object group in tiles. Defaults to 0 and can no longer be changed in Tiled Qt.
        //width: The width of the object group in tiles. Meaningless.
        //height: The height of the object group in tiles. Meaningless.
        //opacity: The opacity of the layer as a value from 0 to 1. Defaults to 1.
        //visible: Whether the layer is shown (1) or hidden (0). Defaults to 1.

        //The object group is in fact a map layer, and is hence called "object layer" in Tiled Qt.
        //Can contain: properties, object

        /// <summary>
        /// Load the objectgroup elements in the tmx file. 
        /// </summary>
        /// <param name="element">The XML Element of the ObjectGroup</param>
        /// <returns>A TObjectGroup instance that contains the properties of the specified ObjectGroup</returns>
        private static TObjectGroup LoadObjectGroup(XElement element)
        {
            try
            {
                TObjectGroup og = new TObjectGroup();
                og.Name = _Helper.GetStringAttribute(element, "name");
                og.Width = _Helper.GetIntAttribute(element, "width");
                og.Height = _Helper.GetIntAttribute(element, "height");
                og.Visible = _Helper.GetIntAttribute(element, "visible", 1)== 1 ? true: false;
                og.Opacity = _Helper.GetDoubleAttribute(element, "opacity", 1.0);
                og.Color = _Helper.GetStringAttribute(element, "color", "");

                //load objects
                foreach(XElement objectElement in element.Descendants("object"))
                {
                    og.ObjectList.Add(LoadObject(objectElement));
                }

                LoadProperties(element, og.Properties);

                return og;

            }
            catch (Exception e)
            {
                Console.WriteLine("Loading LoadObjectGroup Failed. Message: {0}", e.Message);
                return null;
            }
        }
        #endregion

        #region Object
        //<object>
        //name: The name of the object. An arbitrary string.
        //type: The type of the object. An arbitrary string.
        //x: The x coordinate of the object in pixels.
        //y: The y coordinate of the object in pixels.
        //width: The width of the object in pixels (defaults to 0).
        //height: The height of the object in pixels (defaults to 0).
        //gid: An reference to a tile (optional).
        //visible: Whether the object is shown (1) or hidden (0). Defaults to 1. (will come in 0.9.0)

        //While tile layers are very suitable for anything repetitive aligned to the tile grid, sometimes you want to annotate your map with other information, not necessarily aligned to the grid. Hence the objects have their coordinates and size in pixels, but you can still easily align that to the grid when you want to.
        //You generally use objects to add custom information to your tile map, such as spawn points, warps, exits, etc.
        //When the object has a gid set, then it is represented by the image of the tile with that global ID. Currently that means width and height are ignored for such objects. The image alignment currently depends on the map orientation. In orthogonal orientation it's aligned to the bottom-left while in isometric it's aligned to the bottom-center.
        //Can contain: properties, polygon, polyline, image

        /// <summary>
        /// Load the object elements in the tmx file. 
        /// </summary>
        /// <param name="element">The XML Element of the Object</param>
        /// <returns>A TObject instance that contains the properties of the specified Object</returns>
        private static TObject LoadObject(XElement element)
        {
            try
            {
                TObject to = new TObject();
                to.GID = _Helper.GetIntAttribute(element, "gid");
                to.Name = _Helper.GetStringAttribute(element, "name");
                to.Width = _Helper.GetIntAttribute(element, "width");
                to.Height = _Helper.GetIntAttribute(element, "height");
                to.Type = _Helper.GetStringAttribute(element, "type");
                to.X = _Helper.GetIntAttribute(element, "x");
                to.Y = _Helper.GetIntAttribute(element, "y");
                to.Visible = _Helper.GetIntAttribute(element, "visible", 1) == 1 ? true : false;

                XElement polygonElement = element.Element("polygon");
                if (polygonElement != null)
                {
                    string polygonData = _Helper.GetStringAttribute(polygonElement, "points").Trim();
                    to.PolyObject = new TPolygon(polygonData);
                }

                XElement polylineElement = element.Element("polyline");
                if (polylineElement != null)
                {
                    string polylineData = _Helper.GetStringAttribute(polylineElement, "points").Trim();
                    to.PolyObject = new TPolyline(polylineData);
                }

                LoadProperties(element, to.Properties);

                return to;

            }
            catch (Exception e)
            {
                Console.WriteLine("Loading TiledObject Failed. Message: {0}", e.Message);
                return null;
            }
        }

        #endregion

        #region Properties
        //<properties>
        //Can contain: property

        //Wraps any number of custom properties. Can be used as a child of the map, tile (when part of a tileset), layer, 
        //objectgroup and object elements.
        
        //<property>
        //name: The name of the property.
        //value: The value of the property.
        /// <summary>
        /// Load the Properties elements in the tmx file. 
        /// </summary>
        /// <param name="element">The XML Element of the properties</param>
        /// <param name="properties">The reference to the TProperties instance for populating the properties</param>
        private static void LoadProperties(XElement element, TProperties properties)
        {
            try
            {            
                XElement propertiesElement = element.Element("properties");
                
                if(propertiesElement != null)
                {
                    foreach (XElement propertyElement in propertiesElement.Descendants("property"))
                    {
                        properties.Add(_Helper.GetStringAttribute(propertyElement, "name"), _Helper.GetStringAttribute(propertyElement, "value"));
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Loading TiledProperties Failed. Message: {0}", e.Message);
            }
        }
        #endregion
    }
}
