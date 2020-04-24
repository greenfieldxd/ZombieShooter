using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("AI config")]
    public float followDistance;
    public float attackDistance;

    [Header("Attack config")]
    public float attackRate;
    public float damage;

    

    enum ZombieStates
    {
        STAND,
        MOVE,
        ATTACK
    }

    ZombieStates activeState;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        ChangeState(ZombieStates.STAND);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        switch (activeState)
        {
            case ZombieStates.STAND:
                //DO STAND
                break;
            case ZombieStates.MOVE:
                //DO MOVE
                break;
            case ZombieStates.ATTACK:
                //DO ATTACK
                break;
        }
    }

    void ChangeState(ZombieStates newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieStates.STAND:
                //DO STAND
                break;
            case ZombieStates.MOVE:
                //DO MOVE
                break;
            case ZombieStates.ATTACK:
                //DO ATTACK
                break;
        }
    }




}
