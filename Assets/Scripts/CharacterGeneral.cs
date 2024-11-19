using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGeneral : MonoBehaviour
{
    [Header("Character General")]
    public float maxHealth = 100;
    public float currentHealth = 100;

    public ParticleSystem DieParticleSystem;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(Attacker attacker)
    {
        currentHealth -= attacker.damage;
        if (currentHealth <= 0)
        {
            // Die();
            // Debug.Log("Character died");
            // DieParticleSystem.Play();
            if (DieParticleSystem != null)
            {
                Instantiate(DieParticleSystem, transform.position, transform.rotation);
            }

            currentHealth = 100;
        }
    }

}
