using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToBlue : MonoBehaviour
{
    // Start is called before the first frame update
    float changeTime = 15.9f;


    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;

        if (changeTime <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}
