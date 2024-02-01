using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrostStuff : MonoBehaviour
{
    public GameObject ballPrefab;

    
    public Transform spawnpos;

    public bool shot; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 position = spawnpos.position;
        if (shot)
        {


            Instantiate(ballPrefab, position, Quaternion.identity);

            shot = false;

        }
    }


    public void IceSpawn()
    {

        shot = true;

        

    }
}
