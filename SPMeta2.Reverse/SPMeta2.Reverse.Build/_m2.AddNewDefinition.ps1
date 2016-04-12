cls

$g_DefinitionName = "ModuleFile"

$g_Replacements = @{

    Name =$g_DefinitionName;
    ParentName ="Folder";
}


#region utils

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

#endregion

$currentPath =  Get-ScriptDirectory
$solutionPath = [System.IO.Path]::GetFullPath($currentPath  + "\..\")

. "$currentPath\_m2.core.ps1"

Write-BInfo "Solution folder:[$solutionPath]" -fore Gray

function ConfirmYes() {
    
    $title = "Delete Files"
    $message = "Do you want to delete the remaining files in the folder?"

    $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", `
        "Deletes all the files in the folder."

    $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", `
        "Retains all the files in the folder."

    $options = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no)

    $result = $host.ui.PromptForChoice($title, $message, $options, 0) 

    if($result -eq 0) {
        return $true
    }

    return $false

}

function BootstrapDefinition($solutionPath, $definitionName, $replacements) {

    Write-BInfo "Generating classes for definition:[$definitionName]" -fore yellow

    $files = Get-ChildItem $solutionPath -Filter "*.cs.tmpl" -Recurse

    foreach($file in $files) {
    
        $name = $file.Name;
    
        Write-BVerbose "Processing template:[$name]" -fore green

        # file name
        $newFilePath = $file.FullName.Replace("`$Name`$", $definitionName).Replace('.tmpl', "")
        Write-BVerbose "New filepath:[$newFilePath]" -fore Gray

        # content
        $content = Get-Content $file.FullName

        foreach($key in $replacements.Keys) {
            $value = $replacements[$key]

            Write-BVerbose "Replacing [$key] with [$value]" -fore Gray
            $content = $content.Replace("`$" + $key  + "`$", $value);

            if([System.IO.File]::Exists($newFilePath) -eq $true) {
                
                Write-BVerbose "File exists. Want to overwrite? - [$newFilePath]" -fore yellow

                $sure = ConfirmYes


                if($sure -ne $true) {
                    Write-BVerbose "Skipping [$newFilePath]" -fore yellow
                    Write-BVerbose ""
                    continue
                }

                Write-BVerbose "Overwriting [$newFilePath]" -fore red
                $content | Out-File -FilePath $newFilePath -Confirm:$false -Force
            } else {

                Write-BVerbose "Overwriting [$newFilePath]" -fore Gray
                $content | Out-File -FilePath $newFilePath -Confirm:$false -Force
            }
        }

        Write-BVerbose ""
    }
}

BootstrapDefinition $solutionPath $g_DefinitionName  $g_Replacements