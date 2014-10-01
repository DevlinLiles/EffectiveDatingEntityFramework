using System;
using System.Data.Entity;
using System.Linq;
using EffectiveDating.Core.Entities.Domain;
using EffectiveDating.Core.Maps;
using EffectiveDating.Tests.Properties;
using FluentAssertions;
using Highway.Data;
using Highway.Data.Repositories;
using NUnit.Framework;

namespace EffectiveDating.Tests
{
    [TestFixture]
    public class PhysicalAssetTests
    {
        [Test]
        public void ShouldReturnPhysicalAssets()
        {
            //Arrange
            var target = new DomainContext<Domain>(new Domain(Settings.Default.Connection));
            
            //Act
            var dateTime = DateTime.Now;
            var physicalAsset = new PhysicalAsset
            {
                Name = "Test",
                EffectiveFrom = dateTime,
                EffectiveTo = dateTime.AddDays(30),
                ResourceId = 1
            };
            target.Add(physicalAsset);
            target.Commit();

            var query = new GetById<long, PhysicalAsset>(physicalAsset.Id);
            var results = query.Execute(target);

            //Assert
            results.Name.Should().Be("Test");
            results.EffectiveTo.Should().Be(dateTime.AddDays(30));
            results.EffectiveFrom.Should().Be(dateTime);
            results.TestEffectiveThing.Should().BeNullOrWhiteSpace();
        }

        [Test]
        public void ShouldModifyADetailByAddingANewOne()
        {
            //Arrange
            var domain = new Domain(Settings.Default.Connection);
            var target = new DomainRepository<Domain>(new DomainContext<Domain>(domain),domain);

            //Act
            var dateTime = DateTime.Now;
            var physicalAsset = new PhysicalAsset
            {
                Name = "Test",
                EffectiveFrom = dateTime,
                EffectiveTo = dateTime.AddDays(30),
                ResourceId = 1
            };
            target.Context.Add(physicalAsset);
            target.Context.Commit();

            physicalAsset.TestEffectiveThing = "Test";
            target.Context.Commit();
            
            //Assert
            var context = new DomainContext<HistoricalDomain>(new HistoricalDomain(Settings.Default.Connection));
            var asset = context.AsQueryable<Core.Entities.Historical.PhysicalAsset>().Include(x => x.PhysicalAssetEffectives).SingleOrDefault(x => x.Id == physicalAsset.Id);

            asset.PhysicalAssetEffectives.Should().HaveCount(2);
        }

        //TODO : Add Test for Effective Dating Included Collections and Relationships
    }
}