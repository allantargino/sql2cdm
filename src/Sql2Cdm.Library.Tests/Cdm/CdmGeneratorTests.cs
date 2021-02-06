using Microsoft.CommonDataModel.ObjectModel.Cdm;
using Sql2Cdm.Library.Cdm;
using Sql2Cdm.Library.Models;
using Sql2Cdm.Library.Tests.Cdm.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sql2Cdm.Library.Tests.Cdm
{
    public class CdmGeneratorTests
    {
        [Fact]
        public async Task CdmManifestDefinitionIsNotNull()
        {
            CdmGenerator generator = CreateCdmGenerator();
            var model = new RelationalModel();

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.NotNull(manifest);
        }

        [Fact]
        public async Task CdmManifestDefinitionNotContainsReferences()
        {
            var generator = CreateCdmGenerator();
            var model = new RelationalModel();

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.Empty(manifest.Imports);
        }

        [Fact]
        public async Task CdmManifestDefinitionWithEmptyTablesContainsEmptyEntitiesAndRelationships()
        {
            CdmGenerator generator = CreateCdmGenerator();
            var model = new RelationalModel();

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.Empty(manifest.Entities);
            Assert.Empty(manifest.Relationships);
        }

        [Fact]
        public async Task CdmManifestDefinitionContainsEntities()
        {
            var generator = CreateCdmGenerator();
            var model = new RelationalModel()
            {
                Tables = new[] { new Table("Customer") }
            };

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.NotNull(manifest.Entities);
            Assert.Single(manifest.Entities);
        }

        [Fact]
        public async Task CdmManifestDefinitionContainsRelationships()
        {
            var generator = CreateCdmGenerator();
            var fk = new Table("Customer").WithColumn("ID", SqlDbType.Int).GetColumn();
            var table = new Table("CustomerAddresses").WithColumn("C_ID", SqlDbType.Int, foreignKey: fk);
            var model = new RelationalModel()
            {
                Tables = new[] { table }
            };

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.NotNull(manifest.Relationships);
            Assert.Single(manifest.Relationships);
        }


        [Fact]
        public async Task CdmManifestDefinitionIsValid()
        {
            var generator = CreateCdmGenerator();
            var model = new RelationalModel()
            {
                Tables = new[] { new Table("Customer") }
            };

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);
            bool isValid = manifest.Validate();

            Assert.True(isValid);
        }

        [Fact]
        public async Task CdmManifestDefinitionNotContainsModifiedTimesWhenHasTimestampsIsFalse()
        {
            var options = new CdmGenerationOptions() { HasTimestamps = false };
            var generator = CreateCdmGenerator(options);
            var model = new RelationalModel()
            {
                Tables = new[] { new Table("Customer") }
            };

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.Null(manifest.LastFileStatusCheckTime);
        }

        [Fact]
        public async Task CdmManifestDefinitionContainsModifiedTimesWhenHasTimestampsIsTrue()
        {
            var options = new CdmGenerationOptions() { HasTimestamps = true };
            var generator = CreateCdmGenerator(options);
            var model = new RelationalModel()
            {
                Tables = new[] { new Table("Customer") }
            };

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);

            Assert.NotNull(manifest.LastFileStatusCheckTime);
        }

        [Fact]
        public async Task CdmManifestDefinitionChildEntityContainsModifiedTimesWhenHasTimestampsIsTrue()
        {
            var options = new CdmGenerationOptions() { HasTimestamps = true };
            var generator = CreateCdmGenerator(options);
            var model = new RelationalModel()
            {
                Tables = new[] { new Table("Customer") }
            };

            CdmManifestDefinition manifest = await generator.GenerateCdmAsync(model);
            var child = manifest.Entities.First();

            Assert.NotNull(child.LastFileStatusCheckTime);
        }

        private CdmGenerator CreateCdmGenerator(CdmGenerationOptions options = null)
        {
            if (options == null)
            {
                options = new CdmGenerationOptions() { OutputFolder = "." };
            }
            return new CdmGenerator(options);
        }
    }
}
