using TFPAW.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Configuration;


public class ChatService : IChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = "";
        /*Antigua API KEY sk-YVzge0occrSwKstSqVaa8KjCwveOWoRoIQqHZrK9INT3BlbkFJ2uRyfuW-2FSwF2VVwKKahiZWwNuHYFcxv31DUrC7IA*/

    }

    public async Task<string> SendMessageAsync(string message)
{
    var requestBody = new
    {
        model = "gpt-3.5-turbo",
        messages = new[]
        {
            new { role = "user", content = message }
        }
    };

    var jsonRequest = JsonSerializer.Serialize(requestBody);
    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

    var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

    if (!response.IsSuccessStatusCode)
    {
        return $"Error en la solicitud: {response.StatusCode}";
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();
    Console.WriteLine("JSON de respuesta: " + jsonResponse); // Imprime el JSON para depuración

    if (string.IsNullOrWhiteSpace(jsonResponse))
    {
        return "La respuesta de la API de GPT está vacía.";
    }

    try
    {
        var responseObject = JsonSerializer.Deserialize<GptResponse>(jsonResponse);

        if (responseObject?.Choices == null || !responseObject.Choices.Any())
        {
            return "No se recibieron opciones válidas en la respuesta.";
        }

        var messageContent = responseObject.Choices.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrWhiteSpace(messageContent))
        {
            return "No se recibió una respuesta válida de la API de GPT.";
        }

        // Verifica si la respuesta contiene información sobre gatos
        if (!IsResponseAboutCats(messageContent))
        {
            return "Lo siento, solo puedo responder preguntas sobre gatos.";
        }

        return messageContent;
    }
    catch (JsonException ex)
    {
        return $"Error al deserializar la respuesta: {ex.Message}";
    }
}

private bool IsResponseAboutCats(string content)
{
    // Puedes mejorar esta lógica según sea necesario
    return content.Contains("gato", StringComparison.OrdinalIgnoreCase) ||
           content.Contains("felino", StringComparison.OrdinalIgnoreCase) ||
           content.Contains("kitten", StringComparison.OrdinalIgnoreCase);
}


}
