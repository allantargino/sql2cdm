using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Microsoft.Extensions.Options;
using Sql2Cdm.Library.Cdm.Extensions;
using Sql2Cdm.Library.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System;

namespace Sql2Cdm.Library.Cdm
{
    public class CdmGenerator
    {
        private readonly CdmGenerationOptions options;

        public CdmGenerator(IOptions<CdmGenerationOptions> options) : this(options.Value) { }

        public CdmGenerator(CdmGenerationOptions options)
        {
            this.options = options;
        }

        public async Task<CdmManifestDefinition> GenerateCdmAsync(RelationalModel model)
        {
            CdmCorpusDefinition corpus = new CdmCorpusDefinition();
            CdmReferenceResolver resolver = new CdmReferenceResolver(options.ManifestName, options.EntitiesVersion);

            var outputManifestFile = Path.Combine(Path.GetFullPath(options.OutputFolder), resolver.GetManifestFileName());
            if (File.Exists(outputManifestFile))
            {
                if (options.OverrideExistingManifest)
                {
                    File.Delete(outputManifestFile);
                }
                else
                {
                    throw new Exception($"Manifest {outputManifestFile} already exists. Please use the override option.");
                }
            }

            CdmFolderDefinition folder = corpus.GetLocalRootFolder(options.OutputFolder);
            
            CdmManifestDefinition manifest = corpus.CreateCdmManifest(resolver.GetManifestName());
            folder.Documents.Add(manifest);

            foreach (var table in model.Tables)
            {
                string documentName = resolver.GetDocumentFileName(table.Name);
                CdmDocumentDefinition entityDocument = corpus.CreateCdmDocument(documentName);
                folder.Documents.Add(entityDocument);

                var entityGenerator = new CdmEntityGenerator(corpus, resolver, entityDocument, options.EntitiesVersion);
                CdmEntityDefinition entity = entityGenerator.GenerateEntity(table);
                manifest.Entities.Add(entity);

                var relationshipGenerator = new CdmEntityRelationshipGenerator(corpus, resolver);
                IEnumerable<CdmE2ERelationship> relationships = relationshipGenerator.GenerateRelationships(table);
                manifest.Relationships.AddRange(relationships);
            }
            
            if (options.HasTimestamps)
            {
                await manifest.FileStatusCheckAsync();
            }
            
            if (options.HasVirtualPartition)
            {
                CreateVirtualPartitionOnEntities(corpus, manifest);
            }

            return manifest;
        }

        private void CreateVirtualPartitionOnEntities(CdmCorpusDefinition corpus, CdmManifestDefinition defaultManifest)
        {
            foreach (CdmEntityDeclarationDefinition entityDef in defaultManifest.Entities)
            {
                var partition = corpus.MakeObject<CdmDataPartitionDefinition>(CdmObjectType.DataPartitionDef, entityDef.EntityName);
                partition.Location = $"virtual://{entityDef.EntityName}";

                entityDef.DataPartitions.Add(partition);
            }
        }
    }
}
