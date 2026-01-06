using Xunit;
using COPI_API.Models.PIGEEntities;
using System;
using System.Collections.Generic;

namespace COPI_API.Tests
{
    public class ResultadoKPIPIGETests
    {
        [Fact]
        public void ResultadoKPIPIGE_Properties_Are_Set_Correctly()
        {
            // Arrange
            var resultado = new ResultadoKPIPIGE
            {
                Id = 1,
                UnidadeKPIPIGEId = 2,
                KPIPIGEId = 3,
                CicloPIGEId = 4,
                Status = "SIM",
                DataRegistro = DateTime.UtcNow,
                Prova = "Prova Teste",
                AvaliacaoEscrita = "Avaliacao Teste"
            };

            // Assert
            Assert.Equal(1, resultado.Id);
            Assert.Equal(2, resultado.UnidadeKPIPIGEId);
            Assert.Equal(3, resultado.KPIPIGEId);
            Assert.Equal(4, resultado.CicloPIGEId);
            Assert.Equal("SIM", resultado.Status);
            Assert.Equal("Prova Teste", resultado.Prova);
            Assert.Equal("Avaliacao Teste", resultado.AvaliacaoEscrita);
        }
    }
}
