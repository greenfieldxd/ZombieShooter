using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Ammo : MonoBehaviour
{
    public int ammoInMagazine = 15;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)  //Player layer
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.ammo <player.maxAmmo)
            {
                player.ammo += ammoInMagazine;
                player.onPlayerAmmo();

                LeanPool.Despawn(gameObject);
            }
        }
    }
}
