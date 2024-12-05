using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBullet : MonoBehaviour
{
    [SerializeField]
    private float maxLiftTime = 5f;
    [SerializeField]
    private LayerMask Mask;
    public ParticleSystem ImpactParticleSystem;

    public Transform GunRoot;

    private void Start()
    {
        Destroy(gameObject, maxLiftTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        Invoke("Delay", 0.01f);
        Physics.Raycast(GunRoot.position, GunRoot.forward.normalized, out RaycastHit hit, Mask);
        Debug.DrawRay(GunRoot.position, GunRoot.forward.normalized * hit.distance, Color.red, 2.0f);
        Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        
    }
    private void Delay()
    {
        Destroy(gameObject);
    }
}
