using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerR : MonoBehaviour
{

    [SerializeField] private CharacterData[] charDatas;
    [SerializeField] private CharacterData currCharData;
    private Dictionary<PlayerType, CharacterData> charDataDics;

    private void Awake()
    {
        AddDatasToDic();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void AddDatasToDic()
    {
        charDataDics = new Dictionary<PlayerType, CharacterData>();

        foreach(var charData in charDatas)
        {
            charDataDics.Add(charData.charType, charData);
        }
    }
}
