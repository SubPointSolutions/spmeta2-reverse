cls

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value;
    if($Invocation.PSScriptRoot)
    {
        $Invocation.PSScriptRoot;
    }
    Elseif($Invocation.MyCommand.Path)
    {
        Split-Path $Invocation.MyCommand.Path
    }
    else
    {
        $Invocation.InvocationName.Substring(0,$Invocation.InvocationName.LastIndexOf("\"));
    }
}

$currentDir = Get-ScriptDirectory

. "$currentDir\_m2.core.ps1"
. "$currentDir\_m2.nuget.core.ps1"

# global 'g_' variables to be used across the board

# should publish to NuGet?
$g_shouldPublish = $true

# should use daily-based version
$g_useDayVersion = $false

# use M2 v12 or v13 packaging
$g_is13 = $false

# M2 NuGet package version, release noted URL and target solution directory
$g_hardcoreVersionBase = "1.2.65";
$g_hardcoreVersion = "$g_hardcoreVersionBase-beta1";

$g_releaseNotes = "https://github.com/SubPointSolutions/spmeta2-reverse/releases/tag/1.2.60";
$g_solutionDirectory = "C:\Users\$env:USERNAME.$env:USERDOMAIN\Source\Repos\spmeta2-reverse\SPMeta2.Reverse\"

$g_Verbosity = 'quiet'

#by default to the public repo
$g_SourceUrl = $null
$g_apiKey = $null

if($g_isAppVeyor -eq $true) 
{
	Write-BInfo "AppVeyor build"

	# setting up everything for staging build
	
	$g_apiKey = Get-EnvironmentVariable "SPMeta2_NuGet_Staging_APIKey"
	$g_SourceUrl = "https://www.myget.org/F/subpointsolutions-staging/api/v2/package"
	
	$date = get-date
	$stamp = ( $date.ToString("yy") + $date.DayOfYear.ToString("000") + $date.ToString("HHmm"))
	$g_hardcoreVersion = ($g_hardcoreVersionBase + "-alpha" + $stamp)

	# appveyor path to build
	$g_solutionDirectory = "c:\prj\m2.reverse\SPMeta2.Reverse"
}
else 
{
	Write-BInfo "Local build"

	# setting for user-defined build, put your creds here
	$g_apiKey = Get-EnvironmentVariable "SPMeta2_NuGet_Staging_APIKey"
	$g_SourceUrl = "https://www.myget.org/F/subpointsolutions-staging/api/v2/package"
	
	$date = get-date
	$stamp = ( $date.ToString("yy") + $date.DayOfYear.ToString("000") + $date.ToString("HHmm"))
	$g_hardcoreVersion = $g_hardcoreVersionBase + "-alpha" + $stamp

	# local one, no domain
	$g_solutionDirectory = "C:\Users\$env:USERNAME\Source\Repos\spmeta2-reverse\SPMeta2.Reverse\"
}

Write-BInfo "g_solutionDirectory:[$g_solutionDirectory]"
Write-BInfo "g_hardcoreVersion:[$g_hardcoreVersion]"
Write-BInfo "g_SourceUrl:[$g_SourceUrl]"

# override 'g_' here if any

function ProcessReversePackageProperties($package) {
	
	$package.ProjectUrl = "https://github.com/SubPointSolutions/spmeta2-reverse"
	$package.IconUrl = "https://github.com/SubPointSolutions/spmeta2-reverse"

}

function CreateReverseCorePackage() {
    
    $asm = @()

	$asm += "SPMeta2.Reverse.dll";
    $asm += "SPMeta2.Reverse.xml";
	
	$package = GetPackagePrototype

	ProcessReversePackageProperties $package

    #$package.Assemblies35 = $asm;
	$package.Assemblies45 = $asm;
    #$package.Assemblies40 = $asm;

	$package.Version = $version;
	$package.Id =  "SPMeta2.Reverse"

	$package.Description = "SPMeta2 Reverse common infrastructure. Provides based interfaces, services and reverse handlers.";
	$package.Summary = "SPMeta2 Reverse common infrastructure."
	
    $spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Core"
	$spMetaCore.Version = "1.2.60"

	$package.Dependencies += $spMetaCore

	CreatePackage $package $spRuntime
}

function CreateReverseCSOMPackage() {

    $asm = @()

	$asm += "SPMeta2.Reverse.CSOM.dll";
    $asm += "SPMeta2.Reverse.CSOM.xml";

	$package = GetPackagePrototype

	ProcessReversePackageProperties $package

    #$package.Assemblies35 = $asm;
	$package.Assemblies45 = $asm;
    #$package.Assemblies40 = $asm;

	$package.Version = $version;
	$package.Id =  "SPMeta2.Reverse.CSOM"

	$package.Description = "SPMeta2 Reverse implementation for CSOM.";
	$package.Summary = "SPMeta2 Reverse implementation for SharePoint CSOM API."
	
	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Reverse"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

    CreatePackage $package $spRuntime
}

function CreateReverseCSOMStandardPackage() {

    $asm = @()

	$asm += "SPMeta2.Reverse.CSOM.Standard.dll";
    $asm += "SPMeta2.Reverse.CSOM.Standard.xml";

	$package = GetPackagePrototype

	ProcessReversePackageProperties $package

    #$package.Assemblies35 = $asm;
	$package.Assemblies45 = $asm;
    #$package.Assemblies40 = $asm;

	$package.Version = $version;
	$package.Id =  "SPMeta2.Reverse.CSOM.Standard"

	$package.Description = "SPMeta2 Reverse implementation for CSOM Standard.";
	$package.Summary = "SPMeta2 Reverse implementation for SharePoint CSOM Standard API."
	
	$spMetaCore = GetDependencyPrototype
	$spMetaCore.Id = "SPMeta2.Reverse.CSOM"
	$spMetaCore.Version = $version

	$package.Dependencies += $spMetaCore

    $spMetaStandard = GetDependencyPrototype
	$spMetaStandard.Id = "SPMeta2.Core.Standard"
	$spMetaStandard.Version = "1.2.60"

	$package.Dependencies += $spMetaStandard

    CreatePackage $package $spRuntime
}

function CreateNuGetPackages() {
	
	$version = GetPackageVersion 1 0 $g_useDayVersion

	Write-BInfo "Creating packages for version [$version]"

	Write-BInfo "Creating SPMeta2.Reverse package"
	CreateReverseCorePackage

    Write-BInfo "Creating SPMeta2.Reverse.CSOM package"
    CreateReverseCSOMPackage

    Write-BInfo "Creating SPMeta2.Reverse.CSOM.Standard package"
    CreateReverseCSOMStandardPackage
}

CreateNuGetPackages