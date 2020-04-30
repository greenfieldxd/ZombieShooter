using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("AI config")]
    public float followDistance;
    public float attackDistance;
    public float outOfFallowDistance;

    [Header("Attack, health config")]
    public float attackRate;
    public float damage;
    public float health;

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
            //go to startPosition
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieStates.STAND:
                if (distance <= followDistance)
                {
                    ChangeState(ZombieStates.MOVE);
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
                movement.enabled = false;
                movement.StopMovement();
                break;
            case ZombieStates.MOVE:
                movement.enabled = true;
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

        if (health <= 0)
        {
            ZombieDie();
        }

    }

    private void ZombieDie()
    {
        this.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        movement.enabled = false;
        movement.StopMovement();
        anim.SetBool("Death", true);
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


}
