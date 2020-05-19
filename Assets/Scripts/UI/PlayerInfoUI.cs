using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public Slider healthSlider;
    public Text ammoText;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        healthSlider.maxValue = player.health;
        healthSlider.value = player.health;

        player.onHealthChange += UpdateHealth;
        player.onPlayerAmmo += UpdateAmmo;

        UpdateAmmo();
    }

    void UpdateHealth()
    {
        healthSlider.value = player.health;
    }

    void UpdateAmmo()
    {
        ammoText.text = "Ammo: " + player.ammo;
    }

    
}
