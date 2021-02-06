using System.IO;

namespace Sql2Cdm.Library.Sql.Text
{
    public class SqlTextScriptReader
    {
        private readonly string inputSqlFile;
        private string fileContentCache;

        public SqlTextScriptReader(string inputSqlFile)
        {
            var inputFile = Path.GetFullPath(inputSqlFile);
            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException($"SQL file '{inputFile}' does not exist!", inputFile);
            }

            this.inputSqlFile = inputFile;
        }

        public string ReadAllText()
        {
            if (string.IsNullOrWhiteSpace(fileContentCache))
            {
                fileContentCache = File.ReadAllText(inputSqlFile);
            }

            return fileContentCache;
        }
    }
}
