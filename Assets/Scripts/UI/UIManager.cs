using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerHUD hud;

    private void Start()
    {
        hud.SetBloodBar(1);
        hud.SetHeatBar(0);
    }

    private void FixedUpdate()
    {
        // float Percentage = PlayerManager.instance.player.GetComponent<CharacterGeneral>().currentHealth/PlayerManager.instance.player.GetComponent<CharacterGeneral>().maxHealth;
        hud.SetBloodBar(PlayerManager.instance.player.GetComponent<CharacterGeneral>().HealthPercentage);
        hud.SetHeatBar(PlayerManager.instance.player.GetComponent<Heat>().HeatPercentage);
    }
}
