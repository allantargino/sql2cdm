namespace Sql2Cdm.Library.Cdm
{
    public class CdmReferenceResolver
    {
        private readonly string manifestName;
        private readonly string version;

        public CdmReferenceResolver() { }

        public CdmReferenceResolver(string manifestName, string version)
        {
            this.manifestName = manifestName;
            this.version = version;
        }

        public string GetDocumentFileName(string entityName)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return $"{entityName}.cdm.json";
            }
            else
            {
                return $"{entityName}.{version}.cdm.json";
            }
        }

        public string GetManifestName()
        {
            if (string.IsNullOrWhiteSpace(manifestName))
            {
                return "default";
            }
            else
            {
                return manifestName.Replace(" ", "");
            }
        }

        public string GetManifestFileName()
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return GetManifestName() + ".manifest.cdm.json";

            }
            else
            {
                return GetManifestName() + $".{version}.manifest.cdm.json";

            }
        }
    }
}
