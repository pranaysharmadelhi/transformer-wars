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

    public class WarsControllerTest
    {
        private readonly Mock<ILogger<WarsController>> _mockLogger = new Mock<ILogger<WarsController>>();

        private Mock<IWarService> _mockWarsService = new Mock<IWarService>();

        [Fact]
        public void RunWarsReturnsActionResult_WithAListOfTransformers()
        {
        }
    }
}
