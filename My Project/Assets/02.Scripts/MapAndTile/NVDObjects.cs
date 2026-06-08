using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class NVDObjects : MonoBehaviour
{
    private GogglesController gogglesController;
    private TilemapRenderer tilemapRenderer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if(playerObj != null)
        {
            gogglesController = playerObj.GetComponent<GogglesController>();
            if(gogglesController != null)
            {
                gogglesController.IsInNVDMode += isNVD => tilemapRenderer.enabled = isNVD;
            }

        }

        tilemapRenderer = GetComponent<TilemapRenderer>();
        this.gameObject.GetComponent<Tilemap>().color = Color.green;
        tilemapRenderer.enabled = false;
    }
}
