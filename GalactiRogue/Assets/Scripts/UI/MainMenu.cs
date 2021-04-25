using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnStartClicked()
    {
        SceneManager.LoadScene("InitGame");
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
