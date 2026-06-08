using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GogglesController : PlayerControllerR
{
    public Action<bool> IsInNVDMode;

    private GameObject tileMap;
    private GameObject background;

    private Material originalBackground;
    private Material NVDBackground;
    private Tilemap tileMapSpr;

    protected override void Awake()
    {
        base.Awake();

        IsInNVDMode += nvdAction;
    }

    protected void Start()
    {
        NVDBackground = Resources.Load<Material>("Materials/NVDMaterial");
    }

    private void OnEnable()
    {
        tileMap = GameObject.Find("Tilemap");
        tileMapSpr = tileMap.GetComponent<Tilemap>();
        background = GameObject.Find("Quad");
        originalBackground = background.GetComponent<MeshRenderer>().material;
    }

    public override State HandleSpecialStateInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return new NVDState(this);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerManager.ChangeCharType(PlayerType.PLAYERGHOST);
            return new DashState(GetCurrentController());
        }
        return null;
    }

    private void nvdAction(bool isInNVDModes)
    {
        if (isInNVDModes)
            ChangeColorToGreen();
        else
            ChangeColorToWhite();
    }

    public void ChangeColorToGreen()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.green;
        tileMapSpr.color = Color.green;
        background.GetComponent<MeshRenderer>().material = NVDBackground;
    }

    public void ChangeColorToWhite()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        tileMapSpr.color = Color.white;
        background.GetComponent<MeshRenderer>().material = originalBackground;
    }
}
