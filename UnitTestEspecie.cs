using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc; // Necessário para ActionResult, NoContentResult (resolve CS0246)
using System.Threading.Tasks;
// Adicione os usings específicos do seu projeto
using FiapWebAluno.Controllers;
using FiapWebAluno.Service.Interface; 
using static Moq.Times; // Tenta resolver o 'Times' (CS0103 na Linha 26)

namespace FiapWebAlunoTest
{
    // Adicione um nome de classe mais claro se 'Test1' for genérico, ex: CanteirosControllerDeleteTests
    public class Test1 
    {
        // 1. DECLARAÇÃO DOS CAMPOS (Resolve CS0103 para _mockService e _controller)
        private readonly Mock<ICanteiroService> _mockService;
        private readonly CanteirosController _controller;

        // Construtor
        public Test1()
        {
            _mockService = new Mock<ICanteiroService>();
            _controller = new CanteirosController(_mockService.Object);
            // Inicializa o ControllerContext para evitar NRE em ModelState
            _controller.ControllerContext = new ControllerContext(); 
        }

        // Exemplo do teste que estava causando erros de escopo/dependência
        [Fact]
        public async Task Delete_DeveRetornarNoContent_QuandoExclusaoBemSucedida()
        {
            // ARRANGE
            var idCanteiro = 10;
            
            _mockService.Setup(s => s.ExcluirAsync(idCanteiro))
                .ReturnsAsync(true);

            // ACT
            var resultado = await _controller.Delete(idCanteiro);

            // ASSERT
            // NoContentResult agora deve ser reconhecido (resolve CS0246)
            Assert.IsType<NoContentResult>(resultado);
            
            // Times agora deve ser reconhecido (resolve CS0103)
            _mockService.Verify(s => s.ExcluirAsync(idCanteiro), Times.Once); 
        }
    }
}