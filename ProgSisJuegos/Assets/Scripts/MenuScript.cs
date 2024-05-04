using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public string newGameScene = "Introduction";
    public string tryAgainScene = "Gameplay";

    public void ButtonStartGame()
    {
        StartCoroutine(GotoLevel(newGameScene));
    }

    public void ButtonTryAgain()
    {
        StartCoroutine(GotoLevel(tryAgainScene));
    }

    public void ButtonMainMenu()
    {
        StartCoroutine(GotoLevel(mainMenuScene));
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    IEnumerator GotoLevel(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
            yield return null;
    }
}
