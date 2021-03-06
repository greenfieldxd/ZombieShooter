﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;
using Lean.Pool;

public class Zombie : MonoBehaviour
{
    public Action onHealthChanged = delegate { };

    public GameObject startPosition;
    public GameObject ammo;

    [Header("AI config")]
    public float followDistance;
    public float attackDistance;
    public float outOfFallowDistance;
    public float searchAngle = 45;

    [Header("Attack, health config")]
    public float attackRate;
    public float damage;
    public float health;
    public float maxHealth;

    float nextAttack;

    bool death = false;


    enum ZombieStates
    {
        STAND,
        MOVE,
        ATTACK
    }

    ZombieStates activeState;

    Player player;

    AIPath movement;
    AIDestinationSetter target;
    Animator anim;
    Rigidbody2D rb;

   
    void Start()
    {
        player = FindObjectOfType<Player>();

        movement = GetComponent<AIPath>();
        target = GetComponent<AIDestinationSetter>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = Instantiate(startPosition, transform.position, Quaternion.identity);

        ChangeState(ZombieStates.STAND);
        player.onPlayerDeath += ChangeZombieStateToSTAND;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState();

        anim.SetFloat("Speed", movement.velocity.magnitude);

    }

    void UpdateState()
    {
        if (player.enabled == false)
        {
            //go to startPosition when player die (Action onPlayerDeath)
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieStates.STAND:
                if (distance <= followDistance)
                {
                    CheckPlayer(distance);
                }
                //check field of view
                break;
            case ZombieStates.MOVE:
                if (distance <= attackDistance)
                {
                    ChangeState(ZombieStates.ATTACK);
                }
                if (distance >= outOfFallowDistance)
                {
                    ChangeState(ZombieStates.STAND);
                }
                Rotate();
                break;
            case ZombieStates.ATTACK:
                if (distance > attackDistance)
                {
                    ChangeState(ZombieStates.MOVE); 

                }
                nextAttack -= Time.fixedDeltaTime;
                if (nextAttack <= 0)
                {
                    anim.SetTrigger("Shoot");

                    nextAttack = attackRate;
                }
                Rotate();
                if (rb.velocity.magnitude > 0)// make velocity zero when zombie move on STATE ATTACK
                {
                    rb.velocity = Vector2.zero;
                }
                break;
        }
    }

    private void CheckPlayer(float distance)
    {
        Vector2 playerDirection = player.transform.position - transform.position;
        Vector3 lookDirection = -transform.up;

        float angle = Vector2.Angle(lookDirection, playerDirection);

        if (angle <= searchAngle)
        {
            LayerMask mask = LayerMask.GetMask("Walls");
            Vector2 direction = player.transform.position - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, mask);

            if (hit.collider == null)
            {
                ChangeState(ZombieStates.MOVE);
            }
        }

    }

    void ChangeState(ZombieStates newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieStates.STAND:
                target.target = startPosition.transform;
                break;
            case ZombieStates.MOVE:
                movement.enabled = true;
                target.target = player.transform;
                break;
            case ZombieStates.ATTACK:
                movement.enabled = false;
                break;
        }
    }

    void Rotate()
    {
        Vector2 direction = player.transform.position - transform.position;
        transform.up = -direction;
    }



    public void DoDamageToPlayer()
    {
        player.DoDamage(damage);
    }

    public void DoDamage(float damage)
    {
        health -= damage;

        onHealthChanged();

        if (health <= 0)
        {
            ZombieDie();
        }

    }

    private void ZombieDie()
    {
        death = true;
        anim.SetBool("Death", true);
        this.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        movement.enabled = false;
        target.enabled = false;
        Destroy(rb);

        //Instantiate ammo when zombie die
        
        LeanPool.Spawn(ammo, transform.position, Quaternion.identity);
        

        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }
        if (gameObject.CompareTag("Boss"))
        {
            StartCoroutine(BossDeathDelay(3));

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outOfFallowDistance);

        Gizmos.color = Color.magenta;
        Vector3 lookDirection = -transform.up;
        Gizmos.DrawRay(transform.position, lookDirection * followDistance);

        //Quaternion rotation = Quaternion.AngleAxis(searchAngle, Vector3.forward);
        Vector3 v1 = Quaternion.AngleAxis(searchAngle, Vector3.forward) * lookDirection;
        Vector3 v2 = Quaternion.AngleAxis(-searchAngle, Vector3.forward) * lookDirection;

        Gizmos.DrawRay(transform.position, v1 * followDistance);
        Gizmos.DrawRay(transform.position, v2 * followDistance);


    }

    IEnumerator BossDeathDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.LoadNextLevel();
    }

    //when player die change zombie state to STAND and movement ON
    void ChangeZombieStateToSTAND()
    {
        if (!death)
        {
            movement.enabled = true;
            ChangeState(ZombieStates.STAND);
            player.onPlayerDeath -= ChangeZombieStateToSTAND;
        }
    }


}
