using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    public float JumpHeat = 10;
    public float DashHeat = 10;
    public float maxHeat = 100;
    public float currentHeat = 0;
    public float HeatPercentage;
    public bool Overheated = false;

    public float ColdDownRate = 0.1f;
    public float OverheatColdDownRate = 0.1f;

    private ThirdPersonShooterController _shooterController;

    private bool _canColdDown = false;

    void Start()
    {
        currentHeat = 0;
        _shooterController = PlayerManager.instance.player.GetComponent<ThirdPersonShooterController>();
    }

    public void AddJumpHeat()
    {
        currentHeat += JumpHeat;
    }

    public void AddDashHeat()
    {
        if (Overheated)
        {
            return;
        }
        currentHeat += DashHeat;
    }

    private void FixedUpdate()
    {
        HeatPercentage = currentHeat / maxHeat;
        if (currentHeat >= maxHeat && !Overheated)
        {
            //Overheat 
            // call vfx camera shake
            
            currentHeat = maxHeat;
            
            Overheated = true; //will be used to disable dash and jump in ThirdPersonUserControl script
            _canColdDown = false;
            Invoke("CanColdDownAfterOverheat", 2.0f);
            // _shooterController.enabled = false;
        }

        if (currentHeat > 0 && !Overheated)
        {
            currentHeat -= ColdDownRate * Time.deltaTime;
            if (currentHeat < 0)
            {
                currentHeat = 0;
            }
        }

        if (Overheated && _canColdDown)
        {
            currentHeat -= OverheatColdDownRate * Time.deltaTime;
            if (currentHeat <= 0)
            {
                currentHeat = 0;
                Overheated = false;
                // _shooterController.enabled = true;
            }
        }
    }
    private void CanColdDownAfterOverheat()
    {
        _canColdDown = true;
    }

}
