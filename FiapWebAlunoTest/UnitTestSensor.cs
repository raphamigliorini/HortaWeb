using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Usings específicos do seu projeto (ajuste conforme a localização real dos seus arquivos)
using FiapWebAluno.Controllers;
using FiapWebAluno.Service.Interface;
// Assuma a existência de ViewModels genéricas para o teste
using FiapWebAluno.Views; 

// 🚨 Nota: Assuma que você possui as ViewModels 'SensorPagedView' e 'SensorListItemView'
// e que a interface ISensorService possui o método Task<SensorPagedView<SensorListItemView>> ListarAsync(int page, int pageSize).

namespace FiapWebAlunoTest
{
    // Define um tipo genérico de retorno para o item da lista
    public class SensorListItemView { public int IdSensor { get; set; } public string Tipo { get; set; } }

    // Define um tipo genérico de retorno paginado (estrutura comum em ASP.NET Core)
    public class SensorPagedView<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

    public class SensorControllerTests
    {
        private readonly Mock<ISensorService> _mockService;
        private readonly SensorController _controller;

        public SensorControllerTests()
        {
            // Inicializa o Mock do Serviço e o Controller
            _mockService = new Mock<ISensorService>();
            _controller = new SensorController(_mockService.Object);
            
            // Inicializa o ControllerContext para evitar NullReferenceException (NRE) em métodos
            // que acessam propriedades como ModelState ou User.
            _controller.ControllerContext = new ControllerContext();
        }

        // ----------------------------------------------------------------------
        // TESTE PARA MÉTODO GET
        // ----------------------------------------------------------------------

        [Fact]
        public async Task Get_DeveRetornarOk_ComObjetoPaginado()
        {
            // ARRANGE
            var page = 1;
            var pageSize = 10;
            
            // 1. Cria os dados de teste
            var itensLista = new List<SensorListItemView>
            {
                new SensorListItemView { IdSensor = 1, Tipo = "Temperatura" },
                new SensorListItemView { IdSensor = 2, Tipo = "Umidade" }
            };

            // 2. Cria o objeto paginado esperado
            var resultPaginated = new SensorPagedView<SensorListItemView>
            {
                Page = page,            
                PageSize = pageSize,
                TotalItems = 2,         
                Items = itensLista
            };

            // 3. Configura o Mock: Quando ListarAsync for chamado com qualquer página/pageSize, retorne o objeto paginado.
            _mockService.Setup(s => s.ListarAsync(
                It.IsAny<int>(), 
                It.IsAny<int>()))
                // Usamos a sintaxe de lambda com Task.FromResult para lidar corretamente com o tipo genérico
                .Returns((int p, int ps) => Task.FromResult(resultPaginated)); 
            
            // ACT
            var resultado = await _controller.Get(page, pageSize);

            // ASSERT
            // 1. Verifica se o resultado é um OkObjectResult (Status 200)
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            
            // 2. Verifica se o valor retornado é do tipo SensorPagedView<SensorListItemView>
            var valorRetornado = Assert.IsType<SensorPagedView<SensorListItemView>>(okResult.Value);
            
            // 3. Verifica o conteúdo e a paginação
            Assert.Equal(2, valorRetornado.Items.Count());       
            Assert.Equal(page, valorRetornado.Page);             
            Assert.Equal(2, valorRetornado.TotalItems);
            
            // 4. Verifica se o método do serviço foi chamado exatamente uma vez com os parâmetros corretos
            _mockService.Verify(s => s.ListarAsync(page, pageSize), Times.Once);
        }
    }
}