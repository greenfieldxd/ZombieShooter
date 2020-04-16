using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float inputX = (Input.GetAxis("Horizontal"));
        float inputY = (Input.GetAxis("Vertical"));

        rb.velocity = new Vector2(inputX, inputY) * speed;
        anim.SetFloat("Speed", rb.velocity.magnitude);



        //Vector3 newPosition = transform.position;
        //newPosition.x += speed * Time.deltaTime * inputX;
        //transform.position = newPosition;
    }

    private void Rotate()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouseWorldPosition - transform.position;
        transform.up = -direction;
    }


}
