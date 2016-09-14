#addin "Cake.Powershell"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

var solutionDirectory = "./../"; 
var solutionFilePath = "./../SPMeta2.Reverse.sln";

var defaultTestCategory = "CI.Core";
var defaultTestAssemblyPaths = new string[] {
    "./../SPMeta2.Reverse.Tests/bin/debug/SPMeta2.Reverse.Tests.dll"
};

var useNuGetPackaging = false;
var useNuGetPublishing = false;

var useCIBuildVersion = true;

var g_hardcoreVersionBase = "1.2.70";
var g_SPMeta2VersionBase = "1.2.70";

var g_hardcoreVersion = g_hardcoreVersionBase + "-beta1";

var date = DateTime.Now;
var stamp = (date.ToString("yy") + date.DayOfYear.ToString("000") + date.ToString("HHmm"));

if(useCIBuildVersion) {
    g_hardcoreVersion = g_hardcoreVersionBase + "-alpha" + stamp;
}

var nuGetPackagesDirectory = "./packages";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var buildDirs = new [] {

    new DirectoryPath("./../SPMeta2.Reverse/bin/"),

    new DirectoryPath("./../SPMeta2.Reverse.CSOM/bin/"),
    new DirectoryPath("./../SPMeta2.Reverse.CSOM.Standard/bin/"),

    new DirectoryPath("./../SPMeta2.Reverse.Tests/bin/"),

    new DirectoryPath("./../SPMeta2.Reverse.Tests/bin/"),

    new DirectoryPath(nuGetPackagesDirectory)
}; 

var environmentVariables = new [] {
    "spmeta2-reverse-nuget-source",
    "spmeta2-reverse-nuget-key"
};

 var nuspecs = new [] {
        new NuGetPackSettings()
        {
            Id = "SPMeta2.Reverse",
            Version = g_hardcoreVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "SPMeta2.Core", Version = g_SPMeta2VersionBase }
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/spmeta2/license<"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/spmeta2"),
            
            Description = "SPMeta2 Reverse common infrastructure. Provides based interfaces, services and reverse handlers.",
            Copyright = "Copyright 2015",
            Tags = new [] { "SPMeta2", "SPMeta2 Reverse", "SharePoint" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../SPMeta2.Reverse/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "SPMeta2.Reverse.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "SPMeta2.Reverse.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(nuGetPackagesDirectory)
        },

        new NuGetPackSettings()
        {
            Id = "SPMeta2.Reverse.CSOM-v15",
            Version = g_hardcoreVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "SPMeta2.Core", Version = g_SPMeta2VersionBase },
                new NuSpecDependency() { Id = "SPMeta2.CSOM.Foundation", Version = g_SPMeta2VersionBase },
                
                new NuSpecDependency() { Id = "SPMeta2.Reverse", Version = g_hardcoreVersion },
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/spmeta2/license<"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/spmeta2"),
            
            Description = "SPMeta2 Reverse common infrastructure. Provides based interfaces, services and reverse handlers.",
            Copyright = "Copyright 2015",
            Tags = new [] { "SPMeta2", "SPMeta2 Reverse", "SharePoint" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../SPMeta2.Reverse.CSOM/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "SPMeta2.Reverse.CSOM.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "SPMeta2.Reverse.CSOM.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(nuGetPackagesDirectory)
        },

        new NuGetPackSettings()
        {
            Id = "SPMeta2.Reverse.CSOM.Standard-v15",
            Version = g_hardcoreVersion,

            Dependencies = new []
            {
                new NuSpecDependency() { Id = "SPMeta2.Core", Version = g_SPMeta2VersionBase },
                new NuSpecDependency() { Id = "SPMeta2.CSOM.Foundation", Version = g_SPMeta2VersionBase },
                
                new NuSpecDependency() { Id = "SPMeta2.Reverse", Version = g_hardcoreVersion },
                new NuSpecDependency() { Id = "SPMeta2.Reverse.CSOM-v15", Version = g_hardcoreVersion },
            },

            Authors = new [] { "SubPoint Solutions" },
            Owners = new [] { "SubPoint Solutions" },
            LicenseUrl = new Uri("http://docs.subpointsolutions.com/spmeta2/license<"),
            ProjectUrl = new Uri("https://github.com/SubPointSolutions/spmeta2"),
            
            Description = "SPMeta2 Reverse common infrastructure. Provides based interfaces, services and reverse handlers.",
            Copyright = "Copyright 2015",
            Tags = new [] { "SPMeta2", "SPMeta2 Reverse", "SharePoint" },

            RequireLicenseAcceptance = false,
            Symbols = false,
            NoPackageAnalysis = true,
            BasePath  = "./../SPMeta2.Reverse.CSOM.Standard/bin/debug",
            
            Files = new [] {
                new NuSpecContent {
                    Source = "SPMeta2.Reverse.CSOM.Standard.dll",
                    Target = "lib/net45"
                },
                new NuSpecContent {
                    Source = "SPMeta2.Reverse.CSOM.Standard.xml",
                    Target = "lib/net45"
                }
            },
            
            OutputDirectory = new DirectoryPath(nuGetPackagesDirectory)
        }
    };  

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Validate-Environment")
    .Does(() =>
{
    foreach(var name in environmentVariables)
    {
        Information(string.Format("HasEnvironmentVariable? - [{0}]", name));
        if(!HasEnvironmentVariable("spmeta2-reverse-nuget-source")) {
            Information(string.Format("Cannot find environment variable:[{0}]", name));
            throw new ArgumentException(string.Format("Cannot find environment variable:[{0}]", name));
        }
    }
});

Task("Clean")
    .IsDependentOn("Validate-Environment")
    .Does(() =>
{
    foreach(var dirPath in buildDirs) {
        CleanDirectory(dirPath);
    }        
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFilePath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
      MSBuild(solutionFilePath, settings => {
        
            settings.SetVerbosity(Verbosity.Quiet);
            settings.SetConfiguration(configuration);
      });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var assemblyPath in defaultTestAssemblyPaths) {
        
        Information(string.Format("Running test for assembly:[{0}]", assemblyPath));
        
        MSTest(new [] { new FilePath(assemblyPath) }, new MSTestSettings {
            Category = defaultTestCategory
        });
    }
});

Task("NuGet-Packaging")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    if(!useNuGetPackaging) {
        Information("Skipping NuGet packaging...");
    }

    Information("Creating NuGet packages for version:[{0}] and SPMeta2 version:[{1}]", new []{
        g_hardcoreVersion,
        g_SPMeta2VersionBase
    });

    CreateDirectory(nuGetPackagesDirectory);
    CleanDirectory(nuGetPackagesDirectory);

    foreach(var nuspec in nuspecs)
    {   
        Information(string.Format("Creating NuGet package for [{0}]", nuspec.Id));
        NuGetPack(nuspec);
    }        
});

Task("NuGet-Publishing")
    .IsDependentOn("NuGet-Packaging")
    .Does(() =>
{
    if(!useNuGetPublishing) {
        Information("Skipping NuGet publishing...");
    }

    Information("Publishing NuGet packages for version:[{0}] and SPMeta2 version:[{1}]", new []{
        g_hardcoreVersion,
        g_SPMeta2VersionBase
    });

    foreach(var nuspec in nuspecs)
    {   
        Information(string.Format("Publishing NuGet package for [{0}]", nuspec.Id));

        var packageFileName = string.Format("{0}.{1}.nupkg", nuspec.Id, nuspec.Version);
        var packageFilePath = string.Format("{0}/{1}", nuGetPackagesDirectory, packageFileName);
        
        if(System.IO.File.Exists(packageFilePath)) {
            Information(string.Format("Publishing NuGet package [{0}]...", packageFileName));

            NuGetPush(packageFilePath, new NuGetPushSettings {
                Source = EnvironmentVariable("spmeta2-reverse-nuget-source")​,
                ApiKey = EnvironmentVariable("spmeta2-reverse-nuget-key")​
            });
            
        } else {
            Information(string.Format("NuGet package does not exist:[{0}]", packageFilePath));
            throw new ArgumentException(string.Format("NuGet package does not exist:[{0}]", packageFilePath));
        }
    }        
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    //.IsDependentOn("Run-Unit-Tests");
    .IsDependentOn("NuGet-Publishing");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);