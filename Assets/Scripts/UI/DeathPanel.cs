using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
   
    public Image deathPanel;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        player.onPlayerDeath += DeathPlayerPanel;
    }

    void DeathPlayerPanel()
    {
        deathPanel.gameObject.SetActive(true);
    }

}
