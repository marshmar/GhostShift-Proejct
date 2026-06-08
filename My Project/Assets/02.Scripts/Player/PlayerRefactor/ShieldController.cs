using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : PlayerControllerR
{
    public GameObject shield;
    public Vector3 shieldPosition;
    private Shield shieldScr;
    private AudioClip swingSfx;

    protected override void Awake()
    {
        base.Awake();
        shieldPosition = new Vector3(0.8263f, 0, 0);
        shieldScr = shield.GetComponent<Shield>();
    }

    private void Start()
    {
        swingSfx = Resources.Load<AudioClip>("PlayerAudios/swing");
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Bullet"))
        {

        }
    }

    public override State HandleSpecialStateInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return new DefendState(this);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerManager.ChangeCharType(PlayerType.PLAYERGHOST);
            return new DashState(GetCurrentController());
        }

        if (Input.GetMouseButtonDown(1))
        {
            return new ParryState(this);
        }
        return null;
    }

    public void SetShieldObject(bool value)
    {
        shield.SetActive(value);
    }

    public void SetParryState(bool value)
    {
        shieldScr.isParrying = value;
    }

    public void PlaySwingSfx()
    {
        AudioSource.clip = swingSfx;
        AudioSource.Play();
    }
}
