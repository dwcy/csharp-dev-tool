using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace TestConsole;

public class GptService
{
    readonly IConfiguration config;
    public GptService()
    {
        config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();
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
            Print.Info("ChatGPT's response: ");
            foreach (var item in result)
            {
                Print.Code(item);
            }
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

        dynamic json = JsonConvert.DeserializeObject(response);
        foreach (var item in json.choices)
        {
            yield return item.message.content;
        }
    }

    async Task<string> GetOpenAIResponse(GptRequestModel request)
    {
        string apiKey = config["gptkey"];

        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            string requestJson = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            // Set up the HTTP request
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Send the request
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Print.Error($"Failed to get response. Status code: {response.StatusCode}");
                Print.Normal(await response.Content.ReadAsStringAsync());
            }

            return string.Empty;
        }
    }
}