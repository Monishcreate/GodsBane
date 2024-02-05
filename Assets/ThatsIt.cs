using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThatsIt : MonoBehaviour
{
    // Start is called before the first frame update
    float changeTime = 15f;


    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;

        if (changeTime <= 0)
        {
            Application.Quit();
        }

    }
}
