using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void FlightStart()
    {
        SceneManager.LoadScene("Tut1");
    }

    public void FrostStart()
    {
        SceneManager.LoadScene("Tut2");
    }

    public void SpeedStart()
    {
        SceneManager.LoadScene("Tut3");
    }
}
