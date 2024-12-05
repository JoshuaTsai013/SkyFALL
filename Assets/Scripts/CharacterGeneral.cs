using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterGeneral : MonoBehaviour
{
    [Header("Character General")]
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float HealthPercentage => currentHealth / maxHealth;
    public float maxHeat = 100;
    public float currentHeat = 0;
    public float HeatPercentage => currentHeat / maxHeat;

    //新增解體功能
    public MinionDestruct SelfDestroy;
    // public bool getdestruct = false;
    [Range(-10.0f, 20.0f)] public float boomHeight = 1.0f;

    public ParticleSystem DieParticleSystem;

    private void Start()
    {
        currentHealth = maxHealth;
        SelfDestroy = GetComponent<MinionDestruct>();
    }


    public void TakeDamage(Attacker attacker)
    {
        currentHealth -= attacker.damage;

        if (currentHealth <= 0)
        {
            // Die();
            // Debug.Log("Character died");
            // getdestruct = true;
        if (SelfDestroy != null)
        {
            SelfDestroy.SelfDestroy();
        }
            if (DieParticleSystem != null)
            {
                Vector3 boomPos = transform.position + new Vector3(0, boomHeight, 0);
                Instantiate(DieParticleSystem, boomPos, transform.rotation);
            }

            currentHealth = maxHealth;
        }
    }
}
