using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = -transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)// cheack if object was alredy despawn
        {
            LeanPool.Despawn(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "MedicineChest")
        {
            LeanPool.Despawn(gameObject);
        }
    }




}
