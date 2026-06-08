using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerController : PlayerControllerR
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            DamagePlayerAndKnockBack(collider);
        }
    }
}
