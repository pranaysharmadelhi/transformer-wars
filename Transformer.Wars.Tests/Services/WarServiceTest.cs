using System;
using Microsoft.EntityFrameworkCore;
using Transformer.Wars.Models.DB;
using Xunit;
using Moq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Transformer.Wars.Tests.Services
{
    using System.Linq;
    using Transformer.Wars.Controllers;
    using Transformer.Wars.Exceptions;
    using Transformer.Wars.Models;
    using Wars.Services;

    public class WarServiceTest
    {
        private readonly Mock<ILogger<WarService>> _mockLogger = new Mock<ILogger<WarService>>();

        private TransformerWarsContext getDBContext()
        {
            var options = new DbContextOptionsBuilder<TransformerWarsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TransformerWarsContext(options);
        }

        [Fact]
        public void GetTransformersReturns_WithAListOfTransformers()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "Autobot",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "Decepticon",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                context.Transformers.Add(autoBotTransformer);
                context.Transformers.Add(decepticonTransformer);
                context.SaveChanges();

                var warsService = new WarService(context, _mockLogger.Object);
                var autoBotResult = warsService.GetTransformers(Models.AllegianceTypes.Autobot).Result;
                Assert.Collection(autoBotResult, item => Assert.Contains("Autobot", item.Name));
                var decepticonResult = warsService.GetTransformers(Models.AllegianceTypes.Decepticon).Result;
                Assert.Collection(decepticonResult, item => Assert.Contains("Decepticon", item.Name));
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void GetTransformerReturns_WithATransformer()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "Autobot",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };


                context.Transformers.Add(autoBotTransformer);
                context.SaveChanges();

                var warsService = new WarService(context, _mockLogger.Object);
                var autoBotResult = warsService.GetTransformer(autoBotTransformer.TransformerId).Result;
                Assert.Equal(autoBotTransformer.Name, autoBotResult.Name);
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void GetTransformerReturns_Error_WhenNotFound()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Assert.ThrowsAsync<TransformerWarsException>(() => warsService.GetTransformer(123));
                context.Database.EnsureDeleted();

            }
        }

        [Fact]
        public void PutTransformerReturns_WithAddedTransformer()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var transformerResult = warsService.AddTransformer(transformer).Result;

                Assert.Equal(context.Transformers.First().Name, transformer.Name);
                Assert.Equal(transformerResult.Name, transformer.Name);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void PutTransformerReturns_WithError_WhenDuplicateRank()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#1",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                warsService.AddTransformer(transformer).Wait();

                transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#2",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Assert.ThrowsAsync<TransformerWarsException>(() => warsService.AddTransformer(transformer));
                context.Database.EnsureDeleted();

            }
        }



        [Fact]
        public void PutTransformerReturns_WithError_WhenDuplicateName()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#1",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                warsService.AddTransformer(transformer).Wait();

                transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#1",
                    Rank = 2,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Assert.ThrowsAsync<TransformerWarsException>(() => warsService.AddTransformer(transformer));
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void PostTransformerReturns_WithUpdatedTransformer()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var addTransformerResult = warsService.AddTransformer(transformer).Result;
                transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Decepticon",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };
                var transformerResult = warsService.UpdateTransformer(context.Transformers.First().TransformerId, transformer).Result;
                Assert.Equal(context.Transformers.First().Name, transformer.Name);
                Assert.Equal(transformerResult.Name, transformer.Name);
                Assert.Equal(transformerResult.Allegiance, AllegianceTypes.Decepticon);


                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void PostTransformerReturns_Error_WhenNotFound()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };
                Assert.ThrowsAsync<TransformerWarsException>(() => warsService.UpdateTransformer(123, transformer));
                context.Database.EnsureDeleted();
            }
        }



        [Fact]
        public void PostTransformerReturns_WithError_WhenDuplicateRank()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#1",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                warsService.AddTransformer(transformer).Wait();

                transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#2",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Assert.ThrowsAsync<TransformerWarsException>(() => warsService.UpdateTransformer(2, transformer));
                context.Database.EnsureDeleted();

            }
        }



        [Fact]
        public void PostTransformerReturns_WithError_WhenDuplicateName()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#1",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                warsService.AddTransformer(transformer).Wait();

                transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot#1",
                    Rank = 2,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Assert.ThrowsAsync<TransformerWarsException>(() => warsService.UpdateTransformer(2, transformer));
                context.Database.EnsureDeleted();

            }
        }



        [Fact]
        public void DeleteTransformerReturns_Nothing()
        {
            using (var context = getDBContext())
            {
                var warsService = new WarService(context, _mockLogger.Object);
                Models.DTO.TransformerRequest transformer = new Models.DTO.TransformerRequest()
                {
                    AllegianceType = AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Name = "Autobot",
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var addTransformerResult = warsService.AddTransformer(transformer).Result;

                var transformerResult = warsService.DeleteTransformer(context.Transformers.First().TransformerId);
                Assert.Equal(0, context.Transformers.Count());

                context.Database.EnsureDeleted();
            }
        }



        [Fact]
        public void RunBattleReturns_WithOptimus_WhenNameIsOptimus()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "Optimus",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "Test",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 10,
                    Endurance = 10,
                    Firepower = 10,
                    Intelligence = 10,
                    Rank = 10,
                    Skill = 10,
                    Speed = 10,
                    Strength = 10
                };

                var warsService = new WarService(context, _mockLogger.Object);
                var winnerResult = warsService.GetWinner(autoBotTransformer, decepticonTransformer);
                Assert.Equal(AllegianceTypes.Autobot, winnerResult);
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void RunBattleReturns_WithPredaking_WhenNameIsOptimus()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "Test",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 10,
                    Endurance = 10,
                    Firepower = 10,
                    Intelligence = 10,
                    Rank = 10,
                    Skill = 10,
                    Speed = 10,
                    Strength = 10
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "Predaking",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var warsService = new WarService(context, _mockLogger.Object);
                var winnerResult = warsService.GetWinner(autoBotTransformer, decepticonTransformer);
                Assert.Equal(AllegianceTypes.Decepticon, winnerResult);
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void RunWarsReturns_WithNoVictor_WhenNameIsOptimusVsPredaking()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "Optimus",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 10,
                    Endurance = 10,
                    Firepower = 10,
                    Intelligence = 10,
                    Rank = 10,
                    Skill = 10,
                    Speed = 10,
                    Strength = 10
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "Predaking",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };
                context.Transformers.Add(autoBotTransformer);
                context.Transformers.Add(decepticonTransformer);
                context.SaveChanges();


                var warsService = new WarService(context, _mockLogger.Object);
                var warResult = warsService.RunWar().Result;
                Assert.Equal(0, warResult.Autobots.Victors.Count);
                Assert.Equal(0, warResult.Decepticon.Victors.Count);

                context.Database.EnsureDeleted();

            }
        }



        [Fact]
        public void RunWarsReturns_WithResult()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer transformer = new Models.DB.Transformer()
                {
                    Name = "Optimus",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 10,
                    Endurance = 9,
                    Firepower = 1,
                    Intelligence = 3,
                    Rank = 1,
                    Skill = 1,
                    Speed = 4,
                    Strength = 2
                };
                context.Transformers.Add(transformer);

                transformer = new Models.DB.Transformer()
                {
                    Name = "Decepticon#4",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Rank = 4,
                    Strength = 10,
                    Intelligence = 9,
                    Speed = 1,
                    Endurance = 2,
                    Courage = 3,
                    Firepower = 4,
                    Skill = 5
                };
                context.Transformers.Add(transformer);

                transformer = new Models.DB.Transformer()
                {
                    Name = "Autobot#10",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Rank = 10,
                    Strength = 10,
                    Intelligence = 9,
                    Speed = 1,
                    Endurance = 2,
                    Courage = 3,
                    Firepower = 4,
                    Skill = 5
                };
                context.Transformers.Add(transformer);

                context.SaveChanges();


                var warsService = new WarService(context, _mockLogger.Object);
                var warResult = warsService.RunWar().Result;
                Assert.Equal(0, warResult.Autobots.Losers.Count);
                Assert.Equal(1, warResult.Autobots.Victors.Count);
                Assert.Equal(1, warResult.Autobots.Survivors.Count);
                Assert.Equal(1, warResult.Decepticon.Losers.Count);
                Assert.Equal(0, warResult.Decepticon.Victors.Count);
                Assert.Equal(0, warResult.Decepticon.Survivors.Count);

                context.Database.EnsureDeleted();

            }
        }

        [Fact]
        public void RunBattleReturns_WithTransformerWithStrengthDifferenceOf3AndCourageOfLessThan5()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "TestA",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 4
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "TestB",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 4,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var warsService = new WarService(context, _mockLogger.Object);
                var winnerResult = warsService.GetWinner(autoBotTransformer, decepticonTransformer);
                Assert.Equal(AllegianceTypes.Autobot, winnerResult);
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void RunBattleReturns_WithTransformerWithSkillDifferenceOf5OrMore()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "TestA",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "TestB",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 6,
                    Speed = 1,
                    Strength = 1
                };

                var warsService = new WarService(context, _mockLogger.Object);
                var winnerResult = warsService.GetWinner(autoBotTransformer, decepticonTransformer);
                Assert.Equal(AllegianceTypes.Decepticon, winnerResult);
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void RunBattleReturns_WithTransformerWithHigherScore()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "TestA",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 2,
                    Endurance = 2,
                    Firepower = 2,
                    Intelligence = 2,
                    Rank = 2,
                    Skill = 2,
                    Speed = 2,
                    Strength = 2
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "TestB",
                    AllegianceTypeId = (int)AllegianceTypes.Decepticon,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var warsService = new WarService(context, _mockLogger.Object);
                var winnerResult = warsService.GetWinner(autoBotTransformer, decepticonTransformer);
                Assert.Equal(AllegianceTypes.Autobot, winnerResult);
                context.Database.EnsureDeleted();

            }
        }


        [Fact]
        public void RunBattleReturns_ErrorWithSameAllegiance()
        {
            using (var context = getDBContext())
            {
                Models.DB.Transformer autoBotTransformer = new Models.DB.Transformer()
                {
                    Name = "TestA",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 1,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                Models.DB.Transformer decepticonTransformer = new Models.DB.Transformer()
                {
                    Name = "TestB",
                    AllegianceTypeId = (int)AllegianceTypes.Autobot,
                    Courage = 1,
                    Endurance = 1,
                    Firepower = 1,
                    Intelligence = 1,
                    Rank = 2,
                    Skill = 1,
                    Speed = 1,
                    Strength = 1
                };

                var warsService = new WarService(context, _mockLogger.Object);
                Assert.Throws<Exception>(() => warsService.GetWinner(autoBotTransformer, decepticonTransformer)); 
                context.Database.EnsureDeleted();

            }
        }
    }
}
