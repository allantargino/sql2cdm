using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sql2Cdm.CLI.Commands;
using Sql2Cdm.Library.Cdm;
using Sql2Cdm.Library.Interfaces;
using Sql2Cdm.Library.Sql;
using Sql2Cdm.Library.Sql.Sqlite;
using Sql2Cdm.Library.Sql.SqlServer;
using Sql2Cdm.Library.Sql.Annotations.Combiner;
using Sql2Cdm.Library.Sql.Text;
using Sql2Cdm.Library.Sql.Annotations.Alias;

namespace Sql2Cdm.CLI
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureCommonServices(this IServiceCollection services, BaseOptions cliOptions)
        {
            services.AddLogging(configure => configure.AddConsole())
                    .Configure<LoggerFilterOptions>(o => o.MinLevel = cliOptions.LogLevel);

            services.AddScoped<SqlRelationalModelAnnotationCombiner>();

            services.AddScoped<CdmGenerator>();

            services.AddOptions<CdmGenerationOptions>()
                    .Configure(MapCliOptionsToCdmGenerationOptions(cliOptions))
                    .ValidateDataAnnotations();

            services.AddScoped<SqlAnnotationTypeAliasMapper>();

            services.AddOptions<SqlAnnotationTypeAliasOptions>()
                    .Configure(o => o.Alias = cliOptions.Alias)
                    .ValidateDataAnnotations();


            return services;
        }

        private static Action<CdmGenerationOptions> MapCliOptionsToCdmGenerationOptions(BaseOptions cliOptions)
        {
            return (genOptions) =>
            {
                genOptions.ManifestName = cliOptions.ManifestName;
                genOptions.OverrideExistingManifest = cliOptions.OverrideExistingManifest;
                genOptions.EntitiesVersion = cliOptions.EntitiesVersion;
                genOptions.OutputFolder = cliOptions.OutputCdmFolder;
                genOptions.HasVirtualPartition = cliOptions.HasVirtualPartition;
                genOptions.HasTimestamps = cliOptions.HasTimestamps;
            };
        }

        public static IServiceCollection ConfigureDatabaseCommandServices(this IServiceCollection services, DatabaseOptions cliOptions)
        {
            if (cliOptions.SqlProvider == SqlProviderOption.SQLite)
            {
                services.AddSingleton<DbConnection>(new SqliteConnection(cliOptions.ConnectionString));
                services.AddScoped<IRelationalModelReader, SqliteRelationalModelReader>();
                services.AddScoped<ITypeValueAnnotationsReader, SqliteTypeValueAnnotationsReader>();
            }
            else if (cliOptions.SqlProvider == SqlProviderOption.SqlServer)
            {
                services.AddSingleton<DbConnection>(new SqlConnection(cliOptions.ConnectionString));
                services.AddScoped<IRelationalModelReader, SqlServerRelationalModelReader>();
                services.Configure<SqlRelationalModelReaderOptions>(c => c.SchemaFilterRegexPattern = cliOptions.SchemaFilterRegexPattern);
                services.AddScoped<ITypeValueAnnotationsReader, SqlServerTypeValueAnnotationsReader>();
            }

            services.AddScoped<GenerateCdmFromDatabaseCommand>();

            return services;
        }

        public static IServiceCollection ConfigureFileCommandServices(this IServiceCollection services, FileOptions cliOptions)
        {
            services.AddSingleton<SqlTextScriptReader>(new SqlTextScriptReader(cliOptions.InputSqlFile));
            services.AddScoped<SqlTextCommandAdapter>();

            services.AddSingleton<DbConnection>(new SqliteConnection("Filename=:memory:"));
            services.AddScoped<IRelationalModelReader, SqliteRelationalModelReader>();
            services.AddScoped<ITypeValueAnnotationsReader, SqlTextTypeValueAnnotationReader>();

            services.AddScoped<GenerateCdmFromFileCommand>();

            return services;
        }

    }
}
