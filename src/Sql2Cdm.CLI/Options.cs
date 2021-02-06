using CommandLine;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Sql2Cdm.CLI
{
    [Verb("db", HelpText = "Generate CDM from an existing database.")]
    public class DatabaseOptions : BaseOptions
    {
        [Option("provider", Default = SqlProviderOption.SqlServer)]
        public SqlProviderOption SqlProvider { get; set; }

        [Option("connection-string", Required = true)]
        public string ConnectionString { get; set; }

        [Option("schema-filter", Required = false, HelpText = "Regex expression used to filter schemas.")]
        public string SchemaFilterRegexPattern { get; internal set; }
    }

    [Verb("file", HelpText = "Generate CDM from a SQL file.")]
    public class FileOptions : BaseOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input SQL file to be converted to CDM.")]
        public string InputSqlFile { get; set; }
    }

    public abstract class BaseOptions
    {
        [Option('o', "output", Required = false, Default = ".", HelpText = "Output CDM folder.")]
        public string OutputCdmFolder { get; set; }

        [Option('v', "entities-version", Required = false, Default = "")]
        public string EntitiesVersion { get; set; }

        [Option('m', "manifest-name", Required = false, Default = "default")]
        public string ManifestName { get; set; }

        [Option("override", Required = false, Default = false)]
        public bool OverrideExistingManifest { get; set; }

        [Option('t', "timestamps", Required = false, Default = false)]
        public bool HasTimestamps { get; set; }

        [Option("virtual", Required = false, Default = false, Hidden = true)]
        public bool HasVirtualPartition { get; set; }

        [Option("alias", Required = false)]
        public IEnumerable<string> Alias { get; set; }

        [Option('l', "log-level", Default = LogLevel.Information)]
        public LogLevel LogLevel { get; set; }
    }

    public enum SqlProviderOption
    {
        SqlServer,
        SQLite
    }
}
