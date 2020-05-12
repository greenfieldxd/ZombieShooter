using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{

    public float speed;
    public bool moveToPlayer;

    public GameObject startPosition;

    //other gameObjects
    Player player;


    //components on the same gameObject
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = Instantiate(startPosition, transform.position, Quaternion.identity);

        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveToPlayer)
        {
            FollowPlayer();
        }
        else
        {
            GoToStartPosition();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        rb.velocity = direction.normalized * speed;
    }

    private void GoToStartPosition()
    {
        float distance = Vector2.Distance(transform.position, startPosition.transform.position);
        if (distance < 0.2f)
        {
            StopMovement();
            return;
        }
        Vector2 direction = startPosition.transform.position - transform.position;
        rb.velocity = direction.normalized * speed;
        Rotate();
    }


    void Rotate()
    {
        Vector2 direction = startPosition.transform.position - transform.position;
        transform.up = -direction;
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }

}
