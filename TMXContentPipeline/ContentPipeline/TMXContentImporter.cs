using Microsoft.Xna.Framework.Content.Pipeline;
using TMXObjects;

namespace TMXContentPipeline.ContentPipeline
{
    /// <summary>
    /// TMXContentImporter reads the TMX file and returns a TMap Object to the processor
    /// </summary>
    [ContentImporter(".tmx", DisplayName = "TMX Importer", DefaultProcessor = "TMX Processor")]
    public class TMXContentImporter : ContentImporter<TMap>
    {
        public override TMap Import(string filename, ContentImporterContext context)
        {            
            context.Logger.LogMessage("TMXContentImporter - filename: {0}", filename);
            TMap map = _TMXLoader.LoadMap(filename);            
            return map;            
        }
    }
}
