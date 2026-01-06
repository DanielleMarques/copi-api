using Xunit;
using COPI_API.Models.PIGEEntities;
using System;

namespace COPI_API.Tests
{
    public class EixoPIGETests
    {
        [Fact]
        public void EixoPIGE_Properties_Are_Set_Correctly()
        {
            // Arrange
            var eixo = new EixoPIGE
            {
                Id = 1,
                Nome = "Eixo Teste",
                Peso = 2.5m
            };

            // Assert
            Assert.Equal(1, eixo.Id);
            Assert.Equal("Eixo Teste", eixo.Nome);
            Assert.Equal(2.5m, eixo.Peso);
        }
    }
}
