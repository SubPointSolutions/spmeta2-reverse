// load up common tools
#load tools/SubPointSolutions.CakeBuildTools/scripts/SubPointSolutions.CakeBuild.Core.cake

// replace default buil
defaultActionBuild.Task.Actions.Clear();
defaultActionBuild
    .Does(() => 
{
        
});

// default targets
RunTarget(target);