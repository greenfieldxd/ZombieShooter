using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    public Action onHealthChange = delegate { };
    public Action onPlayerDeath = delegate { };

    public float fireRate;
    public float health;


    public GameObject bulletPrefab;
    public Transform shootPosition;

    float nextFire;

    Animator anim;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CanPlayerFire();
    }

    private void CanPlayerFire()
    {
        if (health > 0)
        {
            if (Input.GetButton("Fire1") && nextFire <= 0)
            {
                Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
                nextFire = fireRate;
                anim.SetTrigger("Shoot");
            }

            if (nextFire > 0)
            {
                nextFire -= Time.deltaTime;
            }
        }
    }

    public void DoDamage(float damage)
    {
        health -= damage;
        onHealthChange();

        if (health <= 0)
        {
            DeathPlayer();
        }

    }

    void DeathPlayer()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        anim.SetBool("Death", true);
        playerMovement.enabled = false;
        playerMovement.StopMovement();
        collider.enabled = false; // off collider
        rb.velocity = Vector2.zero; //velocity ZERO
        this.enabled = false;

        onPlayerDeath();
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
