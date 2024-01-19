using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitDetection : MonoBehaviour
{
    public PlayerMovement player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Dash detected the player");
            player.PlayerTakeDamage(100);
        }
    }
}
