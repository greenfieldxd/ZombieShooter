using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public Transform shootPos;
    public GameObject bulletPrefab;

    Animator anim;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(PlayerKill(2));
        StartCoroutine(RotateToPlayer());
    }

    private void Update()
    {
        //RotateToPlayer();
    }


    IEnumerator PlayerKill(float delayFire)
    {
        yield return new WaitForSeconds(delayFire);
        anim.SetTrigger("Shoot");
        Instantiate(bulletPrefab, shootPos.position, transform.rotation);

        StartCoroutine(PlayerKill(1));
         
    }

    IEnumerator RotateToPlayer()
    {
        yield return new WaitForEndOfFrame();
        Player player = FindObjectOfType<Player>();

        Vector2 direction = player.transform.position - transform.position;

        transform.up = -direction;

        StartCoroutine(RotateToPlayer());
    }


    public void DoDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetBool("Death", true);
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            collider.enabled = false;
            StopAllCoroutines();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        
        if (damageDealer != null)
        {
            DoDamage(damageDealer.damage);
        }
    }


}
