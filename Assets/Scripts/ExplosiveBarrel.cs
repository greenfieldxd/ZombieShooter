using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float radiusExplode;
    public float explodeDamage;
    public int strength;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (strength <= 0)
        {
            Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(transform.position, radiusExplode);
            foreach (Collider2D objectI in objectsInRadius)
            {
                Player player = objectI.GetComponent<Player>();
                Enemy enemy = objectI.GetComponent<Enemy>();
                Zombie zombie = objectI.GetComponent<Zombie>();
                if (player != null)
                {
                    player.DoDamage(explodeDamage);
                }
                if (enemy != null)
                {
                    enemy.DoDamage(explodeDamage);
                }
                if (zombie != null)
                {
                    zombie.DoDamage(explodeDamage);
                }
            }
            anim.SetTrigger("Explode");
            Destroy(gameObject, 0.5f);

        }
        strength--;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusExplode);
    }
}
