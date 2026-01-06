using Xunit;
using COPI_API.Models.AdminEntities;
using System;

namespace COPI_API.Tests
{
    public class ServidorTests
    {
        [Fact]
        public void Servidor_Properties_Are_Set_Correctly()
        {
            // Arrange
            var servidor = new Servidor
            {
                Id = 1,
                Nome = "Teste",
                Email = "teste@teste.com",
                UserName = "testeuser",
                PasswordHash = "hash",
                Role = "Admin",
                Celular = "123456789",
                Status = "Ativo",
                CargoOuFuncao = "Gestor",
                PontoSei = "P123",
                ChefiaImediata = "Chefe",
                RF = "RF123",
                HorarioEntrada = "08:00",
                HorarioSaida = "17:00",
                DivisaoId = 2,
                DataCriacao = DateTime.UtcNow
            };

            // Assert
            Assert.Equal(1, servidor.Id);
            Assert.Equal("Teste", servidor.Nome);
            Assert.Equal("teste@teste.com", servidor.Email);
            Assert.Equal("testeuser", servidor.UserName);
            Assert.Equal("hash", servidor.PasswordHash);
            Assert.Equal("Admin", servidor.Role);
            Assert.Equal("123456789", servidor.Celular);
            Assert.Equal("Ativo", servidor.Status);
            Assert.Equal("Gestor", servidor.CargoOuFuncao);
            Assert.Equal("P123", servidor.PontoSei);
            Assert.Equal("Chefe", servidor.ChefiaImediata);
            Assert.Equal("RF123", servidor.RF);
            Assert.Equal("08:00", servidor.HorarioEntrada);
            Assert.Equal("17:00", servidor.HorarioSaida);
            Assert.Equal(2, servidor.DivisaoId);
        }
    }
}
