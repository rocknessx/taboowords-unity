using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void AnaMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void AraMenu()
    {
        SceneManager.LoadScene(3);
    }

}
