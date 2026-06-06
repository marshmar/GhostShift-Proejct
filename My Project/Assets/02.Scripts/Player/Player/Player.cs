using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerGhost;
    public GameObject playerShield;
    public GameObject playerGoggles;
    public GameObject playerCleaner;

    private Dictionary<PlayerType, GameObject> charObjs;
    private Dictionary<PlayerType, PlayerController> charControllers;

    private bool isPossesing;
    private PlayerType currCharType;

    public bool IsPossesing
    {
        get
        {
            return isPossesing;
        }
        set
        {
            this.isPossesing = value;
        }
    }


    private void Awake()
    {
        // 시작 타입을 유령 타입으로 설정
        currCharType = PlayerType.PLAYERGHOST;

        CashCharacterObjects();
        CashScript();
        Init();

    }

    private void CashCharacterObjects()
    {
        charObjs = new Dictionary<PlayerType, GameObject>();

        if (playerGhost != null)
        {
            charObjs.Add(PlayerType.PLAYERGHOST, playerGhost);
        }

        if (playerShield != null)
        {
            charObjs.Add(PlayerType.PLAYERSHIELD, playerShield);
        }

        if (playerShield != null)
        {
            charObjs.Add(PlayerType.PLAYERGOGGLES, playerGoggles);
        }

        if (playerShield != null)
        {
            charObjs.Add(PlayerType.PLAYERCLEANER, playerCleaner);
        }
    }

    public void CashScript()
    {
        charControllers = new Dictionary<PlayerType, PlayerController>();

        var GhostCharacterController = GetComponent<PlayerGhostController>();
        if(GhostCharacterController != null)
        {
            charControllers.Add(PlayerType.PLAYERGHOST, GhostCharacterController);
        }

        var ShieldCharacterController = GetComponent<PlayerShieldController>();
        if (ShieldCharacterController != null)
        {
            charControllers.Add(PlayerType.PLAYERSHIELD, ShieldCharacterController);
        }

        var GoggleCharacterController = GetComponent<PlayerGogglesController>();
        if (GoggleCharacterController != null)
        {
            charControllers.Add(PlayerType.PLAYERGOGGLES, GoggleCharacterController);
        }
        
        var playerCleanerController = GetComponent<PlayerCleanerController>();
        if (playerCleanerController != null)
        {
            charControllers.Add(PlayerType.PLAYERCLEANER, playerCleanerController);
        }
    }

    public void Init()
    {
        isPossesing = false;

        // Enable only ghost character controller
        foreach ( var charController in charControllers.Values)
        {
            charController.enabled = false;
        }
        charControllers[currCharType].enabled = true;
    }

    public void ChangePlayer(PlayerType nextCharType) 
    {
        // 현재 플레이어 캐릭터 오브젝트 비활성화
        inactiveCurrentGameObject(currCharType);

        currCharType = nextCharType;

        // Enable next character object
        charObjs[nextCharType].SetActive(true);

        // Enable next character script
        var controller = charControllers[nextCharType];
        controller.enabled = true;

        PlayerGhostController ghostController = controller as PlayerGhostController;    
        if(ghostController != null)
        {
            ghostController.ChangePlayerToGhost();
        }
    }

    private void inactiveCurrentGameObject(PlayerType currCharType) 
    {
        // Disable current Character Object
        charObjs[currCharType].SetActive(false);

        // Disable current Character Controller
        charControllers[currCharType].enabled = false;
    }

}
