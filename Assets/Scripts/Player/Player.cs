using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void DoDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DeathPlayer();
        }

    }

    void DeathPlayer()
    {
        playerMovement.isAlive = false;
        anim.SetBool("Death", true);
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Game");
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
