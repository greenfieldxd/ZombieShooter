﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

        if (health <= 0)
        {
            DeathPlayer();
        }

    }

    void DeathPlayer()
    {
        anim.SetBool("Death", true);
        playerMovement.enabled = false;
        playerMovement.StopMovement();
        this.enabled = false;

        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.RestartScene();
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
