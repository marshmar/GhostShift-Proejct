using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerR : MonoBehaviour
{
    public StateMachine StateMachine {  get; private set; }

    public float maxSpeed;

    [SerializeField]
    protected float knockBackPower;
    [SerializeField] 
    protected bool isKnockBack;

    protected virtual void Awake()
    {
        StateMachine = new StateMachine();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isKnockBack = false;
    }

    protected virtual void Update()
    {
        StateMachine.UpdateState();
    }

    public virtual void Initialize()
    {

    }
}
