using System;
using Xunit;
namespace Transformer.Wars.Tests
{
    public class TransformerControllerTest
    {
        [Fact]
        public void GetTransformerReturnsActionResult_WithAListOfTransformers()
        {
            var controller = new TransformerController();

            // Act
            var result = await controller.Get();

            // Assert
            var viewResult = Assert.IsType<ActionResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}
