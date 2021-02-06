using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sql2Cdm.CLI.Commands;

namespace Sql2Cdm.CLI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var commandLineParser = new Parser(with =>
            {
                with.CaseInsensitiveEnumValues = true;
                with.HelpWriter = null;
            });

            var results = commandLineParser.ParseArguments<DatabaseOptions, FileOptions>(args);

            results.WithNotParsed(errors => DisplayHelp(results, errors));

            try
            {
                await results.WithParsedAsync<DatabaseOptions>(ConfigureAndRunDatabaseCommandAsync);
                await results.WithParsedAsync<FileOptions>(ConfigureAndRunFileCommandAsync);
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            if (errs.IsVersion() || errs.IsHelp())
            {
                Console.WriteLine(HelpText.AutoBuild(result));
            }
            else
            {
                var helpText = HelpText.AutoBuild(result, h =>
                {
                    h.AdditionalNewLineAfterOption = false;
                    h.AddEnumValuesToHelpText = true;
                    
                    if (errs.Any(e => e.Tag == ErrorType.NoVerbSelectedError))
                    {
                        return h.AddPreOptionsLine("\nERROR:\n\tPlease select a sub-command");
                    }
                    else
                    {
                        return HelpText.DefaultParsingErrorsHandler(result, h);
                    }
                }, e => e);
                Console.Error.WriteLine(helpText);
                Environment.Exit(1);
            }
        }

        private static void DisplayException(Exception ex)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            Console.ForegroundColor = currentColor;
            
            Environment.Exit(1);
        }

        private static async Task ConfigureAndRunDatabaseCommandAsync(DatabaseOptions options)
        {
            var command = new ServiceCollection()
                                .ConfigureCommonServices(options)
                                .ConfigureDatabaseCommandServices(options)
                                .BuildServiceProvider()
                                .GetService<GenerateCdmFromDatabaseCommand>();

            await command.RunAsync(options);
        }

        private static async Task ConfigureAndRunFileCommandAsync(FileOptions options)
        {
            var command = new ServiceCollection()
                                .ConfigureCommonServices(options)
                                .ConfigureFileCommandServices(options)
                                .BuildServiceProvider()
                                .GetService<GenerateCdmFromFileCommand>();

            await command.RunAsync(options);
        }
    }
}