using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;
using UnityEngine.Audio;

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

    public AudioSource shootSound;



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
            AudioSource.PlayClipAtPoint(shootSound.clip, transform.position);
            // trail.GetComponent<Rigidbody>().velocity = direction * Speed;
            // trail.GetComponent<Attacker>().damage = Damage;
            // trail.GetComponent<MinionBullet>().GunRoot = GunRoot;

            LastShootTime = Time.time;
        }
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 _direction)
    {
        if (Trail.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = _direction * Speed;
        }
        if (Trail.TryGetComponent<Attacker>(out Attacker attacker))
        {
            attacker.damage = Damage;
        }
        if (Trail.TryGetComponent<MinionBullet>(out MinionBullet minionBullet))
        {
            minionBullet.GunRoot = GunRoot;
        }
        yield return null;

    }
    }