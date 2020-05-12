using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zombie : MonoBehaviour
{
    public Action onHealthChanged = delegate { };

    [Header("AI config")]
    public float followDistance;
    public float attackDistance;
    public float outOfFallowDistance;

    [Header("Attack, health config")]
    public float attackRate;
    public float damage;
    public float health;
    public float maxHealth;

    float nextAttack;



    enum ZombieStates
    {
        STAND,
        MOVE,
        ATTACK
    }

    ZombieStates activeState;

    Player player;

    ZombieMovement movement;
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        movement = GetComponent<ZombieMovement>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        ChangeState(ZombieStates.STAND);
        player.onPlayerDeath += ChangeZombieStateToSTAND;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState();

        anim.SetFloat("Speed", rb.velocity.magnitude);

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
                    LayerMask mask = LayerMask.GetMask("Walls");
                    Vector2 direction = player.transform.position - transform.position;

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, mask);

                    if (hit.collider == null)
                    {
                        ChangeState(ZombieStates.MOVE);
                    }
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
                    anim.SetTrigger("Move");

                }
                nextAttack -= Time.fixedDeltaTime;
                if (nextAttack <= 0)
                {
                    anim.SetTrigger("Shoot");

                    nextAttack = attackRate;
                }
                Rotate();
                break;
        }
    }

    void ChangeState(ZombieStates newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieStates.STAND:
                movement.moveToPlayer = false;
                break;
            case ZombieStates.MOVE:
                movement.enabled = true;
                movement.moveToPlayer = true;
                break;
            case ZombieStates.ATTACK:
                movement.enabled = false;
                movement.StopMovement();
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
        anim.SetBool("Death", true);
        this.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        movement.enabled = false;
        movement.StopMovement();

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
        movement.enabled = true;
        ChangeState(ZombieStates.STAND);
    }


}
