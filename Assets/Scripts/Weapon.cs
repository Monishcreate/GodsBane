using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;

    CircleCollider2D triggerCircle;

    
    // Start is called before the first frame update
    void Start()
    {
        triggerCircle = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //var enemy = collision.gameObject.GetComponent<>;
        //if (enemy != null)
        //{
        //    enemy.health.HP -= damage;
        //
        //    if(enemy.health.HP <= 0)
        //    {
        //        Destroy(enemy.gameObject);
        //    }
        //}
    }

    public void EnableTriggerCircle()
    {
        triggerCircle.enabled = true;
    }

    public void DisableTriggerCircle()
    {
        triggerCircle.enabled = false;
    }
}
