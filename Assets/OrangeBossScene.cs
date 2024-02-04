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

    private void Start()
    {
        instance = this;
        
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
    }

}
