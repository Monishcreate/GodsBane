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
        if (SceneManager.GetActiveScene().name == "Orange")
        {
            isOrangeScene = true;
        }
        else
        {
            isOrangeScene = false;
        }
        if (SceneManager.GetActiveScene().name == "Black Phase 1" || SceneManager.GetActiveScene().name == "Black Phase2")
        {
            isBlackScene = true;
           
           
        }
        else
        {

            
            isBlackScene = false;
        }
    }

}
