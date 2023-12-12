using System.Text;
using TestConsole;

Print.Ascii("Code Cop!");
Print.Code("var x = new { user: someName };");
GptService gptService = new GptService();

//string prompt = "var x = new { user: someName };";
//await CallChatGpt(prompt);

await GitRepoFinder();

Console.ReadKey();

async Task GitRepoFinder()
{
    var gitRepoService = new GitRepoService();
    string repoPath = "https://github.com/dwcy/hotchocolate";
    var isValidCSharpRepo = gitRepoService.InspectRepo(repoPath);
    if (isValidCSharpRepo)
    {
        var csFiles = gitRepoService.GetCsFiles();
        var excludedFiles = new string[] { "\\Interfaces\\", "\\Contracts\\", "\\DTO\\", "\\Configurations\\", "\\Migrations\\", "\\Entities\\", "Tests\\" };
        var ignoreGenerated = csFiles.Where(file => !excludedFiles.Any(excludedPath => file.Contains(excludedPath)));
        var prompts = new List<string>();

        foreach (string filePath in ignoreGenerated)
        {
            var cleanedFileName = filePath.Replace("C:\\Users\\dawid.parvulescu\\AppData\\Local\\Temp\\tempRepo\\", "");
            Print.Normal($"Processing: {cleanedFileName}");
            var code = GetFileContentAsString(filePath);

            prompts.Add(code);
        }

        await CallChatGpt(prompts);
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

//async Task CallChatGpt(string prompt)
//{
//    Print.Normal("Posting Message: " + prompt);

//    await gptService.CallApi(prompt);
//}

async Task CallChatGpt(List<string> prompts)
{
    await gptService.CallApi(prompts);
}
