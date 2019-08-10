using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine.SceneManagement;

public class Builder  {

	// Use this for initialization
	

    private static string[] _ReadNames()
    {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                temp.Add(scene.path);
            }
        }
        return temp.ToArray();
    }
    [MenuItem("Regulus/Project/BuildProject")]

    public static void BuildProject()
    {
        var version = new Version(PlayerSettings.bundleVersion);
        
        
        var newVersion = new Version(version.Major, version.Minor, version.Build + 1, version.Revision);
        PlayerSettings.bundleVersion = newVersion.ToString();

        var dir = string.Format("Game{0}{1}{2}{3}" , newVersion.Major, newVersion.Minor, newVersion.Build,newVersion.Revision)  ;
        if (System.IO.Directory.Exists("bin/pc/" + dir) == false)
            System.IO.Directory.CreateDirectory("bin/pc/" + dir);

        var scenenames = _ReadNames();


        foreach (var scenename in scenenames)
        {
            Debug.Log(string.Format("scene [{0}]", scenename));
        }
        var filename = string.Format("bin/pc/{0}/Play.exe" , dir);
        BuildPipeline.BuildPlayer(scenenames, filename, BuildTarget.StandaloneWindows64 , BuildOptions.None);
    }

    [MenuItem("Regulus/Project/Play")]

    public static void Play()
    {

        EditorSceneManager.SetActiveScene(EditorSceneManager.GetActiveScene());
        EditorSceneManager.OpenScene("Assets/project/Scene/Start.unity", OpenSceneMode.Single);
        EditorApplication.isPlaying = true;


    }

    [MenuItem("Regulus/Project/ResetVersion")]

    public static void ResetVersion()
    {
        PlayerSettings.bundleVersion = "0.0.0.0";
    }
}
