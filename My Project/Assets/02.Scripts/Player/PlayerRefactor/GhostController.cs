using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : PlayerControllerR
{
    public bool CanDash { get; set; }

    private bool isCalledStickEvent;
    private Enemy target;

    protected override void Awake()
    {
        base.Awake();

        CanDash = true;
        isCalledStickEvent = false;
    }
    public override State HandleSpecialStateInput()
    {
        if(Input.GetMouseButtonDown(0) && CanDash)
        {
            Vector2 dirUnit = GetPlayerToMouseUnitVector();
            return new DashState(this);
        }

        if(isCalledStickEvent)
        {
            isCalledStickEvent = false;
            return new StickState(this, target);

        }
        return null;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy"))
        {
            if(playerManager.GetCurrentState() is DashState)
            {
                Debug.Log("Change to stick state");
                Enemy enemy = collider.GetComponent<Enemy>();   
                if(enemy != null)
                {
                    target = enemy;
                    target.Died();
                    isCalledStickEvent = true;
                }

            }
        }
    }


    public void ChangeCharacter()
    {
        if(CanPosses(target.enemyType) == false)
        {
            return;
        }

        playerManager.ChangeCharType(GetChangePlayerType(target.enemyType));
        target = null;
    }

    private bool CanPosses(EnemyType enemyType)
    {
        if (enemyType == EnemyType.NONE || enemyType == EnemyType.GUNNER)
            return false;
        return true;
    }

    private PlayerType GetChangePlayerType(EnemyType enemyType)
    {
        PlayerType playerType = PlayerType.PLAYERGHOST;
        switch (enemyType)
        {
            case EnemyType.SHIELD:
                playerType = PlayerType.PLAYERSHIELD;
                break;
            case EnemyType.GOGGLES:
                playerType = PlayerType.PLAYERGOGGLES;
                break;
            case EnemyType.CLEANER:
                playerType = PlayerType.PLAYERCLEANER;
                break;
        }
        return playerType;
    }
}
