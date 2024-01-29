using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCombatState : State
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        Debug.Log("Entering Idle Combat State");
    }

    public override void OnUpdate()
    {
        // Implement update logic for idle combat state
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Idle Combat State");
    }
}
