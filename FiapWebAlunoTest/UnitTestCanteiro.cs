using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// NAMESPACES ESSENCIAIS
using FiapWebAluno.Controllers;
using FiapWebAluno.Service.Interface;

// NAMESPACE CORRIGIDO (Contém todas as ViewModels Canteiro*)
using FiapWebAluno.Views.Canteiro; 

// NAMESPACE ONDE ESTAVA CANTEIROVIEW (Agora resolvido, assumindo Canteiro é a entidade)
using FiapWebAluno.Views;
using FiapWebAluno;
using Esg.Horta.Entities; // Assumido para a entidade de retorno 'Canteiro'

// CLASSE ÚNICA DE TESTES
public class CanteirosControllerTests
{
    // VARIÁVEIS DE INSTÂNCIA (escopo correto para todos os testes)
    private readonly Mock<ICanteiroService> _mockService;
    private readonly CanteirosController _controller;
    

    // CONSTRUTOR (Inicializa as variáveis)
    public CanteirosControllerTests()
    {
        _mockService = new Mock<ICanteiroService>();
        _controller = new CanteirosController(_mockService.Object);
    }

    // ------------------------------------
    // TESTES PARA GET (Listagem Paginada)
    // ------------------------------------

    [Fact]
    public async Task Get_DeveRetornarOk_ComObjetoPaginado()
    {
        // ARRANGE
        var page = 1;
        var pageSize = 10;
        
        var itensLista = new List<CanteiroListItemView>
        {
            new CanteiroListItemView { Id = 1, Nome = "Canteiro A" },
            new CanteiroListItemView { Id = 2, Nome = "Canteiro B" }
        };

        var resultPaginated = new CanteiroPagedView<CanteiroListItemView>
        {
            Page = page,            
            PageSize = pageSize,
            TotalItems = 2,         
            Items = itensLista
        };

        // MOCK CORRETO (Resolve CS1929 com a lambda explícita)
        _mockService.Setup(s => s.ListarAsync(
                It.IsAny<int>(), 
                It.IsAny<int>()))
            // Não use ReturnsAsync. Use Returns com a lambda explícita:
            .Returns((int p, int ps) => Task.FromResult(resultPaginated));
        
        // ACT
        var resultado = await _controller.Get(page, pageSize);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        var valorRetornado = Assert.IsType<CanteiroPagedView<CanteiroListItemView>>(okResult.Value);
        
        Assert.Equal(2, valorRetornado.Items.Count());       
        Assert.Equal(page, valorRetornado.Page);             
        Assert.Equal(2, valorRetornado.TotalItems);
        
        _mockService.Verify(s => s.ListarAsync(page, pageSize), Times.Once);
    }

    public Mock<ICanteiroService> Get_mockService()
    {
        return _mockService;
    }

    // ------------------------------------
    // TESTES PARA POST (Criação)
    // ------------------------------------

    [Fact]
    public async Task Post_DeveRetornarCreated_QuandoModeloValido()
    {
        // ARRANGE
        var modeloInput = new CanteiroCreateView { Nome = "Horta Orgânica" };
        var modeloRetornoEsperado = new Canteiro { Id = 50, Nome = "Horta Orgânica" };
        

        
        // ACT
        var resultado = await _controller.Post(modeloInput);

        // ASSERT
        var createdResult = Assert.IsType<CreatedAtActionResult>(resultado);
        Assert.Equal(nameof(CanteirosController.Get), createdResult.ActionName);

        Assert.Equal(modeloRetornoEsperado.Id, createdResult.RouteValues!["id"]);

        var valorRetornado = Assert.IsType<Canteiro>(createdResult.Value);
        Assert.Equal(modeloRetornoEsperado.Nome, valorRetornado.Nome);

        _mockService.Verify(s => s.CriarAsync(modeloInput), Times.Once);
    }

    private Mock<ICanteiroService> GetMockService()
    {
        return _mockService;
    }

    [Fact]
    public async Task Post_DeveRetornarBadRequest_QuandoModeloInvalido()
    {
        // ARRANGE
        var modeloInvalido = new CanteiroCreateView { Nome = null! }; 
        
        _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");

        // ACT
        var resultado = await _controller.Post(modeloInvalido);

        // ASSERT
        Assert.IsType<BadRequestObjectResult>(resultado);
        
        _mockService.Verify(s => s.CriarAsync(It.IsAny<CanteiroCreateView>()), Times.Never);
    }
}