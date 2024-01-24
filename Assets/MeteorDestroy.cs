using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDestroy : MonoBehaviour
{
    private PlayerMovement player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
            CameraShake.instance.ShakeCamera(4f);
        }

        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            CameraShake.instance.ShakeCamera(10f);
            player.PlayerBackDamage(20);

        }
    }

}
