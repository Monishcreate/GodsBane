using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{

    protected float time { get; set; }

    protected float fixedtime { get; set; }

    protected float latetime { get; set; }

    public StateMachine stateMachine;

    public virtual void OnEnter(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual void OnUpdate()
    {
        time += Time.deltaTime;
    }

    public virtual void OnFixedUpdate() 
    { 
        fixedtime += Time.deltaTime;
    }

    public virtual void OnLateUpdate()
    {
        latetime += Time.deltaTime;
    }


    public virtual void OnExit() 
    { 
    
    }

    #region Passthrough methods

    // Removes a gameobject, component or asset.

    protected static void Destroy(UnityEngine.Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }

    // returns the component of type T if the game object has one attached, null if it doesnt

    protected T GetComponent<T>() where T : Component
    {
        return stateMachine.GetComponent<T>();
    }

    protected Component GetComponent(System.Type type)
    {
        return stateMachine.GetComponent(type);
    }

    protected Component GetComponent(string type)
    {
        return stateMachine.GetComponent(type);
    }
    #endregion

}
