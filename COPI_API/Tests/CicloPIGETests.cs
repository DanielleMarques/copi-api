using Xunit;
using COPI_API.Models.PIGEEntities;
using System;
using System.Collections.Generic;

namespace COPI_API.Tests
{
    public class CicloPIGETests
    {
        [Fact]
        public void CicloPIGE_Properties_Are_Set_Correctly()
        {
            // Arrange
            var ciclo = new CicloPIGE
            {
                Id = 1,
                Nome = "Ciclo Teste",
                Encerrado = true,
                DataInicio = DateTime.UtcNow.AddDays(-10),
                DataFim = DateTime.UtcNow
            };

            // Assert
            Assert.Equal(1, ciclo.Id);
            Assert.Equal("Ciclo Teste", ciclo.Nome);
            Assert.True(ciclo.Encerrado);
            Assert.NotNull(ciclo.DataInicio);
            Assert.NotNull(ciclo.DataFim);
        }
    }
}
