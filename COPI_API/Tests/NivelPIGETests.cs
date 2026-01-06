using Xunit;
using COPI_API.Models.PIGEEntities;
using System;

namespace COPI_API.Tests
{
    public class NivelPIGETests
    {
        [Fact]
        public void NivelPIGE_Properties_Are_Set_Correctly()
        {
            // Arrange
            var nivel = new NivelPIGE
            {
                Id = 1,
                Nome = "Nivel Teste",
                Valor = 1.5m
            };

            // Assert
            Assert.Equal(1, nivel.Id);
            Assert.Equal("Nivel Teste", nivel.Nome);
            Assert.Equal(1.5m, nivel.Valor);
        }
    }
}
