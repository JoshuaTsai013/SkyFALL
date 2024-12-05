using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

public class MinionGun : MonoBehaviour
{
    [SerializeField]
    private int Damage = 10;
    [SerializeField]
    private VisualEffect ShootingSystem;
    [SerializeField]
    private Transform GunRoot;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.1f;
    [SerializeField]
    private float Speed = 100;
    [SerializeField]
    private float LastShootTime;



    void Start()
    {
        // gameObject.SetActive(false); // disable self
    }
    private void Update()
    {
        Shoot();
    }
    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            Vector3 direction = GunRoot.forward;
            TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, direction));
            ShootingSystem.Play();
            // trail.GetComponent<Rigidbody>().velocity = direction * Speed;
            // trail.GetComponent<Attacker>().damage = Damage;
            // trail.GetComponent<MinionBullet>().GunRoot = GunRoot;

            LastShootTime = Time.time;
        }
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 _direction)
    {
        Trail.GetComponent<Rigidbody>().velocity = _direction * Speed;
        Trail.GetComponent<Attacker>().damage = Damage;
        Trail.GetComponent<MinionBullet>().GunRoot = GunRoot;
        yield return null;

    }
    }