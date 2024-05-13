namespace Csharp.DevTool.Infrastructure.Features;

public class FileService
{
    private readonly string _folderPath;
    private readonly string[] excludedFiles = new string[] {
        "\\Interfaces\\",
        "\\Contracts\\",
        "\\DTO\\",
        "\\Configurations\\",
        "\\Migrations\\",
        "\\Entities\\",
        "Tests\\"
    };

    public FileService(string path)
    {
        Console.WriteLine(path);
        if (path == null) throw new ArgumentNullException("path is empty");
        path = path.Trim();

        if (!Path.Exists(path)) throw new ArgumentException("path does not exist");

        _folderPath = path;
    }


    public List<string>? GetCsFiles()
    {
        var files = GetFiles("*.cs");
        if (files == null) Console.WriteLine("No cs files exist");

        return files;
    }

    public List<string>? GetReadmeFiles()
    {
        var files = GetFiles("README");
        if (files == null) Console.WriteLine("No README files exist");

        return files;
    }

    public List<string>? GetHttpFiles()
    {
        var files = GetFiles("*.http");
        if (files == null) Console.WriteLine("No http files exist");

        return files;
    }

    public List<string>? GetAppSettingsFiles()
    {
        var files = GetFiles("appsettings.*");
        if (files == null) Console.WriteLine("No appsettings files exist");

        return files;
    }

    public List<string>? GetProjectFiles()
    {
        var files = GetFiles("*.csproj");
        if (files == null) Console.WriteLine("No csproj files exist");

        return files;
    }

    public List<string>? GetSolutionFiles()
    {
        var files = GetFiles("*.sln");
        if (files == null) Console.WriteLine("No solution files exist");

        return files;
    }

    public List<string>? GetGitIgnoreFiles()
    {
        var files = GetFiles("*.gitignore");
        if (files == null) Console.WriteLine("No gitignore files exist");

        return files;
    }

    public List<string>? GetEditorConfigFiles()
    {
        var files = GetFiles("*.editorconfig");
        if (files == null) Console.WriteLine("No editorconfig files exist");

        return files;
    }

    public List<string>? GetTestsFiles()
    {
        var files = GetFiles("*.Test*");
        if (files == null) Console.WriteLine("No Test files exist");

        return files;
    }

    public List<string>? GetAllFiles()
    {
        return GetFiles("*");
    }

    /// <summary>
    /// Get requested file path
    /// </summary>
    /// <param name="extension">*.cs , *.json, README</param>
    /// <returns></returns>
    private List<string>? GetFiles(string extension)
    {

        var csFiles = Directory.EnumerateFiles(_folderPath, $"{extension}", SearchOption.AllDirectories);
        if (!csFiles.Any())
            return null;

        var ignoreGenerated = csFiles.ToList().Where(file => !excludedFiles.Any(excludedPath => file.Contains(excludedPath)));

        return ignoreGenerated.ToList();
    }
}
