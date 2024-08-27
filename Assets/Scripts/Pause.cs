using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseController : MonoBehaviour
{
    bool currentPauseState = false;
    [SerializeField] GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!currentPauseState)
            {
                PauseGameTime();
            }
            else
            {
                UnPauseGameTime();
            }
        }
    }

    public void PauseGameTime()
    {
        currentPauseState = !currentPauseState;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnPauseGameTime()
    {
        currentPauseState = !currentPauseState;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void UnpauseAndRestart()
    {
        UnpauseAndLoad(SceneManager.GetActiveScene().name);
    }

    public void UnpauseAndLoad(string nextScene)
    {
        currentPauseState = !currentPauseState;
        Time.timeScale = 1;
        SceneManager.LoadScene(nextScene);
    }

    public void MenuPrincipal()
    {
        currentPauseState = !currentPauseState;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}