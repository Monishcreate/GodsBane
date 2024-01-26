using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    public static Hitstop instance;
    // Start is called before the first frame update
    bool waiting;
    void Start()
    {
        instance = this;
    }

    public void doHitStop(float duration)
    {
        if (waiting)
        {
            return;
        }
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    public void doSlowDown(float duration)
    {
        if (waiting)
        {
            return;
        }
        Time.timeScale = 0.1f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }

    // Update is called once per frame
  
}
