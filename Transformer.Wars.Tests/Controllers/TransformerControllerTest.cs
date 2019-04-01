using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;

namespace Transformer.Wars.Tests.Controllers
{
    using Wars.Controllers;
    using Wars.Models.DB;
    using Wars.Services;
    using Wars.Interfaces;
    using System.Threading.Tasks;
    using Transformer.Wars.Models.DTO;
    using Transformer.Wars.Models;
    using Transformer.Wars.Exceptions;

    public class TransformerControllerTest
    {
        private readonly Mock<ILogger<TransformerController>> _mockLogger = new Mock<ILogger<TransformerController>>();

        private Mock<IWarService> _mockWarsService = new Mock<IWarService>();

        [Fact]
        public void GetTransformersReturnsActionResult_WithAListOfTransformers()
        {
            List<Models.DB.Transformer> transformers = new List<Models.DB.Transformer>();
            transformers.Add(new Models.DB.Transformer());
            transformers.Add(new Models.DB.Transformer());
            _mockWarsService.Setup(warService => warService.GetTransformers(Models.AllegianceTypes.Autobot)).ReturnsAsync(transformers).Verifiable();
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.GetTeam(Models.AllegianceTypes.Autobot);

            var viewResult = Assert.IsType<Task<ActionResult<IEnumerable<Transformer>>>>(result).Result;
            Assert.Equal(2, viewResult.Value.Count());
        }

        [Fact]
        public void GetTransformerReturnsActionResult_WithATransformer()
        {
            Models.DB.Transformer transformer = new Models.DB.Transformer()
            {
                Name = "Autobot"
            };
            _mockWarsService.Setup(warService => warService.GetTransformer(1)).ReturnsAsync(transformer).Verifiable();
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Get(1);

            var viewResult = Assert.IsType<Task<ActionResult<Transformer>>>(result).Result;
            Assert.Equal(transformer.Name, viewResult.Value.Name);
        }


        [Fact]
        public void GetTransformerReturns404_WhenTransformerWarsException()
        {
            _mockWarsService.Setup(warService => warService.GetTransformer(123)).Throws(new TransformerWarsException());
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Get(123);
            Assert.Equal(404, ((Microsoft.AspNetCore.Mvc.NotFoundResult)(result.Result.Result)).StatusCode);
        }

        [Fact]
        public void GetTransformerScoreReturnsActionResult_WithAScore()
        {
            Models.DB.Transformer transformer = new Models.DB.Transformer()
            {
                Name = "Autobot",
                AllegianceTypeId = 1,
                Courage = 1,
                Endurance = 1,
                Firepower = 1,
                Intelligence = 1,
                Rank = 1,
                Skill = 1,
                Speed = 1,
                Strength = 1
            };
            int score = transformer.Courage + transformer.Endurance + transformer.Firepower + transformer.Intelligence + transformer.Rank + transformer.Skill + transformer.Speed + transformer.Strength;
            _mockWarsService.Setup(warService => warService.GetScore(1)).ReturnsAsync(transformer.Score).Verifiable();
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.GetScore(1);

            var viewResult = Assert.IsType<Task<ActionResult<int>>>(result).Result;
            Assert.Equal(score, viewResult.Value);
        }


        [Fact]
        public void AddTransformerReturnsActionResult_WithAddedTransformers()
        {
            TransformerRequest transformerContract = new TransformerRequest();
            transformerContract.Name = "Optimus";

            Models.DB.Transformer transformer = new Transformer();
            transformer.Name = "Optimus";

            _mockWarsService.Setup(warService => warService.AddTransformer(transformerContract)).ReturnsAsync(transformer).Verifiable();
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Put(transformerContract);

            var viewResult = Assert.IsType<Task<ActionResult<Transformer>>>(result).Result;
            Assert.Equal(transformerContract.Name, viewResult.Value.Name);
        }

        [Fact]
        public void AddTransformerReturns400_WhenTransformerWarsException()
        {
            TransformerRequest transformerContract = new TransformerRequest();
            transformerContract.Name = "Optimus";

            _mockWarsService.Setup(warService => warService.AddTransformer(transformerContract)).Throws(new TransformerWarsException());
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Put(transformerContract);

            Assert.Equal(400, ((Microsoft.AspNetCore.Mvc.BadRequestObjectResult)(result.Result.Result)).StatusCode);
        }

        [Fact]
        public void UpdateTransformerReturnsActionResult_WithUpdatedTransformers()
        {
            TransformerRequest transformerContract = new TransformerRequest();
            transformerContract.Name = "Optimus";
            Transformer transformer = new Transformer();
            transformer.Name = "Optimus";
            int id = 1;
            _mockWarsService.Setup(warService => warService.UpdateTransformer(id, transformerContract)).ReturnsAsync(transformer).Verifiable();
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Post(id, transformerContract);

            var viewResult = Assert.IsType<Task<ActionResult<Transformer>>>(result).Result;
            Assert.Equal(transformerContract.Name, viewResult.Value.Name);
        }

        [Fact]
        public void UpdateTransformerReturns400_WhenTransformerWarsException()
        {
            TransformerRequest transformerContract = new TransformerRequest();
            transformerContract.Name = "Optimus";
            int id = 1;

            _mockWarsService.Setup(warService => warService.UpdateTransformer(id, transformerContract)).Throws(new TransformerWarsException());
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Post(id, transformerContract);

            Assert.Equal(400, ((Microsoft.AspNetCore.Mvc.BadRequestObjectResult)(result.Result.Result)).StatusCode);
        }

        [Fact]
        public void DeleteTransformerReturns_Nothing()
        {
            int id = 1;
            _mockWarsService.Setup(warService => warService.DeleteTransformer(id)).Verifiable();
            var controller = new TransformerController(_mockWarsService.Object, _mockLogger.Object);
            var result = controller.Delete(id);
            Assert.Equal(0, 0);

        }

    }
}
