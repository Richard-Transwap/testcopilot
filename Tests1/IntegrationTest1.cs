using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests1.Tests;

public class TodoApiIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TodoApiIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTodosReturnsOkStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/todos");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateTodoReturnsCreatedStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        var createRequest = new
        {
            Title = "Test Todo",
            Description = "Test Description"
        };

        // Act
        var response = await client.PostAsync("/todos", 
            new StringContent(System.Text.Json.JsonSerializer.Serialize(createRequest), 
                             System.Text.Encoding.UTF8, 
                             "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateAndGetTodoWorksProperly()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        var createRequest = new
        {
            Title = "Integration Test Todo",
            Description = "Created in integration test"
        };

        // Act - Create
        var createResponse = await client.PostAsync("/todos", 
            new StringContent(System.Text.Json.JsonSerializer.Serialize(createRequest), 
                             System.Text.Encoding.UTF8, 
                             "application/json"));

        // Act - Get all
        var getResponse = await client.GetAsync("/todos");
        var content = await getResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.Contains("Integration Test Todo", content);
    }
}