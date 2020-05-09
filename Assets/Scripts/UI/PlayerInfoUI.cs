using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public Slider healthSlider;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        healthSlider.maxValue = player.health;
        healthSlider.value = player.health;

        player.onHealthChange += UpdateHealth;
    }

    void UpdateHealth()
    {
        healthSlider.value = player.health;
    }

    
}
