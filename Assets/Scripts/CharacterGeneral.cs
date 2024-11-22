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

    [Range(-10.0f, 20.0f)] public float boomHeight = 1.0f;

    public ParticleSystem DieParticleSystem;
    public UnityEvent<CharacterGeneral> OnHeatChange;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHeatChange?.Invoke(this);
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
                Vector3 boomPos = transform.position + new Vector3(0, boomHeight, 0);
                Instantiate(DieParticleSystem, boomPos, transform.rotation);
            }

            currentHealth = maxHealth;
        }
        OnHeatChange?.Invoke(this);
    }

}
