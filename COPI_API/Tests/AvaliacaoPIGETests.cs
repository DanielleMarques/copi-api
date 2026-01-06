using Xunit;
using COPI_API.Models.PIGEEntities;
using System;
using System.Collections.Generic;

namespace COPI_API.Tests
{
    public class AvaliacaoPIGETests
    {
        [Fact]
        public void AvaliacaoPIGE_Properties_Are_Set_Correctly()
        {
            // Arrange
            var avaliacao = new AvaliacaoPIGE
            {
                Id = 1,
                DataCriacao = DateTime.UtcNow,
                NivelGerenciadoAtingido = true,
                NivelIntegradoAtingido = false,
                NivelPadronizadoAtingido = true,
                NotaIMPIGE = 9.5m,
                NotaNG = 8.0m,
                NotaNI = 7.0m,
                NotaNP = 6.0m,
                Resultados = new List<ResultadoKPIPIGE>()
            };

            // Assert
            Assert.Equal(1, avaliacao.Id);
            Assert.True(avaliacao.NivelGerenciadoAtingido);
            Assert.False(avaliacao.NivelIntegradoAtingido);
            Assert.True(avaliacao.NivelPadronizadoAtingido);
            Assert.Equal(9.5m, avaliacao.NotaIMPIGE);
            Assert.Equal(8.0m, avaliacao.NotaNG);
            Assert.Equal(7.0m, avaliacao.NotaNI);
            Assert.Equal(6.0m, avaliacao.NotaNP);
            Assert.NotNull(avaliacao.Resultados);
        }
    }
}
