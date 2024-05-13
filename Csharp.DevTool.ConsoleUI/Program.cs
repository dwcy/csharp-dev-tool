using Csharp.DevTool.Infrastructure.Features;
using Csharp.DevTool.Infrastructure.Features.ConsoleTools;
using System.Text;
using System.Text.RegularExpressions;
using TestConsole;

Print.Ascii("Code haxx!");
//ConsoleRadarCharts.TestChart();
//await TestPrompt();
//await RunGPTDemo();
await RunGptFunctionDemo();
//RunCodeScanDemo();
Console.ReadKey();


async Task RunGptFunctionDemo()
{
    var gptService = new GptService();
    var prompt = "What's the weather like in San Francisco, Tokyo, and Paris?";
    var functionName = "get_current_weather";
    var description = "Get the current weather in a given location";
    var properties = new Properties();
    properties.Property1 = new Property("The city and state, e.g. San Francisco, CA");

    //properties.Property2 = new Property()
    //{
    //    Description = "\"enum\": [\"celsius\", \"fahrenheit\"]"
    //};

    await gptService.CallFunctionApi(prompt, properties, functionName, description);
}

void RunCodeScanDemo()
{
    var codeScanner = new CodeScanner();
    codeScanner.ScanInit();
}

async Task RunGPTDemo()
{
    var gptService = new GptService();
    string repoPath = "https://github.com/dwcy/hotchocolate";

    await GitRepoFinder(repoPath);
}

async Task TestPrompt()
{
    // Print.Code("var x = new { user: someName };");
    string prompt = "var x = new { user: someName };";
    var prompts = new List<string>();
    prompts.Add(prompt);
    await CallChatGpt(prompts);
}

async Task GitRepoFinder(string repoPath)
{
    var gitRepoService = new GitRepoService();

    var isValidCSharpRepo = gitRepoService.InspectRepo(repoPath);
    if (isValidCSharpRepo)
    {
        var csFiles = gitRepoService.GetCsFiles();
        var excludedFiles = new string[] { "\\Interfaces\\", "\\Contracts\\", "\\DTO\\", "\\Configurations\\", "\\Migrations\\", "\\Entities\\", "Tests\\" };
        var ignoreGenerated = csFiles.Where(file => !excludedFiles.Any(excludedPath => file.Contains(excludedPath)));
        var prompts = new List<string>();

        foreach (string filePath in ignoreGenerated)
        {
            var cleanedFileName = Regex.Replace(filePath, @".*\\(.*)\\", "");
            Print.Normal($"Processing: {cleanedFileName}");
            var code = GetFileContentAsString(filePath);

            prompts.Add(code);
        }

        await CallChatGpt(prompts);

        //TODO also create documentation mermaid and text
    }
}

string GetFileContentAsString(string filePath)
{
    try
    {
        // Read the entire file into a StringBuilder
        StringBuilder fileContent = new StringBuilder();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                fileContent.AppendLine(line);
            }
        }

        return fileContent.ToString();
    }
    catch (IOException ex)
    {
        Print.Error($"An error occurred while reading the file: {ex.Message}");
    }

    return string.Empty;
}

//TODO extract 
//async Task CallChatGpt(string prompt)
//{
//    Print.Normal("Posting Message: " + prompt);

//    await gptService.CallApi(prompt);
//}

async Task CallChatGpt(List<string> prompts)
{
    var gptService = new GptService();
    await gptService.CallApi(prompts);
}
