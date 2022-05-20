using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildScript : MonoBehaviour
{
    // static void PerformBuild(string path, BuildTarget buildTarget)
    // {
    //     var options = new BuildPlayerOptions
    //     {
    //         scenes = ActiveScenePaths.ToArray(), 
    //         target = buildTarget, 
    //         locationPathName = path,
    //     };

    //     BuildPipeline.BuildPlayer(options);
    // }

    static void PerformiOSBuild()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
   		BuildPipeline.BuildPlayer(scenes, "./builds/iOS", BuildTarget.iOS, BuildOptions.None);
    }

    static void PerformAndroidBuild()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
   		BuildPipeline.BuildPlayer(scenes, "./builds/Android", BuildTarget.Android, BuildOptions.None);
    }
}
