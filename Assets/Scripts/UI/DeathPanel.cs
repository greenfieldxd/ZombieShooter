using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanel : MonoBehaviour
{
   

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        player.onPlayerDeath += DeathPlayerPanel;
    }

    void DeathPlayerPanel()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
