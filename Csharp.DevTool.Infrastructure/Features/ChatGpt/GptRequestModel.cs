using Microsoft.Extensions.Configuration;

public class GptRequestModel
{
    public GptRequestModel(IConfiguration config, List<string> prompts)
    {
        Model = config["gptmodel"];
        ResponseFormat = new ResponseFormat("json_object");
        Messages = new List<GptMessage>();
        foreach (string prompt in prompts)
        {
            Messages.Add(new GptMessage(prompt));
        }
    }
    public GptRequestModel(IConfiguration config, string prompt)
    {
        Model = config["gptmodel"];
        Messages = new List<GptMessage> { new GptMessage(prompt) }; //default with one prompt
    }

    public GptRequestModel(IConfiguration config, string prompt, Properties propeties, string functionName, string description)
    {
        Model = config["gptmodel"];
        Messages = new List<GptMessage> { new GptMessage(prompt, GptMessage.GptRoles.User) }; //default with one prompt
        Tools.Add(new Tool(propeties, functionName, description));
    }

    public string Model { get; set; }
    public List<GptMessage> Messages { get; set; }
    public ResponseFormat ResponseFormat { get; set; }
    public List<Tool> Tools { get; set; } = new List<Tool>();
    public string ToolChoice { get; set; } = "auto";
}

public class ResponseFormat
{
    public ResponseFormat(string type)
    {
        Type = type;
    }

    public string Type { get; set; }
}

public struct GptMessage
{
    // "Act as a c# developer and architect with clean code in mind and you provide refactoring suggestions";
    public enum GptRoles
    {
        System,
        Assistant,
        User,
        Function
    }

    public GptMessage(string prompt)
    {
        string SystemPromptDev = "Act as a c# developer and architect with clean code in mind. Provide response with refactoring suggestions providing only C# code as response - Code only!";

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


public class Tool
{
    public Tool(Properties propeties, string functionName, string description)
    {
        Function = new GptFunction(propeties, functionName, description);
        Type = "function";
    }

    public string Type { get; set; }
    public GptFunction Function { get; set; }
}

public class GptFunction
{
    public GptFunction(Properties propeties, string functionName, string description)
    {
        Name = functionName;
        Description = description;
        Parameters = new GptParams(propeties);
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public GptParams Parameters { get; set; }
}

public struct GptParams
{
    public GptParams(Properties propeties)
    {
        Type = "object";
        Properties = propeties;
        Required.Add(nameof(Properties.Property1));
    }
    public string Type { get; set; }

    public Properties Properties { get; set; }

    public List<string> Required { get; set; } = new List<string>();
}

public class Properties
{
    public Property Property1 { get; set; }
    public Property Property2 { get; set; }
}

public class Property
{
    public Property(string description)
    {
        Description = description;
    }

    public string Type { set; private get; } = "string";
    public string Description { get; set; }
}
