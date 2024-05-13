using LibGit2Sharp;

public class GitRepoService
{
    public bool InspectRepo(string repoPath)
    {
        // Clone or open the Git repository
        using (var repo = CloneOrOpenRepo(repoPath))
        {
            if (repo == null)
            {
                Print.Error("Failed to clone or open the Git repository.");
                return false;
            }

            // Check if it's a C# project
            if (IsCSharpProject(repo))
            {
                Print.Info("This is a C# project.");
                return true;
            }
            else
            {
                Print.Warning("This is not a C# project.");
            }

            return false;
        }
    }

    private string GetWorkDir() => Path.Combine(Path.GetTempPath(), "tempRepo");

    private Repository CloneOrOpenRepo(string repoPath)
    {
        Repository repo;
        string workDir = GetWorkDir();
        try
        {
            repo = new Repository(workDir);
            if (repo != null)
            {
                Print.Normal($"Found cached repo : {workDir}");
                //todo git fetch
            }

        }
        catch (RepositoryNotFoundException)
        {
            // Clone the repository if it doesn't exist locally
            Print.Info($"Cloning repository from {repoPath}...");

            var clonedPath = Repository.Clone(repoPath, workDir);
            repo = new Repository(clonedPath);
        }

        return repo;
    }

    private bool IsCSharpProject(Repository repo)
    {
        // Check if there are .csproj files in the repository
        return Directory.EnumerateFiles(repo.Info.WorkingDirectory, "*.csproj", SearchOption.AllDirectories).Any();
    }

    public List<string> GetCsFiles()
    {
        var folderPath = GetWorkDir();
        var csFiles = Directory.EnumerateFiles(folderPath, "*.cs", SearchOption.AllDirectories);
        return csFiles.ToList();
    }
}
