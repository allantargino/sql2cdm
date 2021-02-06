using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Microsoft.CommonDataModel.ObjectModel.Enums;
using Microsoft.CommonDataModel.ObjectModel.Storage;
using System;
using System.Threading.Tasks;

namespace Sql2Cdm.Library.Cdm.Extensions
{
    public static class CdmExtensions
    {
        private const string foundationJsonPath = "cdm:/foundations.cdm.json";

        public static CdmFolderDefinition GetLocalRootFolder(this CdmCorpusDefinition cdmCorpus, string path)
        {
            var @namespace = "local";
            cdmCorpus.Storage.Mount(@namespace, new LocalAdapter(path));
            return cdmCorpus.Storage.FetchRootFolder(@namespace);
        }

        public static CdmManifestDefinition CreateCdmManifest(this CdmCorpusDefinition corpus, string name)
        {
            return corpus.MakeObject<CdmManifestDefinition>(CdmObjectType.ManifestDef, name);
        }

        public static CdmDocumentDefinition CreateCdmDocument(this CdmCorpusDefinition corpus, string name)
        {
            var document = corpus.MakeObject<CdmDocumentDefinition>(CdmObjectType.DocumentDef, name);
            document.Imports.Add(foundationJsonPath);
            return document;
        }

        public static async Task SaveAsync(this CdmManifestDefinition manifest)
        {
            bool success = await manifest.SaveAsAsync(manifest.Name, saveReferenced: true);

            if (!success)
            {
                throw new InvalidOperationException("CDM was not generated, please check the logs.");
            }
        }
    }
}
