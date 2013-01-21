using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TMXContentPipeline.ContentPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class TMXContentWriter : ContentTypeWriter<MapContent>
    {
        /// <summary>
        /// Output a binary .xnb format with all the information
        /// </summary>
        /// <param name="output"></param>
        /// <param name="value"></param>
        protected override void Write(ContentWriter output, MapContent value)
        {            
            output.Write(value.Version);
            output.Write((int)value.Orientation);
            output.Write(value.Width);
            output.Write(value.Height);
            output.Write(value.TileWidth);
            output.Write(value.TileHeight);

            output.Write(value.LayerBaseList.Count);
            foreach (LayerBaseContent layer in value.LayerBaseList)
            {
                output.WriteObject<LayerBaseContent>(layer);
            }

        }         

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "TMXMapEditorTest.TMXContentReader, TMXMapEditorTest";
        }
    }

}
