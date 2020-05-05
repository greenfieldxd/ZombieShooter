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

        healthSlider.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = player.health;
    }
}
