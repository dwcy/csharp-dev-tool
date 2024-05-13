namespace Csharp.DevTool.Infrastructure.Features;

public class CodeScanner
{
    string path = "";

    public void ScanInit()
    {
        path = "...path to repo"; //TODO path from config
        var fileService = new FileService(path);

        PrintArchitecture(fileService);
        Print.Ascii("Files");
        Scan(fileService.GetCsFiles);
    }

    void PrintArchitecture(FileService fileService)
    {
        Print.Ascii("Architecture");
        Scan(fileService.GetSolutionFiles);
        Scan(fileService.GetAppSettingsFiles);
        Scan(fileService.GetProjectFiles);
    }

    void Scan(Func<List<string>> method)
    {
        var files = method();
        for (int i = 0; i < files.Count; i++)
        {
            var cleanName = files[i].Replace(path, string.Empty);
            Print.Info($"Solution: {cleanName} ({i + 1})");
        }
    }
}
