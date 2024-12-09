using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class ThirdPersonShooterController : MonoBehaviour
{
    public GameObject shootCam;
    public GameObject mainCam;
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
        }
        else
        {
            isAiming = false;
            shootCam.SetActive(false);
            cross.SetActive(false);
            _animator.SetBool("Aim", false);
            _animator.SetFloat("GunAngle", 0f);
        }
        
        if (Inputs.shoot)
        {
            _gun.Shoot();
            _animator.SetBool("Shoot", true);
        }
        else
        {
            _animator.SetBool("Shoot", false);
        }
    }

    private void GunAngle()
    {
        _GunAngle = mainCam.transform.eulerAngles.x;
        if (_GunAngle > 180)
        {
            _GunAngle -= 360;
        }

        _animator.SetFloat("GunAngle", _GunAngle);
    }
}
