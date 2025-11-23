using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// By making it static it's not attached to any specific instance 
// of this object and can't have any instances constructed. 
// Doesn't have to be static, but since all fields are static 
// might as well make it static
public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        // load loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        // loading scene runs the LoaderCallback function in it's Update function so 
        // loads the target scene after the loading scene has rendered for 1 frame
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
