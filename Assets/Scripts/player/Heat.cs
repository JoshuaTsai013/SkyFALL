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
    void Start()
    {
        currentHeat = 0;
    }

    public void AddJumpHeat()
    {
        currentHeat += JumpHeat;
    }

    public void AddDashHeat()
    {
        currentHeat += DashHeat;
    }

    private void FixedUpdate()
    {
        HeatPercentage = currentHeat / maxHeat;
        if(currentHeat >= maxHeat)
        {
            currentHeat = maxHeat;
        }
      

        if (currentHeat > 0)
        {
            currentHeat -= 0.1f;
        }
        else
        {
            currentHeat = 0;
        }
    }

}
