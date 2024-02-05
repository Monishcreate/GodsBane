using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrangeBossScene : MonoBehaviour
{
    public static OrangeBossScene instance;

    public bool isOrangeScene;

    public bool isBlueScene;

    public bool isBlackScene;

    public GameObject PauseMenu;

    public static bool isPaused;

    private void Start()
    {
        instance = this;
        PauseMenu.SetActive(false);
        
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Orange" || SceneManager.GetActiveScene().name == "Tut1")
        {
            isOrangeScene = true;
        }
        else
        {
            isOrangeScene = false;
        }
        if (SceneManager.GetActiveScene().name == "Black Phase 1" || SceneManager.GetActiveScene().name == "Black Phase2" || SceneManager.GetActiveScene().name == "Tut3")
        {
            isBlackScene = true;
           
           
        }
        else
        {

            
            isBlackScene = false;
        }

        if (SceneManager.GetActiveScene().name == "Blue" || SceneManager.GetActiveScene().name == "Tut2")
        {
            isBlueScene = true;
        }
        else
        {
            isBlueScene = false;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ContinueGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
