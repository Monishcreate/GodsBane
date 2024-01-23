using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorShower : MonoBehaviour
{
    public GameObject meteorPrefab;

    [SerializeField] float secondSpawn = 0.5f;

    public Animator anim;

    public GameObject MeteorHeight;

    public GameObject FrontMeteor;

    public GameObject BackMeteor;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (anim.GetBool("isMeteor"))
        {
            StartCoroutine(MeteorSpawn());
            anim.SetBool("isMeteor", false);
        }

    }


    IEnumerator MeteorSpawn()
    {
        while (!anim.GetBool("MeteorExit"))
        {
            float minTras = FrontMeteor.transform.position.x;
            float maxTras = BackMeteor.transform.position.x;
            var wanted = Random.Range(minTras, maxTras);
            var position = new Vector3(wanted, MeteorHeight.transform.position.y);
            GameObject gameObject = Instantiate(meteorPrefab, position, Quaternion.identity);
            yield return new WaitForSeconds(secondSpawn);
            
        }
    }
}
