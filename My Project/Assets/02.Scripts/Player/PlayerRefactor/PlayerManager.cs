using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private StateMachine stateMachine;
    private PlayerControllerR controller;
    private PlayerType charType;

    private Dictionary<PlayerType, GameObject> charObjs;
    private Dictionary<PlayerType, PlayerControllerR> charControllers;

    [SerializeField] private GameObject ghostCharObj;
    [SerializeField] private GameObject shieldCharObj;
    [SerializeField] private GameObject gogglesCharObj;
    [SerializeField] private GameObject cleanerCharObj;

    private void Awake()
    {
        stateMachine = new StateMachine();
        Initialize();
    }

    private void Start()
    {
        SetStartCharacter();
        stateMachine.Initialize(new IdleState(controller));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.LogWarning(GetCurrentState().ToString());
        }
        stateMachine.UpdateState(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdateState(Time.fixedDeltaTime);
    }

    private void Initialize()
    {
        CashCharControllers();
        CashCharObjects();
    }

    private void SetStartCharacter()
    {
        foreach(var controller in charControllers.Values)
        {
            controller.enabled = false;
        }

        charType = PlayerType.PLAYERGHOST;
        if(charControllers.ContainsKey(charType))
        {
            controller = charControllers[charType];
            controller.enabled = true;
        }
    }

    private void CashCharObjects()
    {
        charObjs = new Dictionary<PlayerType, GameObject>();

        if (ghostCharObj != null)
        {
            charObjs.Add(PlayerType.PLAYERGHOST, ghostCharObj);
        }

        if (shieldCharObj != null)
        {
            charObjs.Add(PlayerType.PLAYERSHIELD, shieldCharObj);
        }

        if (gogglesCharObj != null)
        {
            charObjs.Add(PlayerType.PLAYERGOGGLES, gogglesCharObj);
        }

        if (cleanerCharObj != null)
        {
            charObjs.Add(PlayerType.PLAYERCLEANER, cleanerCharObj);
        }
    }

    public void CashCharControllers()
    {
        charControllers = new Dictionary<PlayerType, PlayerControllerR>();

        var GhostCharacterController = GetComponent<GhostController>();
        if (GhostCharacterController != null)
        {
            charControllers.Add(PlayerType.PLAYERGHOST, GhostCharacterController);
        }

        var ShieldCharacterController = GetComponent<ShieldController>();
        if (ShieldCharacterController != null)
        {
            charControllers.Add(PlayerType.PLAYERSHIELD, ShieldCharacterController);
        }

        var GoggleCharacterController = GetComponent<GogglesController>();
        if (GoggleCharacterController != null)
        {
            charControllers.Add(PlayerType.PLAYERGOGGLES, GoggleCharacterController);
        }

        var playerCleanerController = GetComponent<CleanerController>();
        if (playerCleanerController != null)
        {
            charControllers.Add(PlayerType.PLAYERCLEANER, playerCleanerController);
        }
    }

    public void ChangeCharType(PlayerType nextCharType)
    {
        // 현재 플레이어 캐릭터 오브젝트 비활성화
        inactiveCurrentGameObject();

        charType = nextCharType;

        // Enable next character object
        charObjs[nextCharType].SetActive(true);

        // Enable next character script
        var nextCharcontroller = charControllers[nextCharType];
        controller = nextCharcontroller;
        controller.enabled = true;

        controller.SetCashComponent();

        GhostController ghostController = controller as GhostController;
        if (ghostController != null)
        {
            stateMachine.ChangeState(new DashState(controller));
        }
    }

    private void inactiveCurrentGameObject()
    {
        // Disable current Character Object
        charObjs[charType].SetActive(false);

        // Disable current Character Controller
        charControllers[charType].enabled = false;
    }

    public State GetCurrentState()
    {
        return stateMachine.CurrentState;
    }

    public PlayerControllerR GetCurrentController()
    {
        return controller;
    }
}
