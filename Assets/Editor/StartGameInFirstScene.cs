using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]
public static class StartGameInFirstScene
{
    // click command-9 to go to the prelaunch scene and then play

    [MenuItem("Edit/Play-Unplay, But From Prelaunch Scene %0")]
    public static void PlayFromPrelaunchScene()
    {
        if (EditorApplication.isPlaying == true)
        {
            EditorApplication.isPlaying = false;
            return;
        }
        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.OpenScene(
                    "Assets/Scenes/Launcher.unity");
        EditorApplication.isPlaying = true;
    }
}
