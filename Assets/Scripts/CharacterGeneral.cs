using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class CharacterGeneral : MonoBehaviour
{
    [Header("Character General")]
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float HealthPercentage => currentHealth / maxHealth;
    public float maxHeat = 100;
    public float currentHeat = 0;
    public float HeatPercentage => currentHeat / maxHeat;

    [Header("Character Events")]
    public UnityEvent OnDie;
    [Range(-10.0f, 20.0f)] public float boomHeight = 1.0f;

    // public ParticleSystem DieParticleSystem;
    public VisualEffect DieExplosion;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(Attacker attacker)
    {
        currentHealth -= attacker.damage;

        if (currentHealth <= 0)
        {
            Die();
            
            // if (DieParticleSystem != null)
            // {
            //     Vector3 boomPos = transform.position + new Vector3(0, boomHeight, 0);
            //     Instantiate(DieParticleSystem, boomPos, transform.rotation);
            // }
          if (DieExplosion != null)
            {
                Vector3 boomPos = transform.position + new Vector3(0, boomHeight, 0);
                Instantiate(DieExplosion , boomPos, transform.rotation);
            }
            currentHealth = maxHealth;
        }
    }
    private void Die()
    {
        // Debug.Log("Character died");
        // Broadcast the Die event 
        OnDie?.Invoke();
    }
}
