using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    public GameObject shootCam;
    public GameObject cross;

    public Animator _animator;

    public bool isAiming;

    private PlayerInputs Inputs;
    [SerializeField]
    private Gun _gun;

    private float _GunAngle;
    
    private void Start()
    {
        Inputs = GetComponent<PlayerInputs>();
    }
    private void Update()
    {
        if (Inputs.aim)
        {
            isAiming = true;
            shootCam.SetActive(true);
            cross.SetActive(true);
            _animator.SetBool("Aim", true);
            GunAngle();


            //_animator.SetFloat("GunAngle", 1);
        }
        else
        {
            isAiming = false;
            shootCam.SetActive(false);
            cross.SetActive(false);
            _animator.SetBool("Aim", false);
        }
        
        if (Inputs.shoot)
        {
            _gun.Shoot();
        }


    }

    private void GunAngle()
    {
        _GunAngle = Mathf.Clamp(shootCam.transform.rotation.x, -1f, 1f);

        _animator.SetFloat("GunAngle", _GunAngle);
    }
}
