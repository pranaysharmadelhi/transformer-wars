using System;
using Xunit;
namespace Transformer.Wars.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class TransformerControllerTest
    {
        [Fact]
        public void GetTransformerReturnsActionResult_WithAListOfTransformers()
        {
            var controller = new TransformerController();

            // Act
            var result = controller.Get();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(
                viewResult.Value);
            Assert.Equal(2, model.ToList().Count);
        }
    }
}
