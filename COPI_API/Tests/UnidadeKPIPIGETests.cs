using Xunit;
using COPI_API.Models.PIGEEntities;
using System.Collections.Generic;

namespace COPI_API.Tests
{
    public class UnidadeKPIPIGETests
    {
        [Fact]
        public void UnidadeKPIPIGE_Can_Associate_Multiple_Servidores()
        {
            // Arrange
            var unidade = new UnidadeKPIPIGE { Id = 1, UnidadeId = 1, SEI = "123" };
            var servidor1 = new COPI_API.Models.AdminEntities.Servidor { Id = 1, Nome = "Servidor 1" };
            var servidor2 = new COPI_API.Models.AdminEntities.Servidor { Id = 2, Nome = "Servidor 2" };

            // Act
            unidade.Servidores.Add(servidor1);
            unidade.Servidores.Add(servidor2);

            // Assert
            Assert.Equal(2, unidade.Servidores.Count);
            Assert.Contains(unidade.Servidores, s => s.Nome == "Servidor 1");
            Assert.Contains(unidade.Servidores, s => s.Nome == "Servidor 2");
        }
    }
}
