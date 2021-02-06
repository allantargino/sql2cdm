using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sql2Cdm.Library.Cdm
{
    public class CdmGenerationOptions
    {
        [Required(AllowEmptyStrings = false)]
        public string ManifestName { get; set; }

        public bool OverrideExistingManifest { get; set; }

        [RegularExpression(@"^(\d+\.)?(\d+\.)?(\*|\d+)$")]
        public string EntitiesVersion { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string OutputFolder { get; set; }

        public bool HasVirtualPartition { get; set; }
        
        public bool HasTimestamps { get; set; }

        public CdmGenerationOptions()
        {
            ManifestName = "default";
            OverrideExistingManifest = false;
            EntitiesVersion = string.Empty;
            OutputFolder = ".";
            HasVirtualPartition = false;
            HasTimestamps = false;
        }
    }
}
