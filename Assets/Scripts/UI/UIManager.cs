using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerHUD hud;
    GameObject player;
    CharacterGeneral characterGeneral;
    Heat heat;

    private void Start()
    {
        player = PlayerManager.instance.player;
        characterGeneral = player.GetComponent<CharacterGeneral>();
        heat = player.GetComponent<Heat>();
        hud.SetBloodBar(1);
        hud.SetHeatBar(0);
    }

    private void FixedUpdate()
    {
        // float Percentage = PlayerManager.instance.player.GetComponent<CharacterGeneral>().currentHealth/PlayerManager.instance.player.GetComponent<CharacterGeneral>().maxHealth;
        hud.SetBloodBar(characterGeneral.HealthPercentage);
        hud.SetHeatBar(heat.HeatPercentage);
    }
}
