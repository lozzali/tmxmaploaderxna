using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.GZip;                     //for gzip
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace TMXObjects
{
    /// <summary>
    /// Helper class for common used functions
    /// </summary>
    public class _Helper
    {
        /// <summary>
        /// Decompresses ZLib data stream
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DecompressZLib(byte[] data)
        {
            byte[] uncompressedData = null;

            using (System.IO.MemoryStream sourceStream = new System.IO.MemoryStream(data, false))
            {
                using (System.IO.MemoryStream destinationStream = new System.IO.MemoryStream())
                {
                    using (InflaterInputStream zipStream = new InflaterInputStream(sourceStream))
                    {
                        byte[] buffer = new byte[4096];
                        Int32 bytesRead = 0;
                        do
                        {
                            bytesRead = zipStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead != 0)
                            {
                                destinationStream.Write(buffer, 0, bytesRead);
                            }
                        }
                        while (bytesRead != 0);
                    }
                    uncompressedData = destinationStream.ToArray();
                }
            }

            return uncompressedData;
        }

        /// <summary>
        /// Decompresses GZip Streams
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DecompressGZip(byte[] data)
        {
            byte[] uncompressedData = null;

            using (System.IO.MemoryStream sourceStream = new System.IO.MemoryStream(data, false))
            {
                using (System.IO.MemoryStream destinationStream = new System.IO.MemoryStream())
                {
                    using (GZipInputStream gzipStream = new GZipInputStream(sourceStream))
                    {
                        byte[] buffer = new byte[4096];
                        Int32 bytesRead = 0;
                        do
                        {
                            bytesRead = gzipStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead != 0)
                            {
                                destinationStream.Write(buffer, 0, bytesRead);
                            }
                        }
                        while (bytesRead != 0);
                    }

                    uncompressedData = destinationStream.ToArray();
                }

            }

            return uncompressedData;
        }

        /// <summary>
        /// Return the integer value of the specified attribute name. If no attribute is found, it will return the specified default value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultInt"></param>
        /// <returns></returns>
        public static int GetIntAttribute(XElement node, string attributeName, int defaultInt=0)
        {
            try
            {
                return int.Parse(node.Attribute(attributeName).Value);
            }
            catch (Exception)
            {
                return defaultInt;
            }
        }

        /// <summary>
        /// Return the string value of the specified attribute name. If no attribute is found, it will return the specified default value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string GetStringAttribute(XElement node, string attributeName, string defaultString="")
        {
            try
            {
                return node.Attribute(attributeName).Value;
            }
            catch (Exception)
            {
                return defaultString;
            }
        }

        /// <summary>
        /// Return the double value of the specified attribute name. If no attribute is found, it will return the specified default value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultDouble"></param>
        /// <returns></returns>
        public static double GetDoubleAttribute(XElement node, string attributeName, double defaultDouble=0.0)
        {
            try
            {
                return double.Parse(node.Attribute(attributeName).Value);
            }
            catch (Exception)
            {
                return defaultDouble;
            }
        }

        /// <summary>
        /// Return the boolean value of the specified attribute name. If no attribute is found, it will return the specified default value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultBool"></param>
        /// <returns></returns>
        public static bool GetBoolAttribute(XElement node, string attributeName, bool defaultBool=true)
        {
            try
            {
                return bool.Parse(node.Attribute(attributeName).Value);
            }
            catch (Exception)
            {
                return defaultBool;
            }
        }

        /// <summary>
        /// Convert a hex value string to a integer representation of that value
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static int ConvertHexStringToInt(string hexString)
        {
            if(hexString.StartsWith("0x"))
            {
                return Convert.ToInt32(hexString, 16);
            }
                      
            return int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
        }


    }
}
