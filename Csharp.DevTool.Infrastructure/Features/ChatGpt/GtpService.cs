using Dumpify;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace TestConsole;

public class GptService
{
    const string apiUrl = "https://api.openai.com/v1/chat/completions";

    private static readonly JsonSerializerOptions CamelCaseSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    readonly IConfiguration config;
    public GptService()
    {
        config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.development.json")
              .Build();
    }

    public async Task CallFunctionApi(string prompt, Properties properties, string functionName, string description)
    {
        var gptRequest = new GptRequestModel(config, prompt, properties, functionName, description);
        gptRequest.Dump();
        await SendRequest(gptRequest);
    }

    public async Task CallApi(List<string> prompts)
    {
        var gptRequest = new GptRequestModel(config, prompts);
        await SendRequest(gptRequest);
    }

    public async Task CallApi(string prompt)
    {
        var gptRequest = new GptRequestModel(config, prompt);
        await SendRequest(gptRequest);
    }

    async Task SendRequest(GptRequestModel request)
    {
        try
        {
            var response = await GetOpenAIResponse(request);
            var result = ProcessGptResponse(response);
        }
        catch (Exception ex)
        {
            Print.Error("Error: " + ex.Message);
        }
    }

    IEnumerable<string> ProcessGptResponse(string response)
    {
        if (response == null)
            throw new Exception("No content");

        var result = JsonSerializer.Deserialize<dynamic>(response, CamelCaseSerializerOptions);
        foreach (var item in result.choices)
        {
            yield return item.message.content;
        }
    }

    async Task<string> GetOpenAIResponse(GptRequestModel request)
    {
        string apiKey = config["gptkey"];

        using (HttpClient client = new HttpClient())
        {
            string requestJson = JsonSerializer.Serialize(request, CamelCaseSerializerOptions);

            // Set up the HTTP request
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Send the request
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                result.Dump();
                return result;
            }
            else
            {
                Print.Error($"Failed to get response. Status code: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                error.Dump();
                return error;
            }
        }
    }
}