using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineChest : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)  //Player layer
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.health < 100)
            {
                player.health = 100;
                Destroy(gameObject);
            }
        }
    }

  

}
