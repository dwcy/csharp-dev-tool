using Microsoft.Extensions.Configuration;

public class GptRequestModel
{
    public GptRequestModel(IConfiguration config, List<string> prompts, bool functionCall = false)
    {
        Model = config["gptmodel"];
        Messages = new List<GptMessage>();
        foreach (string prompt in prompts)
        {
            Messages.Add(new GptMessage(prompt));
        }
    }
    public GptRequestModel(IConfiguration config, string prompt, bool functionCall = false)
    {
        Model = config["gptmodel"];
        Messages = new List<GptMessage> { new GptMessage(prompt) }; //default with one prompt
        //Functions = functionCall ? new List<GptFunction> { new GptFunction() } : null;
        //FunctionCall = functionCall ? "auto" : null;
    }

    public string Model { get; set; }
    public List<GptMessage> Messages { get; set; }
    //public List<GptFunction> Functions { get; set; }
    //public string FunctionCall; // to find out purpose
}

public struct GptMessage
{
    // "Act as a c# developer and architect with clean code in mind and you provide refactoring suggestions";
    const string SystemPromptDev = "Act as a c# developer and architect with clean code in mind. Provide response with refactoring suggestions providing only C# code as response - Code only!";
    public enum GptRoles
    {
        System,
        Assistant,
        User,
        Function
    }
    public GptMessage()
    {
        Role = GptRoles.Assistant.ToString().ToLower();
        Content = SystemPromptDev;
    }
    public GptMessage(string prompt)
    {
        Role = GptRoles.User.ToString().ToLower();
        Content = SystemPromptDev + " " + prompt;
    }
    public GptMessage(string prompt, GptRoles role)
    {
        Role = role.ToString().ToLower();
        Content = prompt;
    }

    public string Role { get; set; }
    public string Content { get; set; }
}
public struct GptFunction
{
    public GptFunction()
    {
        Name = "get_as_code";
        Description = "Refactor Code";
        Parameters = new GptParams();
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public GptParams Parameters { get; set; }
}

public struct GptParams
{
    public GptParams()
    {
        Type = "object";
    }
    public string Type { get; set; }
}
