using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Attributes")]
    public PlayerType charType;
    public float maxSpeed;
    public float jumpPower;
    public float knockbackPower;
    public bool canJump;
}
