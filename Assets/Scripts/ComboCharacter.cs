using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCharacter : MonoBehaviour
{

    private StateMachine meleeStateMachine;

  
    public float cooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState) && cooldown <= 0)
        {
            meleeStateMachine.SetNextState(new GroundEntry());
        }
    }
}
