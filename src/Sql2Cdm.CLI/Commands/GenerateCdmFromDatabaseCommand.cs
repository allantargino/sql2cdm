using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.Extensions.Logging;
using Sql2Cdm.Library.Cdm;
using Sql2Cdm.Library.Cdm.Extensions;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Sql.Annotations.Combiner;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sql2Cdm.CLI.Commands
{
    public class GenerateCdmFromDatabaseCommand
    {
        private readonly ILogger logger;
        private readonly IRelationalModelReader relationalModelReader;
        private readonly SqlRelationalModelAnnotationCombiner annotationCombiner;
        private readonly CdmGenerator cdmGenerator;

        public GenerateCdmFromDatabaseCommand(
            ILogger<GenerateCdmFromDatabaseCommand> logger, IRelationalModelReader relationalModelReader,
            SqlRelationalModelAnnotationCombiner annotationCombiner, CdmGenerator cdmGenerator)
        {
            this.logger = logger;
            this.relationalModelReader = relationalModelReader;
            this.annotationCombiner = annotationCombiner;
            this.cdmGenerator = cdmGenerator;
        }

        public async Task RunAsync(DatabaseOptions options)
        {
            logger.LogInformation("Reading SQL database ...");
            RelationalModel model = relationalModelReader.ReadRelationalModel();

            if (!model.Tables.Any())
            {
                logger.LogWarning("No tables were found ...");
                return;
            }

            logger.LogDebug("Running Annotation Combiner ...");
            annotationCombiner.ReadAnnotationsAndCombineWithModel(model);

            logger.LogInformation("Generating CDM folder ...");
            CdmManifestDefinition manifest = await cdmGenerator.GenerateCdmAsync(model);

            logger.LogInformation("Saving CDM folder at {cdmOutput}...", Path.GetFullPath(options.OutputCdmFolder));
            await manifest.SaveAsync();

            logger.LogInformation("Success!");
        }
    }
}
