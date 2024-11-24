using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerHUD hud;
    [Header("Events Monitoring")] 
    public PlayerEventSO HeatEvent;


    private void OnEnable()
    {
        HeatEvent.OnEventRaised += OnHeatEvent;
    }

    private void OnHeatEvent(CharacterGeneral Character)
    {
        float Percentage = Character.currentHealth/Character.maxHealth;
        hud.SetHeatBar(Percentage);
        hud.SetBloodBar(Percentage);
    }

    private void OnDisable()
    {
        HeatEvent.OnEventRaised -= OnHeatEvent;
        
    }
}
