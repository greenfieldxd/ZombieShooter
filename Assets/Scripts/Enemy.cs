using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public Transform shootPos;
    public GameObject bulletPrefab;


    private void Start()
    {
        StartCoroutine(PlayerKill(2));
    }

    private void Update()
    {
        RotateToPlayer();
    }


    IEnumerator PlayerKill(float delayFire)
    {
        yield return new WaitForSeconds(delayFire);
        Instantiate(bulletPrefab, shootPos.position, transform.rotation);

        StartCoroutine(PlayerKill(1));
         
    }

    private void RotateToPlayer()
    {
        Player player = FindObjectOfType<Player>();

        Vector2 direction = player.transform.position - transform.position;

        transform.up = -direction;
    }


    void DoDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
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
