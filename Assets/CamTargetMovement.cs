using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTargetMovement : MonoBehaviour
{
    public float target;
    public Vector2 vectarget;
    public GameObject player;
    public GameObject enemy;
    private Vector2 camPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = player.transform.position.x;
        vectarget = new Vector2 (target - 7.8f, transform.position.y);
        camPos = transform.position;
        Vector2.MoveTowards(camPos, vectarget, 20 );
    }
}
